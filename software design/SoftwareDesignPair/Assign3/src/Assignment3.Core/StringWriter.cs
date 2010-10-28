using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class StringWriter : ChainOperator
    {
        StringBuilder stringBuilder;
        bool _isClosed = false;

        public StringWriter()
            : base()
        {
            stringBuilder = new StringBuilder(String.Empty);
        }

        public StringWriter(IOperation next)
            : base(next)
        {
            stringBuilder = new StringBuilder(String.Empty);
        }

        public string Contents
        {
            get
            {
                return stringBuilder.ToString();
            }
        }

        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }
        }

        public void Write(string stringValueToBeWritten)
        {
            if (!IsClosed)
            {
                stringBuilder.Append(BaseTransform(stringValueToBeWritten));
            }
        }

        public override string BaseTransform(string myString)
        {
            return (_next.Transform(myString));
        }

        public void Close()
        {
            _isClosed = true;
        }
    }
}
