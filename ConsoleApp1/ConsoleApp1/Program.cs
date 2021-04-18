//https://gist.github.com/nanotaboada/6396437
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ConsoleApp1
{
    class Program
    {
        const string JSON_ARRAY_NAME = "books";
        static readonly string[] FIELDS = { "title", "author", "category", "language", "published", "isbn" };
        const string INPUT_FILE_PATH = @"../../../../../books.json";
        const string DELETE_COMMAND = "delete";
        const string LIST_COMMAND = "list";
        const string ADD_COMMAND = "add";
        const string TAKE_COMMAND = "take";
        const string RETURN_COMMAND = "return";
        const string QUIT_COMMAND = "quit";
        const string HELP_COMMAND = "help";
        const string HELLO_MESSAGE = @"Welcome to the library application!
                                    In this application you can list, add, take, return or delete a book from a library.
                                    Available commands:
                                    * To list all books: list [author <option> | category <option>  | language <option> | isbn <option> |
                                                             | name <option> | date <option> | taken | available] [ascending | descending]
                                    * To add a new book: add <isbn> <book_name> <author> <category> <language> <publication_date> 
                                    * To take a book from the library: take <name> <surname> <return_date> <book_isbn>
                                    * To return a book back to the library: return <name> <surname> <book_isbn>
                                    * To delete a book: delete <book_isbn>
                                    * To list commands: help
                                    * To exit program: quit";
        const string GOODBYE_MESSAGE = "Thank you for using the application. Have a good day.";

        static Books libraryBooks;
        static People clients;
        static void Main(string[] args)
        {
            Console.WriteLine(HELLO_MESSAGE.Replace("  ", ""));
            clients = new People();
            ReadDataFromFile();            
            string command = "";
            while (command.ToLower() != QUIT_COMMAND)
            {
                command = Console.ReadLine();
                if (command != QUIT_COMMAND)
                {
                    ProcessInput(command);
                }                
            }
            Console.WriteLine(GOODBYE_MESSAGE);
        }

        static void ProcessInput(string command)
        {
            string[] parts = command.Split(" ");
            switch (parts[0].ToLower())
            {
                case LIST_COMMAND:
                    ProcessList(parts);
                    break;
                case ADD_COMMAND:
                    ProcessAdd(parts);
                    break;
                case TAKE_COMMAND:
                    ProcessTake(parts);
                    break;
                case RETURN_COMMAND:
                    ProcessReturn(parts);
                    break;
                case DELETE_COMMAND:
                    ProcessDelete(parts);
                    break;
                case HELP_COMMAND:
                    Console.WriteLine(HELLO_MESSAGE.Replace("  ", ""));
                    break;
                default:
                    Console.WriteLine("Incorrect command");
                    Console.WriteLine("Available comands: {0}, {1}, {2}, {3}, {4}, {5}, {6}", LIST_COMMAND, ADD_COMMAND, TAKE_COMMAND, RETURN_COMMAND, DELETE_COMMAND, HELP_COMMAND, QUIT_COMMAND);
                    break;
            }
        }

        static void ProcessReturn(string[] parts)
        {
            if (parts.Length != 4)
            {
                Console.WriteLine("Incorrect return input");
                Console.WriteLine("Command:");
                Console.WriteLine("return <name> <surname> <book_isbn>");
                return;
            }
            string name = parts[1];
            string surname = parts[2];
            string isbn = parts[3];
            if (!clients.ClientExists(name, surname))
            {
                Console.WriteLine(String.Format("Client {0} {1} does not exist!", name, surname));
                return;
            }
            clients.ReturnBook(name, surname, isbn);
        }

        static void ProcessDelete(string[] parts)
        {
            if (parts.Length != 2)
            {
                Console.WriteLine("Incorrect delete input");
                Console.WriteLine("Command:");
                Console.WriteLine("delete <book_isbn>");
                return;
            }
            libraryBooks.Delete(parts[1]);
            UpdateFile();
        }

        static void ProcessTake(string[] parts)
        {
            if (parts.Length != 5)
            {
                Console.WriteLine("Incorrect take input");
                Console.WriteLine("Command:");
                Console.WriteLine("take <name> <surname> <return_date> <book_isbn>");
                return;
            }
            if (!libraryBooks.Exists(parts[4]))
            {
                Console.WriteLine("book with isbn " + parts[4] + " does not exist!");
                return;
            }
            bool found = false;
            foreach (var book in GetAvailableList())
            {
                if (book.Isbn == parts[4])
                {
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("book with isbn " + parts[4] + " is already taken!");
                return;
            }
            string name = parts[1];
            string surname = parts[2];
            DateTime date = new DateTime();
            if (!DateTime.TryParse(parts[3], out date))
            {
                Console.WriteLine("Wrong date format");
                return;
            }
            string isbn = parts[4];
            clients.TakeBook(name, surname, libraryBooks.GetBook(isbn), date);
        }

        static void ProcessAdd(string[] parts)
        {
            if (parts.Length != 7)
            {
                Console.WriteLine("incorrect add input");
                Console.WriteLine("Command:");
                Console.WriteLine("add <isbn> <book_name> <author> <category> <language> <publication_date>");
                return;
            }
            if (libraryBooks.Exists(parts[1]))
            {
                Console.WriteLine("book with isbn " + parts[1] + " already exists!");
                return;
            }

            DateTime date = new DateTime();
            if (!DateTime.TryParse(parts[6], out date))
            {
                Console.WriteLine("Wrong date format");
                return;
            }
            Book newBook = new Book(parts[2], parts[3], parts[4], parts[5], date, parts[1]);
            libraryBooks.AddBook(newBook);
            Console.WriteLine(newBook.ToString() + "\nAdded successfuly!");
            UpdateFile();
        }

        static void UpdateFile()
        {
            string jsonString = JsonSerializer.Serialize(new { books = libraryBooks.BooksList.ToArray() });
            File.WriteAllText(INPUT_FILE_PATH, jsonString);
        }

        static void ProcessList(string[] parts)
        {
            if (parts.Length == 1)
            {
                Console.WriteLine(libraryBooks.ToString());
                return;
            }
            string filterField = parts[1];
            if (filterField == "ascending" || filterField == "descending")
            {
                if (filterField == "ascending")
                {
                    var bs = from book in libraryBooks.BooksList
                             orderby book ascending
                             select book;
                    Console.WriteLine(new Books(bs.ToList()).ToString());
                    return;
                }
                else
                {
                    var bs = from book in libraryBooks.BooksList
                             orderby book descending
                             select book;
                    Console.WriteLine(new Books(bs.ToList()).ToString());
                    return;
                }
            }
            if (filterField.ToLower() == "available")
            {
                var availableBooks = parts.Length == 3 ? GetAvailableList(parts[2]) : GetAvailableList();
                Console.WriteLine(new Books(availableBooks).ToString());
                return;
            }
            if (filterField.ToLower() == "taken")
            {
                if (parts.Length == 3)
                {
                    clients.Sort(parts[2]);
                }
                Console.WriteLine(clients.ToString());
                return;
            }
            if (parts.Length < 3)
            {
                Console.WriteLine("Not enough arguments");
                Console.WriteLine("Command:");
                Console.WriteLine("list [author <option> | category <option>  | language <option> | isbn <option> | name<option> | date<option> | taken | available] [ascending | descending]");
                return;
            }
            string filterOption = parts[2];
            string order = parts.Length == 4 ? parts[3].ToLower() : null;            
            List<Book> filteredData = libraryBooks.GetFilteredData(filterField.ToLower(), filterOption.ToLower(), order);
            Console.WriteLine(new Books(filteredData).ToString());
        }

        static List<Book> GetAvailableList(string order = null)
        {
            List<Book> takenBooks = clients.GetTakenBooks();
            List<Book> availableBooks = new List<Book>();
            foreach (var book in libraryBooks.BooksList)
            {
                if (!takenBooks.Contains(book))
                {
                    availableBooks.Add(book);
                }
            }

            if (order != null)
            {
                if (order == "ascending")
                {
                    var bs = from book in availableBooks
                             orderby book ascending
                             select book;
                    availableBooks = bs.ToList();
                }
                else
                {
                    var bs = from book in availableBooks
                             orderby book descending
                             select book;
                    availableBooks = bs.ToList();
                }
            }

            return availableBooks;
        }

        static void ReadDataFromFile()
        {
            if (!File.Exists(INPUT_FILE_PATH))
            {
                throw new FileNotFoundException();
            }
            libraryBooks = new Books();
            string jsonString = File.ReadAllText(INPUT_FILE_PATH);
            using (JsonDocument jsonDocument = JsonDocument.Parse(jsonString))
            {
                JsonElement root = jsonDocument.RootElement;
                JsonElement booksElement = root.GetProperty(JSON_ARRAY_NAME);
                foreach (JsonElement jsonBook in booksElement.EnumerateArray())
                {
                    Book book = new Book();
                    JsonElement bookProperty = new JsonElement();
                    book.Name = jsonBook.TryGetProperty(FIELDS[0], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    book.Author = jsonBook.TryGetProperty(FIELDS[1], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    book.Category = jsonBook.TryGetProperty(FIELDS[2], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    book.Language = jsonBook.TryGetProperty(FIELDS[3], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    book.Isbn = jsonBook.TryGetProperty(FIELDS[5], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    string date = jsonBook.TryGetProperty(FIELDS[4], out bookProperty) ? bookProperty.GetString() : throw new Exception("Json element not found");
                    DateTime publicationDate = new DateTime();
                    if (!DateTime.TryParse(bookProperty.GetString(), out publicationDate))
                    {
                        throw new Exception("Wrong date format in " + INPUT_FILE_PATH + " file. Book's ISBN: " + book.Isbn);
                    }
                    book.PublicationDate = publicationDate;
                    libraryBooks.AddBook(book);
                }
            }
        }
    }
}
