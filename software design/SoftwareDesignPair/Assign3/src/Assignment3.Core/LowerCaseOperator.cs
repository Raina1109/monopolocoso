using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class LowerCaseOperator : ChainOperator
    {
        public LowerCaseOperator()
            : base()
        {
        }

        public LowerCaseOperator(IOperation next) : base(next)
        {
        }

        public override String BaseTransform(String myString)
        {
            try
            {
                return myString.ToLower();
            }
            catch (NullReferenceException)
            {
                throw new OperationException();
            }
        }
    }
}
