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

            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                        // Run initial batch of commands to create database and tables if they do not yet exist
                        string[] initialCommands = [
                            "IF DB_ID('FlashCardsDB') IS NULL CREATE DATABASE FlashCardsDB;",
                            "USE FlashCardsDB;",
                            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Stacks')\r\n\tCREATE TABLE Stacks(\r\n\t\tID INT IDENTITY(1,1) PRIMARY KEY, \r\n\t\tName VARCHAR(255) UNIQUE\r\n\t\t);",
                            "IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='FlashCards')\r\n\tCREATE TABLE FlashCards(\r\n\t\tID INT IDENTITY(1,1) PRIMARY KEY,\r\n\t\tFront VARCHAR(255),\r\n\t\tBack VARCHAR(255),\r\n\t\tStackID int FOREIGN KEY REFERENCES Stacks(ID)\r\n\t\t);"
                        ];
                        foreach (var commandText in initialCommands)
                        {
                            using (SqlCommand command = new SqlCommand(commandText, connection))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        public List<Stack> GetStacks()
        {
            string connectionString = GetConnectionString();

            List<Stack> stacks = new List<Stack>();

            try
            {
                using (SqlConnection connection = new SqlConnection())
                using (SqlCommand useDbCommand = new SqlCommand("USE FlashCardsDB;", connection))
                using (SqlCommand selectCommand = new SqlCommand("SELECT ID, Name FROM stacks ORDER BY Name;", connection))
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    useDbCommand.ExecuteNonQuery();
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        stacks.Add(new Stack
                        {
                            ID = id,
                            Name = name
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            return stacks;
        }

        public List<FlashCardDTO> GetFlashCards(int stackID)
        {
            string connectionString = GetConnectionString();
            List<FlashCardDTO> flashCards = new List<FlashCardDTO>();
            try
            {
                using (SqlConnection connection = new SqlConnection())
                using (SqlCommand useDbCommand = new SqlCommand("USE FlashCardsDB;", connection))
                using (SqlCommand selectCommand = new SqlCommand("SELECT Front, Back FROM FlashCards WHERE StackID=@StackID;", connection))
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();                    
                    useDbCommand.ExecuteNonQuery();
                    selectCommand.Parameters.Add(new SqlParameter("@StackID", stackID));
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        flashCards.Add(new FlashCardDTO
                        {
                            Front = reader.GetString(0),
                            Back = reader.GetString(1),
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
            return flashCards;
        }

        public int CreateFlashCard(FlashCardDTO flashCardDTO, int stackID)
        {
            string connectionString = GetConnectionString();

            List<FlashCardDTO> flashCards = new List<FlashCardDTO>();

            int rowsAffected = -1; // Default

            try
            {
                using (SqlConnection connection = new SqlConnection())
                using (SqlCommand useDbCommand = new SqlCommand("USE FlashCardsDB;", connection))
                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO FlashCards (Front, Back, StackID) VALUES (@Front, @Back, @StackID)", connection))
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    useDbCommand.ExecuteNonQuery();
                    insertCommand.Parameters.Add(new SqlParameter("@Front", flashCardDTO.Front));
                    insertCommand.Parameters.Add(new SqlParameter("@Back", flashCardDTO.Back));
                    insertCommand.Parameters.Add(new SqlParameter("@StackID", stackID));
                    rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        throw new Exception("Error inserting row(s) into SQL table.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            return rowsAffected;
        }

        public int CreateStack(string stackName)
        {
            string connectionString = GetConnectionString();
            int rowsAffected = -1; // Default

            try
            {
                using (SqlConnection connection = new SqlConnection())
                using (SqlCommand useDbCommand = new SqlCommand("USE FlashCardsDB;", connection))
                using (SqlCommand insertCommand = new SqlCommand("INSERT INTO Stacks (Name) VALUES (@Name)", connection))
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    useDbCommand.ExecuteNonQuery();
                    insertCommand.Parameters.Add(new SqlParameter("@Name", stackName));
                    rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected <= 0)
                    {
                        throw new Exception("Error inserting row(s) into SQL table.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

            return rowsAffected;
        }
        private string GetConnectionString()
        {
            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }
    }
}
