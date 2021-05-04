using System.Text.RegularExpressions;

namespace LoadLogic.Services.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Test whether the string value is a valid HEX Color.
        /// It is assumed the '#' prefixes the hex value. 
        /// No support for RGBA hex color values.
        /// </summary>
        /// <remarks>
        /// Regex from http://stackoverflow.com/a/1636354/2343
        /// </remarks>
        /// <param name="color">The hex color to be validated</param>
        /// <returns>Boolean indicating if the hex color string is valid</returns>
        public static bool IsValidHexColor(this string color)
        {
            const string HEX_REGEX = "^#(?:[0-9a-fA-F]{3}){1,2}$";
            return Regex.Match(color, HEX_REGEX).Success;
        }
    }
}
