using Azure;
using Social_Networking.Data;
using Social_Networking.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking.Model
{
    internal class FollowUsers
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public string SenderUSername { get; set; }
        public string RecieverString { get; set; }
        public static List<FollowUsers> Sugestions = new List<FollowUsers>() { };
        public static List<FollowUsers> FriendsFriend = new List<FollowUsers>() { };


        public void Follow()
        {
            try
            {
                while (true)
                {

                    using (var dbContext = new UserDbContext())
                    {
                        FollowUsers.Sugestions.Clear();
                        var currentUser = Posts.Users_List.FirstOrDefault();

                        Console.Clear();
                        Console.WriteLine();

                        Console.WriteLine("Friend Suggestions!");
                        var findSugestions = dbContext.Friends.Where(f => f.UserId2 == currentUser.ID || f.UserId1 == currentUser.ID).ToList();
                        foreach (var f in findSugestions)
                        {
                            if (f.UserId1 == currentUser.ID && f.UserId2 != currentUser.ID)
                            {
                                AddSuggestion(f.UserId2, f.UserName2);
                            }
                            else if (f.UserId2 == currentUser.ID && f.UserId1 != currentUser.ID)
                            {
                                AddSuggestion(f.UserId1, f.UserName1);
                            }
                        }

                        foreach (var fa in FollowUsers.Sugestions)
                        {
                            var friendsOfFriend = dbContext.Friends
                                .Where(f => f.UserId2 == fa.SenderID || f.UserId1 == fa.SenderID)
                                .Select(f => f.UserId1 == fa.SenderID ? f.UserId2 : f.UserId1)
                                .ToList();
                            friendsOfFriend = friendsOfFriend.Except(findSugestions.Select(f => f.UserId1).Union(findSugestions.Select(f => f.UserId2)).Append(currentUser.ID)).ToList();

                            foreach (var friendId in friendsOfFriend)
                            {
                                var friend = dbContext.Users.FirstOrDefault(u => u.ID == friendId);
                                if (friend != null)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGreen;             
                                    Console.Write($"{friend.UserName} ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.Write(" Followed By ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine($"{fa.SenderUSername}");
                                    Console.ResetColor();
                                    Console.WriteLine();
                                }
                            }
                        }

                        void AddSuggestion(int senderId, string senderUsername)
                        {
                            if (!FollowUsers.Sugestions.Any(s => s.SenderID == senderId))
                            {
                                FollowUsers.Sugestions.Add(new FollowUsers
                                {
                                    SenderID = senderId,
                                    SenderUSername = senderUsername
                                });
                            }
                        }


                        Console.WriteLine();
                        Console.Write("Enter Username: ");
                        string userNameToFollow = Console.ReadLine();

                        var userToFollow = dbContext.Users.FirstOrDefault(u => u.UserName == userNameToFollow);

                        if (userToFollow == null)
                        {
                            Console.WriteLine("User With Given Username does not exist!.");
                            break;
                        }

                        else
                        {
                            bool if_followexists = dbContext.Follows.Any(u => u.SenderID == currentUser.ID && u.RecieverID == userToFollow.ID);
                            bool if_Already_Freinds = dbContext.Friends.Any(u => u.UserId1 == currentUser.ID && u.UserId2 == userToFollow.ID || u.UserId1 == userToFollow.ID && u.UserId2 == currentUser.ID);
                            if (!if_followexists && !if_Already_Freinds )
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine($"You Followed {userNameToFollow}");
                                Console.ResetColor();
                                Console.WriteLine();

                                Console.WriteLine($"Now You can See {userNameToFollow}'s Posts and Comments!");
                                var follow = new FollowUsers()
                                {
                                    SenderID = currentUser.ID,
                                    SenderUSername = currentUser.UserName,
                                    RecieverID = userToFollow.ID,
                                    RecieverString = userToFollow.UserName,
                                };
                                dbContext.Follows.Add(follow);
                                dbContext.SaveChanges();
                                break;
                            }
                            else
                            {
                                if (if_Already_Freinds)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine($"You Are Already Friends With {userToFollow.UserName}!");
                                    Console.ResetColor();
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    Console.WriteLine($"You Already Follow {userToFollow.UserName}");
                                    Console.ResetColor();
                                    break;
                                }
                             
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
        public void Follow_Requests()
        {
            try
            {
                var currentUser = Posts.Users_List.FirstOrDefault();
                using (var dbcontext = new UserDbContext())
                {
                    var requests = dbcontext.Follows
                        .Where(u => u.RecieverID == currentUser.ID)
                        .ToList();

                    foreach (var userReq in requests)
                    {
                        Console.WriteLine($"ID {userReq.ID}. {userReq.SenderUSername}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Enter ID To respond!");
                    int respondVariable = Convert.ToInt32(Console.ReadLine());

                    bool respond = dbcontext.Follows
                        .Any(u => u.ID == respondVariable && u.RecieverID == currentUser.ID && u.RecieverString == currentUser.UserName);

                    if (respond)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("1. Accept");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("2. Reject");
                        Console.ResetColor();
                        int acceptOrReject = Convert.ToInt32(Console.ReadLine());

                        switch (acceptOrReject)
                        {
                            case 1:
                                Accept(respondVariable, currentUser.ID, currentUser.UserName);
                                break;
                            case 2:
                                Reject(respondVariable);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ID does not match the friend request user!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Accept(int requestId, int currentUserId, string currentUserName)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var befriended = dbContext.Follows
                        .Where(u => u.ID == requestId && u.RecieverID == currentUserId && u.RecieverString == currentUserName)
                        .ToList();

                    if (befriended.Any())
                    {
                        foreach (var item in befriended)
                        {
                            string newFriendName = item.SenderUSername;
                            int newFriendID = item.SenderID;

                            Console.WriteLine($"You and {newFriendName} are now friends!");
                            dbContext.Follows.Remove(item);

                            var friend = new Friends()
                            {
                                UserName1 = currentUserName,
                                UserName2 = newFriendName,
                                UserId1 = currentUserId,
                                UserId2 = newFriendID,
                            };

                            dbContext.Friends.Add(friend);
                        }

                        dbContext.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("No pending friend requests with the specified ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Reject(int requestId)
        {
            try
            {
                var currentUser = Posts.Users_List.FirstOrDefault();
                using (var dbContext = new UserDbContext())
                {
                    var befriended = dbContext.Follows
                        .Where(u => u.ID == requestId && u.RecieverID == currentUser.ID && u.RecieverString == currentUser.UserName)
                        .ToList();
                    foreach (var item in befriended)
                    {
                        dbContext.Follows .Remove(item);
                    }
                    dbContext.SaveChanges();
                    Console.WriteLine($"Friend request with ID {requestId} rejected.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
