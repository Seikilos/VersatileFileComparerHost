using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BundledPlugin.DDS
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dds-pixelformat
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 32, Pack = 1, CharSet = CharSet.Ansi)]
    public class DDSPixelFormat
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] dwFourCC;

        public string dwFourCCString
        {
            get { return string.Join("", dwFourCC.TakeWhile(c => c != 0).Select(Convert.ToChar)); }
        }



        public uint dwRGBBitCount;
        public uint dwRBitMask;
        public uint dwGBitMask;
        public uint dwBBitMask;
        public uint dwABitMask;

        public override string ToString()
        {
            return $"{nameof(dwSize)}: {dwSize}, {nameof(dwFlags)}: {dwFlags}, {nameof(dwFourCC)}: {dwFourCCString}, {nameof(dwRGBBitCount)}: {dwRGBBitCount}, {nameof(dwRBitMask)}: {dwRBitMask}, {nameof(dwGBitMask)}: {dwGBitMask}, {nameof(dwBBitMask)}: {dwBBitMask}, {nameof(dwABitMask)}: {dwABitMask}";
        }

        public bool Equals(DDSPixelFormat other)
        {
            return dwSize == other.dwSize && dwFlags == other.dwFlags && Equals(dwFourCCString, other.dwFourCCString) && dwRGBBitCount == other.dwRGBBitCount && dwRBitMask == other.dwRBitMask && dwGBitMask == other.dwGBitMask && dwBBitMask == other.dwBBitMask && dwABitMask == other.dwABitMask;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DDSPixelFormat && Equals((DDSPixelFormat) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) dwSize;
                hashCode = (hashCode * 397) ^ (int) dwFlags;
                hashCode = (hashCode * 397) ^ (dwFourCC != null ? dwFourCC.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) dwRGBBitCount;
                hashCode = (hashCode * 397) ^ (int) dwRBitMask;
                hashCode = (hashCode * 397) ^ (int) dwGBitMask;
                hashCode = (hashCode * 397) ^ (int) dwBBitMask;
                hashCode = (hashCode * 397) ^ (int) dwABitMask;
                return hashCode;
            }
        }

        public static bool operator ==(DDSPixelFormat left, DDSPixelFormat right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DDSPixelFormat left, DDSPixelFormat right)
        {
            return !left.Equals(right);
        }
    }
}

