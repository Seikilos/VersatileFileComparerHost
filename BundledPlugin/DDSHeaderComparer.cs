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
                handleKnownDDSFields(structA, structB);

                // If code reached this, fields are not specifically known, throw to string
                throw new Exception($"DDS header{Environment.NewLine}{structA}{Environment.NewLine}is not equal to {Environment.NewLine}{structB}");
            }
        }

        /// <summary>
        /// Checks for the most common issues and provides an easier to read error message
        /// </summary>
        /// <param name="structA"></param>
        /// <param name="structB"></param>
        private void handleKnownDDSFields(DDSStruct structA, DDSStruct structB)
        {
            var errors = new List<string>();
           
            // If any dw flag is four CC, try to be more precise
            string formatA = structA.Header.ddspf.dwFlags == DDSPixelFormat.EPixelFlags.DDPF_FOURCC
                ? structA.Header.ddspf.dwFourCCString
                : structA.Header.ddspf.dwFlags.ToString();

            string formatB = structB.Header.ddspf.dwFlags == DDSPixelFormat.EPixelFlags.DDPF_FOURCC
                ? structB.Header.ddspf.dwFourCCString
                : structB.Header.ddspf.dwFlags.ToString();

            if (formatA != formatB)
            {
                errors.Add($"Format different: {formatA} not equal to {formatB}");
            }


            if (structA.Header.DwMipMapCount != structB.Header.DwMipMapCount)
            {
                errors.Add($"Mips different: {structA.Header.DwMipMapCount} not equal to {structB.Header.DwMipMapCount}");
              
            }

            if (errors.Any())
            {
                throw new Exception($"Differences found:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");
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
