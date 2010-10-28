using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Example3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<HomePhone> homePhoneList = new List<HomePhone>();

            homePhoneList.Add(new HomePhone("AT&T"));

            BreakIt(homePhoneList);
        }

        private static void BreakIt(IList homePhoneList)
        {
 	        homePhoneList.Add(new CellPhone("T-Mobile"));
        }
    }

    class HomePhone
    {
        string phoneName;

        public HomePhone(string name)
        {
            phoneName = name;
        }

        public void Dial()
        {
            Console.WriteLine("Dialing Home Phone");
        }
    }

    class CellPhone
    {
        string phoneName;

        public CellPhone(string name)
        {
            phoneName = name;
        }

        public void Dial()
        {
            Console.WriteLine("Dialing Cell Phone");
        }
    }

}
