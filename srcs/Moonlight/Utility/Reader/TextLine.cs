using System;

namespace Moonlight.Utility.Reader
{
    public class TextLine
    {
        private readonly string[] _content;
        private readonly char _separator;

        public TextLine(string[] content, char separator)
        {
            _content = content;
            _separator = separator;
        }

        public string GetValue(int index)
        {
            if (index >= _content.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _content[index];
        }

        public T GetValue<T>(int index) => (T)Convert.ChangeType(GetValue(index), typeof(T));

        public string[] GetValues() => _content;

        public string GetFirstValue() => _content[0];

        public string GetLastValue() => _content[_content.Length - 1];

        public T GetFirstValue<T>() => (T)Convert.ChangeType(GetFirstValue(), typeof(T));
        public T GetLastValue<T>() => (T)Convert.ChangeType(GetLastValue(), typeof(T));

        public bool StartWith(string value) => _content[0].Equals(value);
        public bool EndWith(string value) => _content[_content.Length - 1].Equals(value);

        public string AsString() => string.Join(_separator.ToString(), _content);
    }
}