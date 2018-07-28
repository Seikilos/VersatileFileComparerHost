using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BundledPlugin.DDS;
using Contracts;

namespace BundledPlugin
{
    /// <summary>
    /// Reads DDS headers and compares them. This plugin does not check for sizes of data, only for matching headers.
    /// See header https://docs.microsoft.com/en-us/windows/desktop/direct3ddds/dds-header
    /// </summary>
    public class DDSHeaderComparer : IVFComparer
    {
        public string Explanation => "Compares DDS files by comparing their header";

        private IIO _io;

        public void Init(IIO io)
        {
            _io = io;
        }

        public bool WantsToHandle(string file)
        {
            return Path.GetExtension(file)?.ToLower() == ".dds";
        }

        public void Handle(string fileA, string fileB)
        {
            var structA = ReadDDSStruct(fileA);
            var structB = ReadDDSStruct(fileB);

            if (structA.Equals(structB) == false)
            {
                // Try to resolve most common issues, otherwise dump the string representation
                handleKnownDDSFields(ref structA, ref structB);

                // If code reached this, fields are not specifically known, throw to string
                throw new Exception($"DDS header{Environment.NewLine}{structA}{Environment.NewLine}is not equal to {Environment.NewLine}{structB}");
            }
        }

        /// <summary>
        /// Checks for the most common issues and provides an easier to read error message
        /// </summary>
        /// <param name="structA"></param>
        /// <param name="structB"></param>
        private void handleKnownDDSFields(ref DDSStruct structA, ref DDSStruct structB)
        {
            if (structA.Header.ddspf.dwFourCC != structB.Header.ddspf.dwFourCC)
            {
                throw new Exception($"Different formats found: {structA.Header.ddspf.dwFourCC} not equal to {structB.Header.ddspf.dwFourCC}");
            }

            if (structA.Header.DwMipMapCount != structB.Header.DwMipMapCount)
            {
                throw new Exception($"Different count of mip maps found: {structA.Header.DwMipMapCount} not equal to {structB.Header.DwMipMapCount}");
            }
        }

        public DDSStruct ReadDDSStruct(string file)
        {
            var fileAsStream = _io.ReadFile(file);

            int count = Marshal.SizeOf(typeof(DDSStruct));
            byte[] readBuffer = new byte[count];

            using (var reader = new BinaryReader(fileAsStream))
            {
                readBuffer = reader.ReadBytes(count);
            }

            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            var aStruct = (DDSStruct)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSStruct));
            handle.Free();

            return aStruct;
        }
    }
}
