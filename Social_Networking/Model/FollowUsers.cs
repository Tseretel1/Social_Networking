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




        public void Friends_Adjustment()
        {
            try 
            {

                bool Till_Exit = true;
                while (Till_Exit)
                {
                    using (var dbContext = new UserDbContext())
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();
                        var userFriendCount = dbContext.Friends
                            .Where(u => u.UserId1 == currentUser.ID || u.UserId2 == currentUser.ID)
                            .Count();

                        Console.WriteLine($"1. Friends: {userFriendCount}");

                        Console.WriteLine("2. Follow");
                        Console.WriteLine("3. Friend Requests");
                        Console.WriteLine("4.Exit!");
                        int Choice = Convert.ToInt32(Console.ReadLine());

                        switch( Choice )
                        {
                            case 1: Friends();
                                break;
                            case 2:
                                Follow();
                                break;
                            case 3:
                                FriendRequests();
                                break;
                            case 4:
                                Till_Exit = false;
                                break;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Follow()
        {
            try
            {
                while (true)
                {
                    Console.Write("Enter Username: ");
                    string userNameToFollow = Console.ReadLine();

                    using (var dbContext = new UserDbContext())
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();
                        var userToFollow = dbContext.Users.FirstOrDefault(u => u.UserName == userNameToFollow);

                        if (userToFollow == null)
                        {
                            Console.WriteLine("User With Given Username does not exist!.");
                            break;
                        }

                        else
                        {
                            bool if_followexists = dbContext.Follows.Any(u => u.SenderID == currentUser.ID && u.RecieverID == userToFollow.ID);
                            if (!if_followexists)
                            {
                                Console.WriteLine($"You Followed {userNameToFollow}");
                                Console.WriteLine($"Now You can Chat With {userNameToFollow}. See Their Posts and Comments!");

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
                                Console.WriteLine($"You Already Follow {userToFollow.UserName}");
                                break;
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
        public void FriendRequests()
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
                        Console.WriteLine("1. Accept");
                        Console.WriteLine("2. Reject");
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
                        Console.WriteLine("User With Given id Does not exist or the friend request does not match the current user!");
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
            Console.WriteLine($"Friend request with ID {requestId} rejected.");
        }

        public void Friends()
        {
            try
            {
                bool tillExit = false;
                while (!tillExit)
                {
                    using (var dbContext = new UserDbContext())
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();

                        var friendNames = dbContext.Friends
                            .Where(u => u.UserId1 == currentUser.ID || u.UserId2 == currentUser.ID)
                            .Select(u => u.UserId1 == currentUser.ID ? u.UserName2 : u.UserName1)
                            .ToList();

                        foreach (var userName in friendNames)
                        {
                            Console.WriteLine(userName);
                        }

                        Console.WriteLine("Press 'E' to exit or any other key to continue displaying friends.");
                        string userInput = Console.ReadLine();

                        if (userInput.ToUpper() == "E")
                        {
                            tillExit = true;
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
