using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
            return Path.GetExtension(file).ToLower() == ".dds";
        }

        public void Handle(string fileA, string fileB)
        {
            var fileAStream = _io.ReadFile(fileA);


            DDSStruct aStruct;
            int count = Marshal.SizeOf(typeof(DDSStruct));
            byte[] readBuffer = new byte[count];
            BinaryReader reader = new BinaryReader(fileAStream);
            readBuffer = reader.ReadBytes(count);

            GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
            aStruct = (DDSStruct) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(DDSStruct));
            handle.Free();

            int x = 0;
        }
    }
}
