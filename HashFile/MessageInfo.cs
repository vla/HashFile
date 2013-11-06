using System.Text;

namespace HasFile
{
    public class MessageInfo
    {
        private StringBuilder data = new StringBuilder(1000);

        public MessageInfo Append(StringBuilder value) {
            data.Append(value.ToString());
            value.Remove(0, value.Length);
            return this;
        }

        public void Append(string value) {
            data.Append(value);
        }

        public void AppendLine(string value) {
            data.AppendLine(value);
        }

        public void Clear() {
            data.Remove(0, data.Length);
        }

        public override string ToString() {
            return data.ToString();
        }
    }
}