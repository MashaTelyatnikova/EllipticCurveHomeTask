using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HomeTask
{
    public class StreamTokenizer
    {
        private IEnumerator<string> tokensEnumerator;
        private const int blockSize = 1024;

        public StreamTokenizer(TextReader inputStream)
        {
            tokensEnumerator = ReadStream(inputStream).GetEnumerator();
        }

        public string NextWord()
        {
            return tokensEnumerator.MoveNext() ? tokensEnumerator.Current : null;
        }

        public int NextInt()
        {
            return int.Parse(NextWord());
        }

        public void ProcessQueryCollection(Action<StreamTokenizer> processor)
        {
            var count = NextInt();
            for (int i = 0; i < count; ++i)
            {
                processor(this);
            }
        }

        public void ProcessSingleQuery(Action<StreamTokenizer> processor)
        {
            processor(this);
        }

        private IEnumerable<string> ReadStream(TextReader reader)
        {
            StringBuilder currentToken = new StringBuilder();
            char[] buffer = new char[blockSize];
            int readCount;
            while ((readCount = reader.Read(buffer, 0, blockSize)) != 0)
            {
                for (int i = 0; i < readCount; ++i)
                    if (!Char.IsWhiteSpace(buffer[i]))
                    {
                        currentToken.Append(buffer[i]);
                    }
                    else if (currentToken.Length != 0)
                    {
                        yield return currentToken.ToString();
                        currentToken.Clear();
                    }
            }
            if (currentToken.Length != 0)
                yield return currentToken.ToString();
        }
    }

}
