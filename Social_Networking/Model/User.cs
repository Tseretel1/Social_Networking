using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Social_Networking.Data;
using Social_Networking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Networking
{
    internal class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastaname { get; set; }
        public int Age { get; set; }
        public int ID { get; set; }
        public bool isonline { get; set; }
        public string Content { get; set; }
        public virtual List<Messages> messages { get; set; }
         public void Registration()
        {
            try
            {
                bool RegistrationBool = false;
                while (!RegistrationBool)
                {
                    Console.Clear();
                    Console.WriteLine("Registration");
                    Console.WriteLine();
                    Console.Write("Enter username: ");
                    string UserName = Console.ReadLine();

                    using (var dbContext = new UserDbContext())
                    {
                        bool isUsernameExists = dbContext.Users.Any(u => u.UserName == UserName);

                        if (isUsernameExists)
                        {
                            Console.WriteLine("Username already exists. Please try again with a different username.");
                        }
                        else
                        {
                            Console.WriteLine("Enter Your Password");
                            string Password = Console.ReadLine();

                            Console.WriteLine("Enter Your Email");
                            string Email = Console.ReadLine();

                            Console.WriteLine("Enter Your Name");
                            string Name = Console.ReadLine();

                            Console.WriteLine("Enter Your Lastname");
                            string LastName = Console.ReadLine();

                            Console.WriteLine("Enter Your Age");
                            int Age = Convert.ToInt32(Console.ReadLine());

                            UserDbContext context = new UserDbContext();

                            var user = new User()
                            {
                                Name = Name,
                                Lastaname = LastName,
                                Age = 20,
                                Email = Email,
                                UserName = UserName,
                                Password = Password,
                                Content = "",
                            };
                            context.Users.Add(user);
                            context.SaveChanges();
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
        public void Login()
        {
            try
            {
                bool loginSuccessful = false;
                Console.Clear();
                while (!loginSuccessful)
                {
                    Console.WriteLine("Login");
                    Console.WriteLine();
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    using (var dbContext = new UserDbContext())
                    {
                        bool isCredentialsValid = dbContext.Users.Any(u => u.UserName == username && u.Password == password);

                        if (isCredentialsValid)
                        {
                            loginSuccessful = true;
                            Console.WriteLine("Login successful!");
                            string Username = "";
                            string name = "";
                            string lastname = "";
                            string email = "";
                            int age = 0;
                            int user_id = 0;
                            var userToUpdate = dbContext.Users.FirstOrDefault(u => u.UserName == username);
                            userToUpdate.isonline = true;
                            dbContext.SaveChanges();
                            if (userToUpdate != null)
                            {
                                username = userToUpdate.UserName;
                                name = userToUpdate.Name;
                                lastname = userToUpdate.Lastaname;
                                email = userToUpdate.Email;
                                age = userToUpdate.Age;
                                user_id = userToUpdate.ID;
                                userToUpdate.isonline = true;
                                Posts.Users_List.Add(userToUpdate);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Username or password. Please try again.");
                        }
                    }
                }
                Console.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void ContentPreference()
        {
            Console.Clear();
            bool TillExit = true;
            while (TillExit)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Choose Preferred Content For Your Page!");
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
                            Console.WriteLine($"Content Preference set to {content}");
                            TillExit = false;
                            break;
                        case 2:
                            content = "Films";
                            Console.WriteLine($"Content Preference set to {content}");
                            TillExit = false;
                            break;
                        case 3:
                            content = "Games";
                            Console.WriteLine($"Content Preference set to {content}");
                            TillExit = false;
                            break;
                        case 4:
                            content = "";
                            Console.WriteLine($"Content Preference set to None");
                            TillExit = false;
                            break;
                        case 5:
                            TillExit = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose a number between 1 and 4.");
                            break;
                    }

                    if (chosenContent >= 1 && chosenContent <= 4)
                    {
                        var currentUser = Posts.Users_List.FirstOrDefault();
                        if (currentUser != null)
                        {
                            using (var dbContext = new UserDbContext())
                            {
                                var existingUser = dbContext.Users.SingleOrDefault(u => u.UserName == currentUser.UserName);

                                if (existingUser != null)
                                {
                                    existingUser.Content = content;
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    Console.WriteLine($"User with username {currentUser.UserName} not found in the database.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Current user not found.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void Setings()
        {
            Console.Clear();
            bool whilee = true; 
            while (whilee)
            {
                try 
                {
                    Console.WriteLine("Settings");
                    Console.WriteLine();
                    Console.WriteLine("1. Content Preference");
                    Console.WriteLine("2. Exit!");
                    int Cce = Convert.ToInt32( Console.ReadLine());

                    switch (Cce) 
                    {
                        case 1:
                            ContentPreference();
                            Console.Clear();
                            break;                     
                        case 2:
                            whilee = false;
                            Console.Clear();
                            break;
                        default: whilee = false;
                            break;
                    }
                }


                catch (Exception ex) 
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void ExitFromAccount()
        {
            Console.Clear();
            string Usenrame = "";
            foreach(var user in Posts.Users_List)
            {
                Usenrame = user.UserName;
            }
            using (var dbContext = new UserDbContext())
            {
                var userToUpdate = dbContext.Users.FirstOrDefault(u => u.UserName == Usenrame);
                userToUpdate.isonline = false;
                dbContext.SaveChanges();
            }
            Posts.Users_List.Clear();
        }
    }
 }
