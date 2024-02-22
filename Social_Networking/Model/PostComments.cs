using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class PostComments
    {
        public int ID { get; set; }
        public int PostId { get; set; }
        public int Like { get; set; }
        public string Comment { get; set; }
        public int UserID { get; set; }
        public string Username { get; set; }
        public static List<PostComments> Posts_List = new List<PostComments>() { };
    }
}
