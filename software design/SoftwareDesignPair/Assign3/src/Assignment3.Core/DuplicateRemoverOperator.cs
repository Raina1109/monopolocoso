using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment3.Core
{
    public class DuplicateRemoverOperator : ChainOperator
    {
        const string duplicatePattern = "\\b(\\w+) \\1\\b";

        public DuplicateRemoverOperator()
            : base()
        {
        }

         public DuplicateRemoverOperator(IOperation next)
            : base(next)
        {
        }

        public override String BaseTransform(String myString)
        {
            Regex regulareExpression = new Regex(duplicatePattern);

            try
            {
                foreach (Match duplicateMatch in regulareExpression.Matches(myString))
                {
                    string duplicateWord = duplicateMatch.ToString().Split(' ')[0];

                    myString = myString.Replace(duplicateMatch.Value, duplicateWord);
                }

                return myString;
            }
            catch (NullReferenceException)
            {
                throw new OperationException();
            }
        }
    }
}
