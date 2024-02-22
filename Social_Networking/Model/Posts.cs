using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Social_Networking.Data;
using Social_Networking.Model;
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
                        string content = "";
                        foreach (var post in Get_My_Posts)
                        {
                           if(post.Content== "")
                            {
                                content = "None";
                            }
                            else
                            {
                                content = post.Content;
                            }
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{post.Username}");
                            Console.ResetColor();
                            Console.Write($"Time {post.DateTime.TimeOfDay} content");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($" {content}");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"{post.Post}");
                            Console.ResetColor();
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
                    var currentUser = Posts.Users_List.FirstOrDefault();

                    string contentPreference = "";
                    int CurrentUSerID = 0;
                    foreach (var i in Users_List)
                    {
                        CurrentUSerID = i.ID;
                    }
                    var UserContent = dbContext.Users.FirstOrDefault(u => u.ID == CurrentUSerID);
                    contentPreference = UserContent.Content;
                    bool if_no_content_Preffered = dbContext.Users.Any(u => u.ID == CurrentUSerID && u.Content == "");

                    var ForYou = (IEnumerable<Posts>)null;
                    var Prefered_Content = dbContext.Post.Where(u => u.Content == contentPreference);
                    var All_Content = dbContext.Post.ToList();

                    var content = if_no_content_Preffered ? ForYou = All_Content : ForYou = Prefered_Content;

                    int currentIndex = 0;
                    int pageSize = 1;

                    if (ForYou != null && ForYou.Any())
                    {
                        bool exit = false;
                        while (!exit)
                        {
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("    Press Enter to scroll!");
                            Console.WriteLine("    1.like 2.comment 3.exit");
                            Console.ResetColor();
                            int Postid = 0;
                            var currentPosts = ForYou.Skip(currentIndex).Take(pageSize);
                            foreach (var post in currentPosts)
                            {;
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("    Post:");
                                Console.ResetColor();
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.Write($"  {post.Username} :");
                                Console.ResetColor();
                                Console.Write($" Time {post.DateTime.TimeOfDay}");
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine($" {post.Content}");
                                Console.ResetColor();
                                Console.WriteLine("");
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine($"    - {post.Post}");
                                Console.ResetColor();
                                Console.WriteLine("    _______________________________");
                                Postid = post.ID;

                                var like = "";
                                var r = Console.ForegroundColor = ConsoleColor.Cyan;
                                bool Likedornot = dbContext.PostLikes.Any(u => u.postID == Postid && u.Like == true && u.userID == currentUser.ID);
                                var Color = Likedornot ? r = ConsoleColor.Cyan : r = ConsoleColor.White;
                                var LikeedText  = Likedornot ? like = "Liked" : like = "like";
                                var likeCount = dbContext.PostLikes.Count(u => u.postID == Postid && u.Like == true);
                                var comments = dbContext.PostComments.Where(u => u.PostId == Postid).ToList();

                                Console.ForegroundColor = r;
                                Console.WriteLine($"    {like}: {likeCount}");
                                Console.ResetColor();
                                Console.WriteLine();

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine("    Comments");
                                Console.ResetColor();
                                Console.WriteLine("");

                                foreach (var comment in comments)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.Write($"        {comment.Username}: ");
                                    Console.ResetColor();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine($"{comment.Comment}");
                                    Console.ResetColor();
                                }

                            }
                            string input = Console.ReadLine();

                            if (input.ToLower() == "1")
                            {
                                Console.Clear();
                                currentIndex = currentIndex;
                                bool Likedornot = dbContext.PostLikes.Any(u => u.postID == Postid && u.Like == true && u.userID == currentUser.ID);

                                if (Likedornot)
                                {
                                    var existingLike = dbContext.PostLikes.FirstOrDefault(u => u.postID == Postid && u.Like == true && u.userID == currentUser.ID);
                                    existingLike.Like = false;
                                    dbContext.SaveChanges();
                                }

                                else
                                {
                                    PostLikesCount p = new PostLikesCount();
                                    p.Like = true;
                                    p.postID = Postid;
                                    p.userID = currentUser.ID;
                                    p.UserName = currentUser.UserName;
                                    dbContext.PostLikes.Add(p);
                                    dbContext.SaveChanges();
                                }
                            }

                            else if (input.ToLower() == "2")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write($"        {currentUser.UserName}:");
                                Console.ResetColor();
                                string comment = Console.ReadLine();
                                PostComments p = new PostComments();
                                p.Comment = comment;
                                p.PostId = Postid;
                                p.UserID = currentUser.ID;
                                p.Username = currentUser.UserName;
                                dbContext.PostComments.Add(p);
                                dbContext.SaveChanges();
                                Console.Clear();
                                currentIndex = currentIndex;
                            }
                            else if(input.ToLower() == "3")
                            {
                                exit = true;
                                Console.Clear();
                            }
                            else
                            {
                                currentIndex += pageSize;
                                if (currentIndex >= ForYou.Count())
                                {
                                    Console.WriteLine("You've reached the end of the posts.");
                                    Console.WriteLine();
                                    Console.WriteLine("Press Enter to exit.");
                                    Console.ReadLine();
                                    exit = true;
                                    Console.Clear();
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No posts to display.");
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
                    if (Status == "")
                    {
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("You cant Write empty Post");
                        Console.WriteLine();
                    }
                    else
                    {
                        bool ifexit = true;
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
                                Console.Clear();
                                break;
                            case 2:
                                content = "Films";
                                Console.Clear();
                                break;
                            case 3:
                                content = "Games";
                                Console.Clear();
                                break;
                            case 4:
                                content = "";
                                Console.Clear();

                                break;
                            case 5:
                                ifexit = false;
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please choose a number between 1 and 4.");
                                Console.Clear();
                                break;
                        }

                        if (ifexit) 
                        {
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
                        else if(!ifexit)
                        {
                            Console.Clear();
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
