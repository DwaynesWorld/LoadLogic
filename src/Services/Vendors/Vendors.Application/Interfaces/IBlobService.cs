using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoadLogic.Services.Vendors.Application.Interfaces
{
    public interface IBlobService
    {
        Task<Uri> UploadFileAsync(Stream content, string containerName, string fileName, CancellationToken cancellationToken = default);
        Task<Stream> GetFileAsync(string containerName, string fileName, CancellationToken cancellationToken = default);

    }

}
