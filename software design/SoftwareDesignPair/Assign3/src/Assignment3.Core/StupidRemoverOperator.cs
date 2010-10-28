using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class StupidRemoverOperator : ChainOperator
    {
        public StupidRemoverOperator()
            : base()
        {
        }

        public StupidRemoverOperator(IOperation next)
            : base(next)
        {
        }

        public override string BaseTransform(string myString)
        {
            try
            {
                return myString.Replace("stupid", "s*****");
            }
            catch (NullReferenceException)
            {
                throw new OperationException();
            }
        }
    }
}
