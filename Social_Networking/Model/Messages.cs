using Microsoft.EntityFrameworkCore;
using Social_Networking.Data;
using Social_Networking.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class Messages
    {
        public int ID {  get; set; }
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public string Message { get; set; }
        public string SenderUsername { get; set; }
        public string RecieverUserName { get; set; }
        public bool Seen { get; set; }
        public List<FollowUsers> Message_List { get; set; }



        public void Chat_Display()
        {
            try
            {
                Colecting_data();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("  My Friends!");
                Console.ResetColor();
                Console.WriteLine();
                using (var dbContext = new UserDbContext())
                {
                    foreach (var item in ListClass.MessageCount)
                    {
                        var friend = dbContext.Users.FirstOrDefault(u => u.ID == item.FriendID);
                        if (friend != null)
                        {
                            var Notification = item.FriendMessageCount.ToString();
                            var empty = "";
                            var onlineColor = friend.isonline == true ? ConsoleColor.Green : ConsoleColor.Red;
                            var IF_Notification_exists = item.FriendMessageCount > 0 ? item.FriendMessageCount.ToString() : Notification = "";
                            var countColor33 = item.FriendMessageCount > 0 ? empty = "+" :  empty = "";

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"  {item.FriendName}");
                            Console.ResetColor();
                            Console.ForegroundColor = onlineColor;
                            Console.Write("°");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"   {empty}{Notification}");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"           ID {item.FriendID}  ");
                            Console.ResetColor();
                            Console.WriteLine("___________________________________");
                            Console.WriteLine();
                        }
                    }
                }

                Send_MEssages();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void Colecting_data()
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var MeUser = Posts.Users_List.FirstOrDefault(); 
                    var Friends = dbContext.Friends.Where(u=> u.UserId1 == MeUser.ID || u.UserId2 == MeUser.ID);
                    ListClass.Friends.Clear();
                    ListClass.MessageCount.Clear();
                    foreach (var F in Friends)
                    {
                        if(F.UserId1 == MeUser.ID)
                        {
                            ListClass fc = new ListClass();
                            fc.FriendName = F.UserName2;
                            fc.FriendID = F.UserId2;
                            ListClass.Friends.Add(fc);
                        }
                        else if(F.UserId2 == MeUser.ID)
                        {
                            ListClass fc = new ListClass();
                            fc.FriendName = F.UserName1;
                            fc.FriendID = F.UserId1;
                            ListClass.Friends.Add(fc);
                        }
                    }
                    foreach (var item in ListClass.Friends)
                    {
                        var friendId = item.FriendID;
                        var messages = dbContext.Messages.Where(u => (u.SenderID == friendId && u.RecieverID == MeUser.ID));
                        var messageCount = messages.Count(u => u.Seen == false);
                        ListClass l = new ListClass();
                        l.FriendName = item.FriendName;
                        l.FriendMessageCount = messageCount;
                        l.FriendID = item.FriendID;
                        ListClass.MessageCount.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Send_MEssages ()
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    bool Conversation = false;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("  Enter User ID To Start Chatting!");
                    int FriendID = Convert.ToInt32(Console.ReadLine());
                    var currentUser = Posts.Users_List.FirstOrDefault();
                    bool If_IS_Friend = dbContext.Friends.Any(u => u.UserId1 == FriendID && u.UserId2 == currentUser.ID || u.UserId2 == FriendID && u.UserId1 == currentUser.ID);
                    var friendMessages = dbContext.Messages.Where(u => (u.SenderID == currentUser.ID && u.RecieverID == FriendID) || (u.SenderID == FriendID && u.RecieverID == currentUser.ID));
                    if (!friendMessages.Any() && If_IS_Friend)
                    {
                        Console.WriteLine();
                        Console.WriteLine("       Start Chatting");
                        Console.WriteLine();
                    }
                    else
                    {
                        if (If_IS_Friend)
                        {
                            Console.Clear();
                            var Message_Count_Clear = dbContext.Messages.Where(u => u.SenderID == FriendID && u.RecieverID == currentUser.ID);

                            foreach (var item in Message_Count_Clear)
                            {
                                item.Seen = true;
                            }
                            dbContext.SaveChanges();
                            Console.WriteLine();
                            Console.WriteLine($"Chat");
                            Console.WriteLine();
                            foreach (var item in friendMessages)
                            {
                                if (item.SenderID == FriendID && item.RecieverID == currentUser.ID)
                                {
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine($"- {item.Message}");
                                }
                                else if (item.SenderID == currentUser.ID && item.RecieverID == FriendID)
                                {
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                                    Console.WriteLine($"          - {item.Message}");
                                }
                            }
                            Conversation = true;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Id Doesnot Match Any of your Friends!");
                            Console.WriteLine();
                        }

                    }
                    if (Conversation)
                    {
                        bool Till_exit = true;
                        while (Till_exit)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine();
                            Console.Write("          - ");
                            string typeMessgae = Console.ReadLine();
                            Console.ResetColor();
                            var FriendsONot = dbContext.Users.SingleOrDefault(u => u.ID == FriendID);
                            if (typeMessgae == "")
                            {
                                Till_exit = false;
                                Console.Clear();
                            }
                            else
                            {
                                var message = new Messages()
                                {
                                    SenderID = currentUser.ID,
                                    RecieverID = FriendID,
                                    SenderUsername = currentUser.UserName,
                                    RecieverUserName = FriendsONot.UserName,
                                    Message = typeMessgae,
                                    Seen = false,
                                };
                                dbContext.Messages.Add(message);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }           
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
