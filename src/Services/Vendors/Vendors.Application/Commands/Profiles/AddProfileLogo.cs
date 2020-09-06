using LoadLogic.Services.Vendors.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Linq;
using LoadLogic.Services.Vendors.Application.Interfaces;
using LoadLogic.Services.Exceptions;

namespace LoadLogic.Services.Vendors.Application.Commands.Profiles
{
    /// <summary>
    /// An immutable command message for adding a profile logo to storage.
    /// </summary>
    /// <remarks>
    /// Logos are always added to blob storage, even if a previous logo exist.
    /// This is to handle the situation where logo urls are currently in use in 
    /// other systems.
    /// </remarks>
    public sealed class AddProfileLogo : IRequest<string>
    {
        public AddProfileLogo(string contentType, long length, Stream content)
        {
            this.ContentType = contentType;
            this.Length = length;
            this.Content = content;
        }

        /// <summary>
        /// Gets the raw Content-Type header of the uploaded file.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// Gets the file length in bytes.
        /// </summary>
        public long Length { get; }

        /// <summary>
        ///  Logo file contents.
        /// </summary>
        public Stream Content { get; }

    }

    internal class AddProfileLogoHandler : IRequestHandler<AddProfileLogo, string>
    {
        /// <summary>
        /// All profile logos will be stored in the public container.
        /// </summary>
        private const string LogoContainerName = "public";

        /// <summary>
        /// Maximum Logo size is 2 MB.
        /// </summary>
        private const long MaximumContentLength = 2097152;


        private readonly IMediator _mediator;
        private readonly ICrudRepository<Profile> _profileRepo;
        private readonly IBlobService _blobService;

        public AddProfileLogoHandler(
            IMediator mediator,
            ICrudRepository<Profile> profileRepo,
            IBlobService blobService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _profileRepo = profileRepo ?? throw new ArgumentNullException(nameof(profileRepo));
            _blobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
        }

        public async Task<string> Handle(AddProfileLogo request, CancellationToken cancellationToken)
        {
            var spec = new UniqueProfileSpec();
            var profile = await _profileRepo.FindOneAsync(spec);
            if (profile == null)
            {
                // FIXME: Profile Id
                throw new NotFoundException(nameof(Profile), 1);
            }

            if (!IsValidImageSize(request.Length))
            {
                throw new InvalidContentLengthException(request.Length, MaximumContentLength);
            }

            var isSupported = await IsSupportedMediaType(request.ContentType, request.Content);
            if (!isSupported)
            {
                throw new InvalidImageFormatException(request.ContentType);
            }

            request.Content.Position = 0;
            var fileName = Guid.NewGuid().ToString();
            var uri = await _blobService.UploadFileAsync(
                request.Content,
                LogoContainerName,
                fileName,
                cancellationToken);

            var url = uri.ToString();
            profile.UpdateLogoUrl(url);
            await _profileRepo.UnitOfWork.SaveEntitiesAsync();
            return url;
        }

        private bool IsValidImageSize(long contentLength)
        {
            return contentLength <= MaximumContentLength;
        }

        private async Task<bool> IsSupportedMediaType(string contentType, Stream content)
        {
            if (!contentType.ToLower().Contains("image")) return false;

            using var memoryStream = new MemoryStream();
            await content.CopyToAsync(memoryStream);
            var bytes = memoryStream.ToArray();

            // BMP
            var bmp = Encoding.ASCII.GetBytes("BM");

            // PNG
            var png = new byte[] { 137, 80, 78, 71 };

            // JPEG
            var jpeg = new byte[] { 255, 216, 255, 224 };
            var jpeg2 = new byte[] { 255, 216, 255, 225 };

            if (bmp.SequenceEqual(bytes.Take(bmp.Length))) return true;
            if (png.SequenceEqual(bytes.Take(png.Length))) return true;
            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length))) return true;
            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length))) return true;

            return false;
        }
    }
}
