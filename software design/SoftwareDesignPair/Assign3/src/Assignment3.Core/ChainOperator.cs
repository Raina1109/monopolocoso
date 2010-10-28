using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment3.Core
{
    public abstract class ChainOperator : IOperation
    {
        protected IOperation _next;

        public abstract String BaseTransform(String myString);

        public ChainOperator(IOperation next)
        {
            _next = next;
        }

        public ChainOperator()
        {
            _next = new ConcreteOperator();
        }

        public String Transform(String myString)
        {
            return BaseTransform(_next.Transform(myString));
        }
    }
}
