using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
namespace User_Registration;
using System.Net.Mail;
using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            string userId = Guid.NewGuid().ToString();
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
            string hashedPassword = HashPassword(password);

            userData.Add("Password", hashedPassword);


            Console.Write("Enter your Phone Number: ");
            string phonenumber = Console.ReadLine();
            userData.Add("PhoneNumber", phonenumber);
            string tableName = "USER";
            string[] columns = { "Userid", "Firstname", "lastname", "email", " hashedPassword", "PhoneNumber" };
            string[] values = {   userId,  firstname, lastname, mail, hashedPassword, phonenumber };
            string columnList = string.Join(", ", columns);
            string valueList = string.Join(", ", values.Select(v => $"'{v}'"));
            string query = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", tableName, columnList, valueList);

            string connectionString = @"server=localhost;uid=root;pwd=Ayevbosaimade1;database=Project;";
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

   
  

        public void ConfigureServices(IServiceCollection services)
            => services.AddDbContext<ApplicationDbContext>();

  

    static string  HashPassword(string password)
    {
        byte[] salt = new byte[16];
        new RNGCryptoServiceProvider().GetBytes(salt);

        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);


    }

    static void RegisterUser(Dictionary<string, string> userData, string connectionString)
    {
        using var context = new ApplicationDbContext(connectionString);
        var user = new User
        {
            Firstname = userData["Firstname"],
            Lastname = userData["Lastname"],
            Email = userData["Email"],
            hashedPassword = userData["Password"],
            PhoneNumber = userData["PhoneNumber"]

        };

    }
   

}
