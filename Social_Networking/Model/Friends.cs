using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class Friends
    {
        public int ID { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
        public List<FollowUsers> Users_Friends { get; set; }
    }
}
