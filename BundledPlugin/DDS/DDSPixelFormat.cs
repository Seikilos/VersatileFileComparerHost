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
            DDPF_LUMINANCE = 0x20000, // Used in some older DDS files for single channel color uncompressed data (dwRGBBitCount contains the luminance channel bit count; dwRBitMask contains the channel mask). Can be combined with DDPF_ALPHAPIXELS for a two channel DDS file.
        }

        /// <summary>
        /// Instead of 4 byte FOURCC flag use the https://docs.microsoft.com/en-us/windows/desktop/direct3d9/d3dformat
        /// data type which also reflects things like R16F
        /// </summary>
        public enum D3DFormat : uint
        {
            D3DFMT_UNKNOWN = 0,

            D3DFMT_R8G8B8 = 20,
            D3DFMT_A8R8G8B8 = 21,
            D3DFMT_X8R8G8B8 = 22,
            D3DFMT_R5G6B5 = 23,
            D3DFMT_X1R5G5B5 = 24,
            D3DFMT_A1R5G5B5 = 25,
            D3DFMT_A4R4G4B4 = 26,
            D3DFMT_R3G3B2 = 27,
            D3DFMT_A8 = 28,
            D3DFMT_A8R3G3B2 = 29,
            D3DFMT_X4R4G4B4 = 30,
            D3DFMT_A2B10G10R10 = 31,
            D3DFMT_A8B8G8R8 = 32,
            D3DFMT_X8B8G8R8 = 33,
            D3DFMT_G16R16 = 34,
            D3DFMT_A2R10G10B10 = 35,
            D3DFMT_A16B16G16R16 = 36,

            D3DFMT_A8P8 = 40,
            D3DFMT_P8 = 41,

            D3DFMT_L8 = 50,
            D3DFMT_A8L8 = 51,
            D3DFMT_A4L4 = 52,

            D3DFMT_V8U8 = 60,
            D3DFMT_L6V5U5 = 61,
            D3DFMT_X8L8V8U8 = 62,
            D3DFMT_Q8W8V8U8 = 63,
            D3DFMT_V16U16 = 64,
            D3DFMT_A2W10V10U10 = 67,

            D3DFMT_UYVY = 1431918169, // MAKEFOURCC('U', 'Y', 'V', 'Y'),
            D3DFMT_R8G8_B8G8 = 1380401735, //MAKEFOURCC('R', 'G', 'B', 'G'),
            D3DFMT_YUY2 = 1498765618, //MAKEFOURCC('Y', 'U', 'Y', '2'),
            D3DFMT_G8R8_G8B8 = 1196574530, //MAKEFOURCC('G', 'R', 'G', 'B'),
            D3DFMT_DXT1 = 1146639409, //MAKEFOURCC('D', 'X', 'T', '1'),
            D3DFMT_DXT2 = 1146639410, //MAKEFOURCC('D', 'X', 'T', '2'),
            D3DFMT_DXT3 = 1146639411, //MAKEFOURCC('D', 'X', 'T', '3'),
            D3DFMT_DXT4 = 1146639412, //MAKEFOURCC('D', 'X', 'T', '4'),
            D3DFMT_DXT5 = 1146639413, //MAKEFOURCC('D', 'X', 'T', '5'),

            D3DFMT_D16_LOCKABLE = 70,
            D3DFMT_D32 = 71,
            D3DFMT_D15S1 = 73,
            D3DFMT_D24S8 = 75,
            D3DFMT_D24X8 = 77,
            D3DFMT_D24X4S4 = 79,
            D3DFMT_D16 = 80,

            D3DFMT_D32F_LOCKABLE = 82,
            D3DFMT_D24FS8 = 83,


            D3DFMT_D32_LOCKABLE = 84,
            D3DFMT_S8_LOCKABLE = 85,


            D3DFMT_L16 = 81,

            D3DFMT_VERTEXDATA = 100,
            D3DFMT_INDEX16 = 101,
            D3DFMT_INDEX32 = 102,

            D3DFMT_Q16W16V16U16 = 110,

            D3DFMT_MULTI2_ARGB8 = 1296389169, // MAKEFOURCC('M','E','T','1'),

            D3DFMT_R16F = 111,
            D3DFMT_G16R16F = 112,
            D3DFMT_A16B16G16R16F = 113,

            D3DFMT_R32F = 114,
            D3DFMT_G32R32F = 115,
            D3DFMT_A32B32G32R32F = 116,

            D3DFMT_CxV8U8 = 117,

            D3DFMT_A1 = 118,
            D3DFMT_A2B10G10R10_XR_BIAS = 119,
            D3DFMT_BINARYBUFFER = 199,

            D3DFMT_FORCE_DWORD = 0x7fffffff
        }


        public uint dwSize;
        public EPixelFlags dwFlags;

        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] dwFourCC;

        public D3DFormat dwFourCCString
        {
            get { return (D3DFormat) BitConverter.ToUInt32(dwFourCC,0); }
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

