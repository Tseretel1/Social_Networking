using Microsoft.EntityFrameworkCore;
using Social_Networking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class Messages
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public string SenderUserName { get; set; }  
        public string Message { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverUserName { get; set; }
        public string Seen { get; set; }
        public virtual List<User> Users { get; set; }



        public void Chats_Friends()
        {
            try
            {
                bool tillExit = true;
                while (tillExit)
                {
                    using (var dbContext = new UserDbContext())
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();
                        var friendNames = dbContext.Friends.Where(u => u.UserId1 == currentUser.ID || u.UserId2 == currentUser.ID);

                        foreach (var userName in friendNames)
                        {
                            string userNotme = "";
                            int userid = 0;
                            int MessageCount = 0;
                            if (userName.UserName1 != currentUser.UserName)
                            {
                                userNotme = userName.UserName1;
                                userid = userName.UserId1;

                            }
                            else if (userName.UserName2 != currentUser.UserName)
                            {
                                userNotme = userName.UserName2;
                                userid = userName.UserId2;

                            }
                            Console.WriteLine();
                            Console.Write($"{userid}. {userNotme}");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"  +{MessageCount}");
                            Console.ResetColor();

                        }
                        tillExit = false;
                        Message_Friend();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void Message_Friend()
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    Console.WriteLine("Enter User ID To Start Chatting!");
                    int FriendID = Convert.ToInt32(Console.ReadLine());


                    var currentUser = Posts.Users_List.FirstOrDefault();
                    var friendMessages = dbContext.Messages
                        .Where(u => (u.SenderID == currentUser.ID && u.ReceiverID == FriendID) || (u.SenderID == FriendID && u.ReceiverID == currentUser.ID));

                    if (!friendMessages.Any())
                    {
                        Console.WriteLine();
                        Console.WriteLine("       Start Chatting");
                        Console.WriteLine();
                    }
                    else
                    {
                        foreach (var item in friendMessages)
                        {
                            Console.WriteLine();
                            if (item.SenderID == FriendID && item.ReceiverID == currentUser.ID)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine($"-{item.Message}");
                                Console.ResetColor();
                            }
                            else if (item.SenderID == currentUser.ID && item.ReceiverID == FriendID)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine($"            -{item.Message}");
                                Console.ResetColor();
                            }
                        }

                    }




                    bool FriendsOrNOT = dbContext.Friends.Any(u => u.UserId1 == FriendID && u.UserId2 == currentUser.ID || u.UserId2 == FriendID && u.UserId1 == currentUser.ID);
                    if(!FriendsOrNOT)
                    {
                        Console.WriteLine("ID is incorrect or You are not Friends With User with This ID!");
                    }
                    else
                    {
                        bool till_exit = true;
                        while (till_exit)
                        {
                            int middleX = Console.WindowWidth / 2 - 47;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.SetCursorPosition(middleX, Console.CursorTop);
                            string typeMessage = Console.ReadLine();
                            Console.ResetColor();
                            if(typeMessage == "")
                            {
                                till_exit = false;
                            }
                            else
                            {
                                var friendUsernameFind = dbContext.Users.FirstOrDefault(u => u.ID == FriendID);
                                string friendsName = "";
                                if (friendUsernameFind != null)
                                {
                                    friendsName = friendUsernameFind.UserName;
                                }

                                Messages newMessage = new Messages()
                                {
                                    SenderID = currentUser.ID,
                                    ReceiverID = FriendID,
                                    SenderUserName = currentUser.UserName,
                                    ReceiverUserName = friendsName,
                                    Message = typeMessage,
                                    Seen = "Not seen",
                                };

                                dbContext.Messages.Add(newMessage);
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
