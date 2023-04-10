using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
namespace User_Registration;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using User_Registration.Data;

class Program
{




    static void Main(string[] arg)
    {

      
        try
        {
            var userData = new Dictionary<string, string>();

            string userId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Console.Write("Enter your Firstname: ");
            string firstname = Console.ReadLine();
            userData.Add("Firstname", firstname);

            Console.Write("Enter your Lastname: ");
            string lastname = Console.ReadLine();
            userData.Add("Lastname", lastname);

            Console.Write("Enter your Email Address: ");
            string mail = Console.ReadLine();
            userData.Add("Email", mail);

            Console.Write("Enter your Password: ");
            string password = Console.ReadLine();
            userData.Add("Password", password);
            string hashedPassword = HashPassword(password);

            Console.Write("Enter your Phone Number: ");
            string phonenumber = Console.ReadLine();
            userData.Add("PhoneNumber", phonenumber);
            string tableName = "users";
            string[] columns = { "Userid", "Firstname", "lastname", "emai", " Password", "PhoneNumber" };
            string[] values = {   userId,  firstname, lastname, mail, password, phonenumber };
            string columnList = string.Join(", ", columns);
            string valueList = string.Join(", ", values.Select(v => $"'{v}'"));
            string query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, columnList, valueList);

            string connectionString = @"server=localhost;uid=root;pwd=Ayevbosaimade1;database=project;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("User registration successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }



            RegisterUser(userData, "your_connection_string_here");
            MailAddress email = new MailAddress(mail);
        }

        catch (FormatException)
        {
            Console.WriteLine("The email is invalid.");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
        }

        Console.Read();
    }
    static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
    static void RegisterUser(Dictionary<string, string> userData, string connectionString)
    {
        using (var context = new ApplicationDbContext(connectionString))
        {
            var user = new User
            {
                Firstname = userData["Firstname"],
                Lastname = userData["Lastname"],
                Email = userData["Email"],
                Password = userData["Password"],
                PhoneNumber = userData["PhoneNumber"]
            };
           
        }

    }
   

}
