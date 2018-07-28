using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BundledPlugin.DDS
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dds-header
    /// </summary>
    public struct DDSHeader
    {
        [Flags]
        public enum EFlags
        {
            DDSD_CAPS = 0x1,    // Required in every .dds file.
            DDSD_HEIGHT = 0x2,  // Required in every .dds file.
            DDSD_WIDTH = 0x4,   // Required in every .dds file.
            DDSD_PITCH = 0x8,   // Required when pitch is provided for an uncompressed texture.
            DDSD_PIXELFORMAT = 0x1000,  // Required in every .dds file. 
            DDSD_MIPMAPCOUNT = 0x20000, // Required in a mipmapped texture.
            DDSD_LINEARSIZE = 0x80000,  // Required when pitch is provided for a compressed texture.
            DDSD_DEPTH = 0x800000,	// Required in a depth texture.

        }

        [Flags]
        public enum ECaps
        {
            DDSCAPS_COMPLEX = 0x8, // Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
            DDSCAPS_MIPMAP = 0x400000, // Optional; should be used for a mipmap.
            DDSCAPS_TEXTURE = 0x1000, // Required
        }

        [Flags]
        public enum ECaps2
        {
            DDSCAPS2_CUBEMAP = 0x200, // Required for a cube map. 								
            DDSCAPS2_CUBEMAP_POSITIVEX = 0x400, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_CUBEMAP_NEGATIVEX = 0x800, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_CUBEMAP_POSITIVEY = 0x1000, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_CUBEMAP_NEGATIVEY = 0x2000, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_CUBEMAP_POSITIVEZ = 0x4000, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x8000, // Required when these surfaces are stored in a cube map. 	
            DDSCAPS2_VOLUME = 0x200000, // Required for a volume texture. 							
        }

        public uint DwSize;


        public EFlags DwFlags;

        public uint DwHeight;
        public uint DwWidth;

        public uint DwPitchOrLinearSize;
        public uint DwDepth;
        public uint DwMipMapCount;

        // 11 x 4 byte size of reserved data
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11*4)]
        public string DwReserved1;

        public DDSPixelFormat ddspf;

        public ECaps DwCaps;

        public ECaps2 DwCaps2;

        // Unused
        public uint DwCaps3;
        public uint DwCaps4;
        public uint DwReserved2;


        public override string ToString()
        {
            return $"{nameof(DwSize)}: {DwSize}, {nameof(DwFlags)}: {DwFlags}, {nameof(DwHeight)}: {DwHeight}, {nameof(DwWidth)}: {DwWidth}, {nameof(DwPitchOrLinearSize)}: {DwPitchOrLinearSize}, {nameof(DwDepth)}: {DwDepth}, {nameof(DwMipMapCount)}: {DwMipMapCount}, {nameof(DwReserved1)}: {DwReserved1}, {nameof(ddspf)}: {ddspf}, {nameof(DwCaps)}: {DwCaps}, {nameof(DwCaps2)}: {DwCaps2}, {nameof(DwCaps3)}: {DwCaps3}, {nameof(DwCaps4)}: {DwCaps4}, {nameof(DwReserved2)}: {DwReserved2}";
        }

        public bool Equals(DDSHeader other)
        {
            return DwSize == other.DwSize && DwFlags == other.DwFlags && DwHeight == other.DwHeight && DwWidth == other.DwWidth && DwPitchOrLinearSize == other.DwPitchOrLinearSize && DwDepth == other.DwDepth && DwMipMapCount == other.DwMipMapCount && string.Equals(DwReserved1, other.DwReserved1) && ddspf.Equals(other.ddspf) && DwCaps == other.DwCaps && DwCaps2 == other.DwCaps2 && DwCaps3 == other.DwCaps3 && DwCaps4 == other.DwCaps4 && DwReserved2 == other.DwReserved2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DDSHeader && Equals((DDSHeader) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) DwSize;
                hashCode = (hashCode * 397) ^ (int) DwFlags;
                hashCode = (hashCode * 397) ^ (int) DwHeight;
                hashCode = (hashCode * 397) ^ (int) DwWidth;
                hashCode = (hashCode * 397) ^ (int) DwPitchOrLinearSize;
                hashCode = (hashCode * 397) ^ (int) DwDepth;
                hashCode = (hashCode * 397) ^ (int) DwMipMapCount;
                hashCode = (hashCode * 397) ^ (DwReserved1 != null ? DwReserved1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ddspf.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) DwCaps;
                hashCode = (hashCode * 397) ^ (int) DwCaps2;
                hashCode = (hashCode * 397) ^ (int) DwCaps3;
                hashCode = (hashCode * 397) ^ (int) DwCaps4;
                hashCode = (hashCode * 397) ^ (int) DwReserved2;
                return hashCode;
            }
        }
    }
}

