using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Library
{
    internal class LibraryParser
    {
        private List<Book> books = new List<Book>();
        private string jsonPath;

        public LibraryParser(string jsonPath)
        {
            this.jsonPath = jsonPath;
        }
        public void ReadLibraryFromJSON()
        {
            string jsonString = File.ReadAllText(jsonPath);
            books = JsonSerializer.Deserialize<List<Book>>(jsonString);
        }

         public void WriteLibraryToJSON()
        {
            string jsonString = JsonSerializer.Serialize(books);
            File.WriteAllText(jsonPath, jsonString);
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            WriteLibraryToJSON();
            System.Console.WriteLine($"Book \"{book.title}\" successfuly added to the library");
        }

        public void DeleteBookByISBN(int ISBN)
        {
            Book book = books.Find(x => x.ISBN == ISBN);
            books.Remove(book);
            WriteLibraryToJSON();
            System.Console.WriteLine($"Book successfuly deleted from the library");
        }

        internal Book FindBookObject(string argument)
        {   
            Book found;
            if ((found = books.FirstOrDefault(x => x.title == argument)) == null)
            {
                if ((found = books.FirstOrDefault(x => x.author == argument)) == null)
                {
                    if (!Regex.IsMatch(argument, @"^\d+$") || (found = books.FirstOrDefault(x => x.ISBN == int.Parse(argument))) == null)
                    {
                        System.Console.WriteLine("Book not found");
                        return null;
                    }
                }
            }
            return found;
        }

        internal void FindNotRentedInWeeks(int weeks)
        {
            var result = books.FindAll(
                    x => (int)(DateTime.Today - DateTime.Parse(x.lastRentalDate))
                    .TotalDays / 7 >= weeks)
                    .ToArray();
            foreach(Book book in result)
            {
                System.Console.WriteLine(book.ToString() + "\n");
            }
        }

        internal void RentABook(int ISBN, string borrowerName)
        {
            Book book = FindBookObject(ISBN.ToString());
            int index;
            if (book != null)
            {
                index = books.FindIndex(x => x.ISBN == ISBN);
                if (index != -1)
                {
                    books[index].borrowerName = borrowerName.Replace("\"","");
                    books[index].lastRentalDate = DateTime.Today.ToString("yyyy/MM/dd");
                    WriteLibraryToJSON();
                    System.Console.WriteLine($"Book \"{book.title}\" successfuly rented to {book.borrowerName.Replace("\"","")}");
                }
            }
        }

        internal void ShowPeopleWhoHaveBooks()
        {
            var dict = books.GroupBy(x => x.borrowerName).Select(x => new { Stuff = x.Key, Count = x.Count() });
            Dictionary<string, int> dictionary = dict.ToDictionary(g => g.Stuff, g => g.Count);
            dictionary.Remove("");      // don't list not rented books
            var lines = dictionary.Select(kvp => kvp.Key + ": " + kvp.Value.ToString());
            System.Console.WriteLine(string.Join(Environment.NewLine, lines));
        }
    }
}