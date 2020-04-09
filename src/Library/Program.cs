using System;

namespace Library
{
    class Program
    {
        private static LibraryParser libraryParser;
        static void Main(string[] args)
        {
            System.Console.WriteLine("Enter the path to the json file:");
            string path = System.Console.ReadLine().Replace("\"", "");
            if (path == "")
            {
                path = @".\books.json";
            }
            libraryParser = new LibraryParser(path);
            libraryParser.ReadLibraryFromJSON();
 
            string input;
            do {
                ViewHelp();
                System.Console.Write("Command: ");
                input = Console.ReadLine();
                CheckInput(input);
            } while (input.ToLower() != "q");
        }

        private static void CheckInput(string input)
        {
            string[] arguments;
            switch (input)
            {
                case "1":
                    System.Console.WriteLine("Adding a book");
                    arguments = GetArguments( new string[] {"Title?", "Author?", "ISBN?", "", ""} );
                    libraryParser.AddBook(new Book(arguments[0], arguments[1], int.Parse(arguments[2]), DateTime.Today.ToString("yyyy/MM/dd"), ""));
                    break;
                case "2":
                    System.Console.WriteLine("Deleting a book");
                    arguments = GetArguments( new string[] {"ISBN?"} );
                    libraryParser.DeleteBookByISBN(int.Parse(arguments[0]));
                    break;
                case "3":
                    System.Console.WriteLine("Finding a book");
                    arguments = GetArguments( new string[] {"Title, author or ISBN?"} );
                    Book book;
                    if ((null != (book = libraryParser.FindBookObject(arguments[0]))))
                    System.Console.WriteLine(book.ToString());
                    break;
                case "4":
                    System.Console.WriteLine("Finding books not rented in weeks");
                    arguments = GetArguments( new string[] {"How many weeks?"} );
                    libraryParser.FindNotRentedInWeeks(int.Parse(arguments[0]));
                    break;
                case "5":
                    System.Console.WriteLine("Renting a book");
                    arguments = GetArguments( new string[] {"ISBN?", "Your full name?"} );
                    libraryParser.RentABook(int.Parse(arguments[0]), arguments[1]);
                    break;
                case "6":
                    System.Console.WriteLine("Showing people who have rented books");
                    libraryParser.ShowPeopleWhoHaveBooks();
                    break;
                case "q":
                default:
                    break;
            }
        }
        
        private static string[] GetArguments(string[] questions)
        {
            string[] arguments = new string[questions.Length];
            int i = 0;
            foreach(string question in questions)
            {
                if (question != "")
                {
                    System.Console.WriteLine(questions[i]);
                    arguments[i++] = System.Console.ReadLine();
                } else
                {
                    arguments[i++] = "";
                }
            }
            return arguments;
        }

        private static void ViewHelp()
        {
            string help = @"1: Add a book (""title"" ""author"" ISBN ""borrower name"")
2: Delete a book (ISBN)
3: Find a book (""title"" / ""author"" / ISBN)
4: Find books not rented in weeks (amount of weeks)
5: Rent a book (ISBN ""full name"")
6: Show people who have rented books
q: Quit";
            System.Console.WriteLine($"\n{help}\n");
        }
    }
}