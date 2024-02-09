using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Exceptions
{
    internal class Exceptions : Exception
    {
        public class WrongInputException : ApplicationException
        {
            public WrongInputException() : base("Wrong Input Please Try Again")
            {

            }
        }
    }
}
