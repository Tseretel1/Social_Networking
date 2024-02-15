using Social_Networking;
using Social_Networking.Data;
using Social_Networking.Migrations;
using Social_Networking.Model;
using System;
using static Social_Networking.Exceptions.Exceptions;

class Program
{
    static void Main()
    {
        Main_Functional main = new Main_Functional();
        main.Func();
    }
}

public class Main_Functional
{
    public void Func()
    {
        while (true)
        {
            try
            {
                User user = new User();
                Console.WriteLine("Welcome To Our App");
                Console.WriteLine();
                Console.WriteLine("1.Register");
                Console.WriteLine("2.Login");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int Reg_Log))
                {
                    if (Reg_Log == 1)
                    {
                        user.Registration();
                        user.Login();
                    }
                    else if (Reg_Log == 2)
                    {
                        user.Login();
                        Post_Managment();
                    }
                    else
                    {
                        throw new WrongInputException();
                    }
                }
                else
                {
                    throw new WrongInputException();
                }
            }
            catch (WrongInputException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    public void Post_Managment() 
    {
        try
        {
            bool whilee = true;    
            while (whilee)
            {

                Posts posts = new Posts();
                User user1 = new User();
                FollowUsers f = new FollowUsers();
                Friends friends = new Friends();
                Messages messages = new Messages();
                string username = "";
                foreach (var user in Posts.Users_List)
                {
                   username =  user.UserName;
                }
                Console.WriteLine($"Welcome {username}!");
                Console.WriteLine();
                Console.WriteLine("1.ForYou Page") ;
                Console.WriteLine("2.Write  Posts");
                Console.WriteLine("3.Delete Posts");
                Console.WriteLine("4.See My Posts Posts");
                Console.WriteLine("5.Settings");
                Console.WriteLine("6.Friends!");
                Console.WriteLine("7.Exit From Account!");
                int PostChoice = Convert.ToInt32(Console.ReadLine());
                switch (PostChoice)
                {
                    case 1:
                        posts.For_You_Page();
                        break;
                    case 2:
                        posts.Write_Post();
                        break;
                    case 3:
                        posts.Delete_My_Post();
                        break;
                    case 4:
                        posts.See_My_Posts();
                        break;
                    case 5:
                        messages.Chats_Friends();
                        break;
                    case 6:friends.Friends_Adjustment();
                        break;
                    case 7:
                        user1.ExitFromAccount();
                        whilee = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please choose a number between 1 and 4.");
                        break;
                }
            }
        }
        
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}



