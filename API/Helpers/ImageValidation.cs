using System;
using System.IO;

namespace API.Helpers
{
    public class ImageValidation
    {
        public static bool IsImage(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            using (var br = new BinaryReader(stream))
            {
                var jpegSoi = br.ReadUInt16();
                var marker = br.ReadUInt16();
                br.BaseStream.Position = 0;
                var pngSoi = br.ReadUInt64();

                return jpegSoi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff || pngSoi == 0x0a1a0a0d474e5089;
            }
        }

        public static bool IsJpeg(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                var soi = br.ReadUInt16();
                var marker = br.ReadUInt16();
                return soi == 0xd8ff && (marker & 0xe0ff) == 0xe0ff;
            }
        }

        public static bool IsPng(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                var soi = br.ReadUInt64();
                return soi == 0x0a1a0a0d474e5089;
            }
        }

        public static bool IsGif(Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                var soi = br.ReadUInt32();
                var p2 = br.ReadUInt16();

                return soi == 0x38464947 && (p2 == 0x6137 || p2 == 0x6139);
            }
        }
    }
}
