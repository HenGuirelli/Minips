using Minips2.Memories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Minips2.Data
{
    public enum DataTypeFile
    {
        Data,
        Rodata
    }

    public class DataLoader
    {
        private readonly string _data;
        private readonly DataTypeFile _datatypeFile;

        private DataLoader(string data, DataTypeFile datatypeFile)
        {
            _data = data;
            _datatypeFile = datatypeFile;
        }

        public static DataLoader Empty(DataTypeFile dataTypeFile = DataTypeFile.Data)
        {
            return new DataLoader(string.Empty, dataTypeFile);
        }

        public static DataLoader FromFile(string filename)
        {
            if (filename.EndsWith(".rodata") && !File.Exists(filename)) return Empty(DataTypeFile.Rodata);

            using (var fs = new FileStream(filename, FileMode.Open))
            {
                int hexIn;
                var hex = string.Empty;

                for (int i = 0; (hexIn = fs.ReadByte()) != -1; i++)
                {
                    hex += string.Format("{0:X2}", hexIn);
                }
                return new DataLoader(hex, filename.EndsWith(".data") ? DataTypeFile.Data : DataTypeFile.Rodata);
            }
        }

        // https://stackoverflow.com/questions/1450774/splitting-a-string-into-chunks-of-a-certain-size
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
    }
}
