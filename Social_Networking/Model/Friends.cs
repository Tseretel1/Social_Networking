using Social_Networking.Data;
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
        public virtual List<FollowUsers> Users_Friends { get; set; }
        public void Friends_Adjustment()
        {
            try
            {

                bool Till_Exit = true;
                while (Till_Exit)
                {
                    Friends f = new Friends();
                    FollowUsers FS = new FollowUsers();
                    using (var dbContext = new UserDbContext())
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();
                        var userFriendCount = dbContext.Friends
                            .Where(u => u.UserId1 == currentUser.ID || u.UserId2 == currentUser.ID)
                            .Count();
                        var UserFollowCount = dbContext.Follows
                          .Where(u => u.RecieverID == currentUser.ID)
                          .Count();

                        Console.Write($"1. Friends:");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($" {userFriendCount}");
                        Console.ResetColor();
                        Console.WriteLine("2. Follow People");
                        Console.Write($"3. Friend Requests:");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($" +{UserFollowCount}");
                        Console.ResetColor();
                        Console.WriteLine("4.Exit!");
                        int Choice = Convert.ToInt32(Console.ReadLine());

                        switch (Choice)
                        {
                            case 1:
                                f.MyFriends(); ;
                                break;
                            case 2:
                                FS.Follow();
                                break;
                            case 3:
                                FS.Follow_Requests();
                                break;
                            case 4:
                                Till_Exit = false;
                                break;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void MyFriends()
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
                            Console.WriteLine();
                            Console.WriteLine(userName);
                        }
                        string userInput = Console.ReadLine();

                        if (userInput=="")
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
