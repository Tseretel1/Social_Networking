using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Social_Networking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking
{
    internal class Posts
    {
        public string Post { get; set; }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int UserId  {get; set;}
        public DateTime DateTime { get; set; }
        public string Content { get; set; }

        public static List<User> Users_List = new List<User>(){};


        public void See_My_Posts()
        {
            try {
                int userId = 0;
                foreach (var user in Users_List)
                {
                    userId = user.ID;
                    using (var dbContext = new UserDbContext())
                    {
                        var Get_My_Posts = dbContext.Post.Where(u => u.UserId == userId).ToList();
                        foreach (var post in Get_My_Posts)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"User:{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine();
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine("______________");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void For_You_Page()
        {
            try
            {

                using (var dbContext = new UserDbContext())
                {
                    string contentPreference = "";
                    int CurrentUSerID = 0;
                    foreach(var i in Users_List)
                    {
                       CurrentUSerID = i.ID;
                    }
                    var UserContent = dbContext.Users.FirstOrDefault(u => u.ID == CurrentUSerID);
                    contentPreference = UserContent.Content;

                    if (contentPreference != "")
                    {
                        var query = dbContext.Post.Where(u => u.Content == contentPreference);
                        foreach (var post in query)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write($"{post.Username} :");
                            Console.ResetColor();
                            Console.Write($" Time {post.DateTime.TimeOfDay} Content");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($" {post.Content}");
                            Console.ResetColor();
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"-{post.Post}");
                            Console.ResetColor();
                            Console.WriteLine("");
                        }
                    }
                    else if(contentPreference == "")
                    {
                        var GetEveryPost = dbContext.Post.Where(u=> u.Content == u.Content);
                        string PostContent = "";
                        foreach (var post in GetEveryPost)
                        {
                            if(post.Content == "")
                            {
                                PostContent = "None";
                            }
                            else
                            {
                                PostContent = post.Content;
                            }
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{post.Username} :");
                            Console.ResetColor();
                            Console.Write($" Time {post.DateTime.TimeOfDay} Content");
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine($" {PostContent}");
                            Console.ResetColor();
                            Console.WriteLine("");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"-{post.Post}");
                            Console.ResetColor();
                            Console.WriteLine("");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Delete_My_Post()
        {
            try
            {
                int userId = 0;
                foreach (var user in Users_List)
                {
                    userId = user.ID;
                    using (var dbContext = new UserDbContext())
                    {
                        var Get_My_Posts = dbContext.Post.Where(u => u.UserId == userId).ToList();
                        foreach (var post in Get_My_Posts)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{post.ID}).");
                            Console.ResetColor();
                            Console.WriteLine($"{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine($"");
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine($"_______________");
                        }
                    }
                }
                Console.WriteLine("Enter Post Number To Delete");
                int PostDelete = Convert.ToInt32(Console.ReadLine());
                var currentUser = Posts.Users_List.FirstOrDefault();
                using (var dbContext = new UserDbContext())
                {
                    bool IsMyPost = dbContext.Post.Any(u => u.ID == PostDelete && u.UserId == currentUser.ID);
                    if (IsMyPost)
                    {
                        var postToDelete = dbContext.Post.SingleOrDefault(u => u.ID == PostDelete);

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Are You Sure To delete This Post?");
                        Console.ResetColor();

                        var Get_My_Posts = dbContext.Post.Where(u => u.ID == PostDelete).ToList();
                        foreach (var post in Get_My_Posts)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{post.ID}).");
                            Console.ResetColor();
                            Console.WriteLine($"{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine($"");
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine($"_______________");
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("1.Yes Delete!");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("2.No Im Not Sure");
                        Console.ResetColor();
                        int Sure_Ornot = Convert.ToInt32(Console.ReadLine());
                        if (Sure_Ornot == 1)
                        {
                            if (postToDelete != null)
                            {
                                dbContext.Post.Remove(postToDelete);
                                dbContext.SaveChanges();
                                Console.WriteLine();
                                Console.ForegroundColor= ConsoleColor.Green;
                                Console.WriteLine("Post deleted successfully.");
                                Console.ResetColor();
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Post with given id does not exist!");
                                Console.ResetColor();
                                Console.WriteLine();
                            }
                        }
                        else if (Sure_Ornot == 2)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Declined!");
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Incorrect input Post Did not Deleted!");
                            Console.ResetColor();
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Post Id is Incorrect!");
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void Write_Post()
        {
            try
            {
                using (var context = new UserDbContext())
                {
                    Console.WriteLine("Post: ");
                    string Status = Console.ReadLine();
                        Console.WriteLine();
                        Console.WriteLine("What Content Is Your Post?");
                        Console.WriteLine();
                        Console.WriteLine("1. Music");
                        Console.WriteLine("2. Films");
                        Console.WriteLine("3. Games");
                        Console.WriteLine("4. None");
                        Console.WriteLine("5. Exit");

                        int chosenContent = Convert.ToInt32(Console.ReadLine());
                        string content = "";

                        switch (chosenContent)
                        {
                            case 1:
                                content = "Music";
                                break;
                            case 2:
                                content = "Films";
                                break;
                            case 3:
                                content = "Games";
                                break;
                            case 4:
                                content = "";
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please choose a number between 1 and 4.");
                                break;                               
                        }

                    string Username = "";
                    string name = "";
                    string lastname = "";
                    string email = "";
                    int age = 0;
                    int user_id = 0;
                    foreach (var item in Users_List)
                    {
                        Username = item.UserName;
                        name = item.Name;
                        lastname = item.Lastaname;
                        age = item.Age;
                        email = item.Email;
                        user_id = item.ID;
                    }
                    var post = new Posts()
                    {
                        Post = Status,
                        UserId = user_id,
                        Name = name,
                        LastName = lastname,                       
                        Username = Username,    
                        DateTime = DateTime.Now,
                        Content = content
                    };
                    context.Post.Add(post);
                    context.SaveChanges();
                    Console.WriteLine($"{Username} Content {content}");
                    Console.WriteLine();
                    Console.WriteLine($"   {Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
