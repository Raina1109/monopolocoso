using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Src
{
    public class LetterCartException : ApplicationException
    {
        public LetterCartException() : base() {}
        public LetterCartException(string s) : base(s) {}
        public LetterCartException(string s, Exception ex) : base(s, ex) { }
    }
}
