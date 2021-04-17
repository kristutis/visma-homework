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
        const string HELLO_MESSAGE = @"Welcome to the library application!
                                    In this application you can list, add, take, return or delete a book from a library.
                                    Available commands:
                                    To list all books: list [author <option> | category <option>  | language <option> | isbn <option> |
                                                             | name <option> | date <option> | taken | available] [ascending | descending]
                                    To add a new book: add <isbn> <book_name> <author> <category> <language> <publication_date> 
                                    To take a book from the library: take <name> <surname> <return_date> <book_isbn>
                                    To return a book back to the library: return <name> <surname> <book_isbn>
                                    To delete a book: delete <book_isbn>";
        const string GOODBYE_MESSAGE = "Thank you for using the application. Have a good day.";

        static Books libraryBooks;
        static List<Person> people;
        static void Main(string[] args)
        {
            Console.WriteLine(HELLO_MESSAGE.Replace("  ", ""));
            people = new List<Person>();
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
                    //ProcessTake(parts);
                    break;
                case RETURN_COMMAND:
                    //ProcessReturn(parts);
                    break;
                case DELETE_COMMAND:
                    //ProcessDelete(parts):
                    break;
                default:
                    Console.WriteLine("Incorrect command");
                    break;
            }
        }

        static void ProcessAdd(string[] parts)
        {
            if (parts.Length != 7)
            {
                Console.WriteLine("incorrect add input");
                return;
            }
            DateTime date = new DateTime();
            if (!DateTime.TryParse(parts[6], out date))
            {
                Console.WriteLine("Wrong date format");
                return;
            }
            //<isbn> <book_name> <author> <category> <language> <publication_date> 
            Book newBook = new Book(parts[2], parts[3], parts[4], parts[5], date, parts[1]);
            libraryBooks.AddBook(newBook);
            Console.WriteLine(newBook.ToString() + " added successfuly!");
            UpdateFile();
        }

        static void UpdateFile()
        {
            string jsonString = JsonSerializer.Serialize(new { books = libraryBooks.BooksList.ToArray() });
            File.WriteAllText(INPUT_FILE_PATH, jsonString);
            ReadDataFromFile();
        }

        static void ProcessList(string[] parts)
        {
            if (parts.Length == 1)
            {
                Console.WriteLine(libraryBooks.ToString());
                return;
            } else if (parts.Length < 3) {
                Console.WriteLine("Not enough arguments");
                return;
            }
            string filterField = parts[1];
            string filterOption = parts[2];
            string order = parts.Length == 4 ? parts[3].ToLower() : null;
            if (filterField.ToLower() == "taken" && people.Count != 0)
            {
                List<Book> takenBooks = libraryBooks.GetFilteredData(filterField.ToLower(), filterOption.ToLower(), order, people);
                Console.WriteLine(new Books(takenBooks).ToString());
                return;
            }
            List<Book> filteredData = libraryBooks.GetFilteredData(filterField.ToLower(), filterOption.ToLower(), order);
            Console.WriteLine(new Books(filteredData).ToString());
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
                foreach(JsonElement jsonBook in booksElement.EnumerateArray())
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
