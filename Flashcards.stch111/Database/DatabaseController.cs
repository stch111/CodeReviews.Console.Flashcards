using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FlashCards.stch111.Database;
using Microsoft.Data.SqlClient;

namespace FlashCards.stch111
{
    public class DatabaseController
    {
        public DatabaseController()
        { 
            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                try
                {
                    // Run initial batch of commands to create database and tables if they do not yet exist
                    string[] initialCommands = [
                        "IF DB_ID('FlashCardsDB') IS NULL CREATE DATABASE FlashCardsDB;",
                        "USE FlashCardsDB;",
                        "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Stacks')\r\n\tCREATE TABLE Stacks(\r\n\t\tID INT IDENTITY(1,1) PRIMARY KEY, \r\n\t\tName VARCHAR(255) UNIQUE\r\n\t\t);",
                        "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='FlashCards')\r\n\tCREATE TABLE FlashCards(\r\n\t\tID INT NOT NULL PRIMARY KEY,\r\n\t\tQuestion VARCHAR(255),\r\n\t\tAnswer VARCHAR(255),\r\n\t\tStackID int FOREIGN KEY REFERENCES Stacks(ID)\r\n\t\t);"
                    ];
                    foreach (var commandText in initialCommands)
                    {
                        using (SqlCommand command = new SqlCommand(commandText, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    Environment.Exit(0);
                }
            }
        }

        public List<StackDTO> GetStacks()
        {
            string connectionString = GetConnectionString();

            List<StackDTO> stacks = new List<StackDTO>();

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                try
                {
                    SqlCommand useDbCommand = new SqlCommand("USE FlashCardsDB;", connection);
                    useDbCommand.ExecuteNonQuery();
                    
                    using (SqlCommand command = new SqlCommand("SELECT Name FROM stacks ORDER BY Name;", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            stacks.Add(new StackDTO
                            {
                                Name = name
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                }
                return stacks;
            }
        }

        private string GetConnectionString()
        {
            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }
    }
}
