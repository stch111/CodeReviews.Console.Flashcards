using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.stch111.UserInterface
{
    enum Screen
    {
        MainMenu,
        StackMenu,
        FlashCardMenu,
        StudyMenu,
        SessionMenu
    }
    internal class UserInterfaceController
    {
        Screen screen = Screen.MainMenu;
        DatabaseController _database;
        public UserInterfaceController(DatabaseController database) 
        {
            _database = database;
            do 
            {
                switch(screen) 
                {
                    case Screen.MainMenu:
                        ShowMainMenu();
                        break;
                    case Screen.StackMenu:
                        ShowStackMenu();
                        break;
                    case Screen.FlashCardMenu:
                        ShowFlashCardMenu();
                        break;
                    case Screen.StudyMenu:
                        ShowStudyMenu();
                        break;
                    case Screen.SessionMenu:
                        ShowSessionMenu();
                        break;
                    default:
                        break;
                }
            } while (true);
        }

        public void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine(
                "----------\n" +
                "1. Exit\n" +
                "2. Manage Stacks\n" +
                "3. Manage FlashCards\n" +
                "4. Study\n" +
                "5. View Study Session Data\n" +
                "----------\n" +
                "Press the key to change screens or exit."
                );

            var userInput = Console.ReadKey();
            switch (userInput.Key)
            {
                case ConsoleKey.D1:
                    Environment.Exit(0);
                    break;
                case ConsoleKey.D2:
                    screen = Screen.StackMenu;
                    break;
                case ConsoleKey.D3:
                    screen = Screen.FlashCardMenu;
                    break;
                case ConsoleKey.D4:
                    screen = Screen.StudyMenu;
                    break;
                case ConsoleKey.D5:
                    screen = Screen.SessionMenu;
                    break;
                default:
                    break;
            }
        }

        public void ShowStackMenu()
        {
            bool exitFlag = false;            
            do
            {
                Console.Clear();
                Console.WriteLine(
                    "----------\n" +
                    "Stacks\n" +
                    "----------"
                    );
                var stacks = _database.GetStacks();
                stacks.ForEach(x => Console.WriteLine(x.Name));
                Console.WriteLine("----------\nInput a current stack name or input 0 to exit to main menu.");
                var userInput = Console.ReadLine();
                if (userInput == "0")
                {
                    exitFlag = true;
                    screen = Screen.MainMenu;
                }
                if (userInput != null)
                {
                    var selectedStack = stacks.Find(x => x.Name.ToLower() == userInput.ToLower());
                    if (selectedStack != null)
                    {
                        ShowStackEditMenu(selectedStack.Name);
                    }
                }
            } while (!exitFlag);
        }

        public void ShowStackEditMenu(string stackName)
        {
            bool exitFlag = false;
            do
            {
                Console.Clear();
                Console.WriteLine(
                    "----------\n" +
                    $"Current working stack: {stackName}\n" +
                    "----------\n" +
                    "1. Return to stack menu\n" +
                    "2. View all flashcards in stack\n" +
                    "3. Create a flashcard in current stack\n" +
                    "4. Edit a flashcard\n" +
                    "5. Delete a flashcard"
                    );
                var userInput = Console.ReadKey();
                switch (userInput.Key)
                {
                    case ConsoleKey.D1:
                        exitFlag = true;
                        break;
                    case ConsoleKey.D2:
                        
                        break;
                    case ConsoleKey.D3:
                        
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
