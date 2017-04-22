using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using BeardBook.Entities;
using File = BeardBook.Entities.File;

namespace BeardBook.DAL
{
    public static class FileService
    {
        public const int DefaultMaxPixels = 64;

        public static File CreateThumbnail(byte[] fileBytes, int maxPixels = DefaultMaxPixels)
        {
            Image image;
            try
            {
                image = (Bitmap)new ImageConverter().ConvertFrom(fileBytes);
            }
            catch (Exception)
            {
                return new File {ContentType = "image/png"};
            }
            var size = GetThumbnailSize(image, maxPixels);
            var thumbnailImage = image?.GetThumbnailImage(
                size.Width,
                size.Height,
                () => false,
                IntPtr.Zero);

            var stream = new MemoryStream();
            thumbnailImage?.Save(stream, ImageFormat.Png);

            return new File
            {
                ContentType = "image/png",
                Data = stream.ToArray()
            };
        }

        private static Size GetThumbnailSize(Image original, int maxPixels)
        {
            var originalWidth = original.Width;
            var originalHeight = original.Height;

            var factor = originalWidth > originalHeight
                ? (double)maxPixels / originalWidth
                : (double)maxPixels / originalHeight;

            return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
        }

        public static FileType GetFileTypeFrom(string contentType)
        {
            return contentType.Contains("image")
                ? FileType.Photo
                : (contentType.Contains("video")
                ? FileType.Video
                : FileType.Other);
        }
    }
}
