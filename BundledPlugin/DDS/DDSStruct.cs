using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace BundledPlugin.DDS
{
    /// <summary>
    /// The entire data structure for DDS. Actually a class to avoid copying int around in c#
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 128,Pack = 1, CharSet = CharSet.Ansi)]
    public class DDSStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] DwMagic;
        public string DwMagicString
        {
            get { return string.Join("", DwMagic.TakeWhile(c => c != 0).Select(Convert.ToChar)); }
        }


        public DDSHeader Header;


        public override string ToString()
        {
            return $"{nameof(DwMagic)}: {DwMagicString}, {nameof(Header)}: {Header}";
        }

        public bool Equals(DDSStruct other)
        {
            return string.Equals(DwMagicString, other.DwMagicString) && Header.Equals(other.Header);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DDSStruct && Equals((DDSStruct) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DwMagic != null ? DwMagic.GetHashCode() : 0) * 397) ^ Header.GetHashCode();
            }
        }

        public static bool operator ==(DDSStruct left, DDSStruct right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DDSStruct left, DDSStruct right)
        {
            return !left.Equals(right);
        }
    }
}
