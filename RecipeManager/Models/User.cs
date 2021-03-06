﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Collections.Generic;

namespace RecipeManager.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        //public string PassHash { get; set; }
        //public string Salt { get; set; }
    }

    public static class UserDb
    {
        public static User ValidateUser(string username, string password)
        {
            User user = null;

            var connection = MySqlProvider.Connection;

            MySqlCommand validateCommand = connection.CreateCommand();
            validateCommand.CommandText = "SELECT ValidateUser(@username, @password)";
            validateCommand.Parameters.AddWithValue("@username", username);
            validateCommand.Parameters.AddWithValue("@password", password);

            MySqlDataReader reader = null;
            try
            {
                reader = validateCommand.ExecuteReader();
                if (reader.Read())
                {
                    user = new User() {
                        Username = username
                        //UserId = Convert.ToInt32(reader["storedId"])
                    };
                }
            }
            finally
            {
                reader?.Close();
            }

            return user;
        }

        public static User CreateUser(string username, string password)
        {
            User user = null;

            var connection = MySqlProvider.Connection;

            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "CALL CreateUser(@username, @password)";
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            try
            {
                command.ExecuteNonQuery();

                user = new User() { Username = username };
            }
            catch
            {
                return null;
            }
           
            return user;
        }

        public static List<User> SelectByAllUsers()
        {

            List<User> output = new List<User>();
            MySqlConnection connection = MySqlProvider.Connection;
            MySqlCommand command = connection.CreateCommand();
            //command.CommandType = CommandType.Text;
            command.CommandText = "SELECT UserId,Username FROM Users order by UserId"; //"SELECT RecipeName FROM RecipeLists JOIN Recipes on RecipeLists.RecipeId = Recipes.RecipeId";

            MySqlDataReader Reader = null;
            try
            {
                //connection.Open();
                Reader = command.ExecuteReader();
                if (Reader.Read())
                {
                    do
                    {
                        var user = new User()
                        {
                            UserId = Convert.ToInt32(Reader["UserId"]),
                            Username = Convert.ToString(Reader["Username"]),
                            // PassHash = Reader["PassHash"].ToString()


                        };
                        output.Add(user);
                    } while (Reader.Read());
                }

            }
            catch (MySqlException ex)
            {
                output.Add(new User() { Username = ex.Message });
            }
            finally
            {
                Reader?.Close();
            }

            return output;
        }

        internal static User GetUserInfo(int UserId)
        {
            User output = new User();
            MySqlConnection connection = MySqlProvider.Connection;

            MySqlCommand Command = connection.CreateCommand();

            Command.Parameters.AddWithValue("@param1", UserId);
            Command.CommandText = "SELECT Username FROM Users WHERE UserId = @param1";

            MySqlDataReader recipeReader = null;
            
            try
            {

                recipeReader = Command.ExecuteReader();
                if (recipeReader.Read())
                {

                    var user = new User()
                    {
                        
                        Username = Convert.ToString(recipeReader["UserName"]),
                       
                    };
                    output = user;

                }
            }
            catch (MySqlException ex)
            {
                
            }


            finally
            {
                recipeReader?.Close();
            }

            return output;
        }
        
    }


}