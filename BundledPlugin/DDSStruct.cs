using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BundledPlugin
{
    public struct DDSStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string dwMagic;

        public DDSHeader header;

        public struct DDSHeader
        {

            public uint size;

            public override string ToString()
            {
                return $"{nameof(size)}: {size}";
            }
        }

        public override string ToString()
        {
            return $"{nameof(dwMagic)}: {dwMagic}, {nameof(header)}: {header}";
        }
    }
}
