using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundledPlugin.DDS
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dds-pixelformat
    /// </summary>
    public struct DDSPixelFormat
    {
        [Flags]
        public enum EPixelFlags
        {
            DDPF_ALPHAPIXELS = 0x1, // Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
            DDPF_ALPHA = 0x2, // Used in some older DDS files for alpha channel only uncompressed data (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data)
            DDPF_FOURCC = 0x4, // Texture contains compressed RGB data; dwFourCC contains valid data.
            DDPF_RGB = 0x40, // Texture contains uncompressed RGB data; dwRGBBitCount and the RGB masks (dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
            DDPF_YUV = 0x200, // Used in some older DDS files for YUV uncompressed data (dwRGBBitCount contains the YUV bit count; dwRBitMask contains the Y mask, dwGBitMask contains the U mask, dwBBitMask contains the V mask)
            DDPF_LUMINANCE = 0x2000, // Used in some older DDS files for single channel color uncompressed data (dwRGBBitCount contains the luminance channel bit count; dwRBitMask contains the channel mask). Can be combined with DDPF_ALPHAPIXELS for a two channel DDS file.
        }

        public uint dwSize;
        public EPixelFlags dwFlags;
        public uint dwFourCC;
        public uint dwRGBBitCount;
        public uint dwRBitMask;
        public uint dwGBitMask;
        public uint dwBBitMask;
        public uint dwABitMask;

        public override string ToString()
        {
            return $"{nameof(dwSize)}: {dwSize}, {nameof(dwFlags)}: {dwFlags}, {nameof(dwFourCC)}: {dwFourCC}, {nameof(dwRGBBitCount)}: {dwRGBBitCount}, {nameof(dwRBitMask)}: {dwRBitMask}, {nameof(dwGBitMask)}: {dwGBitMask}, {nameof(dwBBitMask)}: {dwBBitMask}, {nameof(dwABitMask)}: {dwABitMask}";
        }
    }
}

