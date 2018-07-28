﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BundledPlugin.DDS;

namespace BundledPlugin
{
    /// <summary>
    /// The entire data structure for DDS
    /// </summary>
    public struct DDSStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string DwMagic;

        public DDSHeader Header;


        public override string ToString()
        {
            return $"{nameof(DwMagic)}: {DwMagic}, {nameof(Header)}: {Header}";
        }

        public bool Equals(DDSStruct other)
        {
            return string.Equals(DwMagic, other.DwMagic) && Header.Equals(other.Header);
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
    }
}