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
                bool isContentPreferenceSet = false;
                string contentPreference = "";

                foreach (var user in Users_List)
                {
                    if (user.Content != null)
                    {
                        isContentPreferenceSet = true;
                        contentPreference = user.Content;
                        break;
                    }
                    else
                    {
                        isContentPreferenceSet = false;
                    }
                }

                using (var dbContext = new UserDbContext())
                {
                    var query = dbContext.Post.AsQueryable();
                    if (isContentPreferenceSet)
                    {
                        query = query.Where(u => u.Content == contentPreference);
                        foreach (var post in query)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{post.ID}).User:{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine("");
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine("_______________");
                        }
                    }
                    if(!isContentPreferenceSet)
                    {
                        var getMyPosts = query.ToList();
                        foreach (var post in getMyPosts)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{post.ID}).User:{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine("");
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine("_______________");
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
                            Console.WriteLine($"{post.ID}).User:{post.Username} Time {post.DateTime.TimeOfDay}");
                            Console.WriteLine($"");
                            Console.WriteLine($"{post.Post}");
                            Console.WriteLine($"_______________");
                        }
                    }
                }
                Console.WriteLine("Enter Post Number To Delete");
                int PostDelete = Convert.ToInt32(Console.ReadLine());

                using (var dbContext = new UserDbContext())
                {
                    var postToDelete = dbContext.Post.SingleOrDefault(u => u.ID == PostDelete);

                    if (postToDelete != null)
                    {
                        dbContext.Post.Remove(postToDelete);
                        dbContext.SaveChanges();
                        Console.WriteLine("Post deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Post with given id does not exist!");
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
                        Console.WriteLine("4. All Together");
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
                                content = "All Together";
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
