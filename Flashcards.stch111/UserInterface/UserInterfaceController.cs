using FlashCards.stch111.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.stch111.UserInterface
{
    internal class UserInterfaceController
    {
        DatabaseController _database;
        public UserInterfaceController(DatabaseController database) 
        {
            _database = database;
            do 
            {
                ShowMainMenu();
            } while (true);
        }

        public void ShowMainMenu()
        {
            Console.Clear();
            Console.Write(
                "----------\n" +
                "1. Exit\n" +
                "2. Manage Stacks\n" +
                "3. Manage FlashCards\n" +
                "4. Study\n" +
                "5. View Study Session Data\n" +
                "----------\n" +
                "Press the key to change screens or exit.\n"
                );

            var userInput = Console.ReadKey();
            switch (userInput.Key)
            {
                case ConsoleKey.D1:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.D2:
                    ShowStackMenu();
                    break;
                case ConsoleKey.D3:
                    ShowFlashCardMenu();
                    break;
                case ConsoleKey.D4:
                    ShowStudyMenu();
                    break;
                case ConsoleKey.D5:
                    ShowSessionMenu();
                    break;
                default:
                    break;
            }
        }

        // List stacks and allow the user to select one by typing its name, non-case sensitive
        public void ShowStackMenu()
        {
            bool exitFlag = false;            
            do
            {
                Console.Clear();
                Console.Write(
                    "----------\n" +
                    "Stacks\n" +
                    "----------\n"
                    );
                var stacks = _database.GetStacks();
                stacks.ForEach(x => Console.WriteLine(x.Name));
                Console.Write("----------\n" +
                    "Input a current stack name to edit it.\n" +
                    "Input '0' to exit the stack menu.\n" +
                    "Input '1' to create a new stack.\n");

                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    exitFlag = true;
                }
                else if (userInput == "1")
                {
                    ShowStackCreate();
                    exitFlag = true;
                }
                else if (userInput != null)
                {
                    var selectedStack = stacks.Find(x => x.Name.ToLower() == userInput.ToLower());
                    if (selectedStack != null)
                    {
                        ShowStackEditMenu(selectedStack);
                        exitFlag = true;
                    }
                    else
                    {
                        Console.WriteLine("No matching stacks found. Press any key to continue.");
                        Console.ReadKey();
                    }
                }
            } while (!exitFlag);
        }

        public void ShowStackCreate()
        {
            bool exitFlag = false;
            var stacks = _database.GetStacks();
            do
            {
                Console.Clear();
                Console.WriteLine("Enter the name of your new stack.");
                var stackName = Console.ReadLine();
                if (stackName is null || stackName == "")
                {
                    Console.WriteLine("Error reading stack name.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                else if (stackName.Trim() == "1" || stackName.Trim() == "0")
                {
                    Console.WriteLine("Cannot name stack '0' or '1'.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                // Check if stack exists already
                else if (stacks.Exists(stack => stack.Name.ToLower() == stackName.ToLower()))
                {
                    Console.WriteLine($"A stack named '{stackName}' already exists.");
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                }
                else
                {
                    var rowsAffected = _database.CreateStack(stackName);
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("New stack added.");
                        Console.WriteLine("Press any key to continue.");
                        Console.ReadKey();
                    }
                    exitFlag = true;
                }
            } while(!exitFlag);
        }

        // List options for currently selected stack
        public void ShowStackEditMenu(Database.Stack stack)
        {
            bool exitFlag = false;
            do
            {
                Console.Clear();
                Console.Write(
                    "----------\n" +
                    $"Current working stack: {stack.Name}\n" +
                    "----------\n" +
                    "1. Return to stack menu\n" +
                    "2. View all flashcards in stack\n" +
                    "3. Create a flashcard in current stack\n" +
                    "4. Edit a flashcard\n" +
                    "5. Delete a flashcard\n" +
                    "6. Delete this stack and all related flashcards\n"
                    );
                var userInput = Console.ReadKey();
                switch (userInput.Key)
                {
                    case ConsoleKey.D1:
                        exitFlag = true;
                        break;
                    case ConsoleKey.D2:
                        ShowFlashCardsInStack(stack);
                        break;
                    case ConsoleKey.D3:
                        CreateFlashCard(stack);
                        break;
                    case ConsoleKey.D4:
                        
                        break;
                    case ConsoleKey.D5:
                        
                        break;
                    default:
                        break;
                }
            }
            while (!exitFlag);
        }

        // List all flashcards in current stack
        public void ShowFlashCardsInStack(Database.Stack stack)
        {
            Console.Clear();
            Console.Write(
                "----------\n" +
                $"Showing flashcards for stack: {stack.Name}\n" +
                "Front / Back\n" +
                "----------\n"
            );
            var flashCards = _database.GetFlashCards(stack.ID);
            flashCards.ForEach( flashCard => Console.WriteLine(flashCard.Front + " / " + flashCard.Back));
            Console.WriteLine("----------\nPress any key to go back.");
            Console.ReadKey();
        }

        public void CreateFlashCard(Database.Stack stack)
        {
            bool exitFlag = false;
            do 
            { 
                Console.Clear();
                Console.WriteLine("Please enter the front side flashcard text.");
                string front = Console.ReadLine() ?? "";
                Console.WriteLine("Please enter the back side flashcard text.");
                string back = Console.ReadLine() ?? "";
                Console.Write(
                    "Your flashcard is: \n" +
                    "----------\n" +
                    $"{front} / {back}\n" +
                    "----------\n" +
                    "Do you wish to confirm this as your flashcard?\n" +
                    "1. Yes\n" +
                    "2. No (Restart)\n" +
                    "3. No (Exit without saving)\n"
                );
                var input = Console.ReadKey();
                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        // Save new flashcard
                        var newFlashCard = new FlashCardDTO
                        {
                            Back=back,
                            Front=front
                        };
                        _database.CreateFlashCard(newFlashCard, stack.ID);
                        exitFlag = true;
                        break;
                    case ConsoleKey.D2:
                        exitFlag = false;
                        break;
                    case ConsoleKey.D3:
                        exitFlag = true;
                        break;
                }
            } while (!exitFlag);
        }

        public void ShowFlashCardMenu()
        {

        }

        public void ShowStudyMenu()
        {

        }

        public void ShowSessionMenu()
        {

        }
    }
}
