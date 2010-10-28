using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class FileWriter : ChainOperator
    {
        string _fullFilePath = String.Empty;
        bool _isClosed = false;

        public FileWriter(string fullFilePath)
            : base()
        {
            _fullFilePath = fullFilePath;
        }

        public FileWriter(IOperation next, string fullFilePath)
            : base(next)
        {
            _fullFilePath = fullFilePath;
        }

        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

        public void Write(string stringValueTobeWritten)
        {
            if (!IsClosed)
            {
                using (System.IO.TextWriter textWriter = new System.IO.StreamWriter(_fullFilePath))
                {
                    textWriter.Write(BaseTransform(stringValueTobeWritten));
                    textWriter.Close();
                }
            }
        }

        public override string BaseTransform(string stringValueTobeWritten)
        {
            return (_next.Transform(stringValueTobeWritten));
        }

        public void Close()
        {
            _isClosed = true;
        }
    }
}
