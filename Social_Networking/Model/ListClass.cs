using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class ListClass
    {
        public string FriendName { get; set; }
        public int FriendID { get; set; }
        public int FriendMessageCount { get; set; }
        public static List<ListClass> Friends = new List<ListClass>() { };
        public static List<ListClass> MessageCount = new List<ListClass>() { };

    }
}
