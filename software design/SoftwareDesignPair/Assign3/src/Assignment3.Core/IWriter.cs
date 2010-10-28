using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Assignment3.Core
{
    public interface IWriter
    {
         bool IsClosed { get;}
         void Write(string stringValueTobeWritten);
    }
}
