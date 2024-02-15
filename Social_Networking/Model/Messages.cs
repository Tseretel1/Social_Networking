﻿using Microsoft.EntityFrameworkCore;
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
                            if(userName.UserName1!= currentUser.UserName)
                            {
                                userNotme = userName.UserName1;
                                userid = userName.UserId1;
                            }
                            else if(userName.UserName2 != currentUser.UserName)
                            {
                                userNotme = userName.UserName2;
                                userid = userName.UserId2;
                            }
                            Console.WriteLine();
                            Console.WriteLine($"ID {userid} {userNotme}");
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
                            if (item.SenderID == FriendID && item.ReceiverID == currentUser.ID)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"-{item.Message}");
                            }
                            else if (item.SenderID == currentUser.ID && item.ReceiverID == FriendID)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"            -{item.Message}");
                            }
                        }
                    }




                    bool FriendsOrNOT = dbContext.Friends.Any(u => u.UserId1 == FriendID && u.UserId2 == currentUser.ID || u.UserId2 == FriendID && u.UserId1 == currentUser.ID);
                    string typeMessage = Console.ReadLine();
                    if (!string.IsNullOrEmpty(typeMessage) && typeMessage.Trim() != "" && FriendsOrNOT)
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
                        };

                        dbContext.Messages.Add(newMessage);
                        dbContext.SaveChanges();
                    }
                    else if(!FriendsOrNOT)
                    {
                        Console.WriteLine("ID is incorrect or You are not Friends With User with This ID!");
                    }
                    else
                    {
                        Console.WriteLine("You can't send an empty message.");
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