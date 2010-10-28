using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public class UpperCaseOperator : ChainOperator
    {
        public UpperCaseOperator()
            : base()
        {
        }

        public UpperCaseOperator(IOperation next)
            : base(next)
        {
        }

        public override String BaseTransform(String myString)
        {
            try
            {
                return myString.ToUpper();
            }
            catch(NullReferenceException)
            {
                throw new OperationException();
            }
        }
    }
}
