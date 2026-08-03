using System;
using System.IO;
using System.Web;

namespace EVotingSystem
{
    /// <summary>
    /// Validates uploaded files before they are saved: whitelisted
    /// extension, size limit, and content magic-byte check that must
    /// match the declared extension.
    /// </summary>
    public static class UploadHelper
    {
        public const int MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        /// <summary>
        /// Returns null if the file is acceptable to save, otherwise a
        /// user-facing error message.
        /// </summary>
        public static string Validate(HttpPostedFile file)
        {
            if (file == null)
                return "No file selected.";
            return Validate(file.FileName, file.ContentLength, file.InputStream);
        }

        /// <summary>
        /// Core validation: whitelisted extension, size limit, and content
        /// magic-byte check that must match the declared extension.
        /// </summary>
        public static string Validate(string fileName, int contentLength, Stream input)
        {
            if (contentLength <= 0)
                return "No file selected.";

            if (contentLength > MaxFileSizeBytes)
                return "File size must be 5 MB or less.";

            string ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext))
                return "File has no extension.";
            ext = ext.ToLowerInvariant();
            if (Array.IndexOf(AllowedExtensions, ext) < 0)
                return "Only .jpg, .jpeg, .png and .gif image files are allowed.";

            byte[] header = new byte[8];
            int read = 0;
            if (input != null)
            {
                input.Position = 0;
                read = input.Read(header, 0, header.Length);
            }
            if (read < 4)
                return "File content is not a valid image.";

            bool isJpeg = header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
            bool isPng = header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47;
            bool isGif = header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x38;

            bool matches;
            if (ext == ".jpg" || ext == ".jpeg")
                matches = isJpeg;
            else if (ext == ".png")
                matches = isPng;
            else
                matches = isGif;

            if (!matches)
                return "File content does not match its extension.";

            return null;
        }
    }
}
