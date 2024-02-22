using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class PostLikesCount
    {
        public bool Like { get; set; }
        public int ID { get; set; }
        public string UserName { get; set; }
        public int userID { get; set; }
        public int postID { get; set; }
    }
}
