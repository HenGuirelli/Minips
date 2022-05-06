using System.Text;

namespace Minips2.Data
{
    public class DataFileContentConverter
    {
        private string _value;

        public DataFileContentConverter(string value)
        {
            _value = value.PadRight(8, '0');
        }

        public DataFileContentConverter ToLittleEndian()
        {
            var result = new StringBuilder();
            var aux = _value[6..8];
            result.Append(aux);
            aux = _value[4..6];
            result.Append(aux);
            aux = _value[2..4];
            result.Append(aux);
            aux = _value[0..2];
            result.Append(aux);

            return new DataFileContentConverter(result.ToString());
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
