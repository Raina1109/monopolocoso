using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class ConcreteOperator : IOperation
    {
        public String Transform(String myString)
        {
            return myString;
        }
    }
}
