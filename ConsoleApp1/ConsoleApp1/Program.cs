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
        static readonly string[] FIELDS = { "title", "author", "subtitle", "website", "published", "isbn" };
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
                                    To add a new book: add <book_name> <author> <category> <language> <publication_date> <isbn>
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
            if (!DateTime.TryParse(parts[5], out date))
            {
                Console.WriteLine("Wrong date format");
                return;
            }
            Book newBook = new Book(parts[1], parts[2], parts[3], parts[4], date, parts[6]);
            libraryBooks.AddBook(newBook);
            Console.WriteLine(newBook.ToString() + " added successfuly!");
            UpdateFile();
        }

        static void UpdateFile()
        {
            //upload()
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
            List<Book> filteredData = libraryBooks.GetFilteredData(filterField.ToLower(), filterOption.ToLower(), order);
            Books filteredBooks = new Books(filteredData);
            Console.WriteLine(filteredBooks.ToString());
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

    public class Books
    {
        public List<Book> BooksList { get; set; }

        public Books()
        {
            BooksList = new List<Book>();
        }

        public Books(List<Book> books)
        {
            BooksList = books;
        }

        public void AddBook(Book book)
        {
            BooksList.Add(book);
        }

        public void RemoveBook(Book book)
        {
            BooksList.Remove(book);
        }

        public override string ToString()
        {
            if (BooksList.Count != 0)
            {
                string data = "Books list (count="+ BooksList.Count + ")\n";
                foreach (var book in BooksList)
                {
                    data += book.ToString()+"\n";
                }
                return data;
            }
            return base.ToString();
        }

        public List<Book> GetFilteredData(string field, string option, string order=null)
        {
            List<Book> books = new List<Book>();
            switch (field)
            {
                case "name":
                    books = BooksList.Where(book => book.Name.ToLower().Contains(option)).ToList();
                    break;
                case "author":
                    books = BooksList.Where(book => book.Author.ToLower().Contains(option)).ToList();
                    break;
                case "category":
                    books = BooksList.Where(book => book.Category.ToLower().Contains(option)).ToList();
                    break;
                case "language":
                    books = BooksList.Where(book => book.Language.ToLower().Contains(option)).ToList();
                    break;
                case "date":
                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(option, out date))
                    {
                        Console.WriteLine("Wrong date format");
                        return new List<Book>();
                    }
                    books = BooksList.Where(book => book.PublicationDate.CompareTo(date) > 0).ToList();
                    break;
                case "isbn":
                    books = BooksList.Where(book => book.Isbn.ToLower().Contains(option)).ToList();
                    break;
                case "taken":
                    //ss
                    break;
                case "available":
                    //ss
                    break;
                default:
                    Console.WriteLine("Unknown field");
                    return new List<Book>();
            }
            
            if (order != null)
            {
                if (order == "ascending")
                {
                    var bs = from book in books
                             orderby book ascending
                             select book;
                    return bs.ToList();
                } else
                {
                    var bs = from book in books
                             orderby book descending
                             select book;
                    return bs.ToList();
                }                
            }            
            return books;
        }
    }
    
    public class Book : IComparable<Book>, IEquatable<Book>
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Isbn { get; set; }

        public Book()
        { 
        }

        public Book(string name, string author, string category, string language, DateTime publicationDate, string isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            Language = language;
            PublicationDate = publicationDate;
            Isbn = isbn;
        }

        public static bool operator ==(Book book1, Book book2)
        {
            return book1.Equals(book2);
        }

        public static bool operator !=(Book book1, Book book2)
        {
            return !book1.Equals(book2);
        }

        public static bool operator <(Book book1, Book book2)
        {
            return book1.CompareTo(book2) < 0 ? true : false;
        }

        public static bool operator >(Book book1, Book book2)
        {
            return !(book1<book2);
        }

        public static bool operator <=(Book book1, Book book2)
        {
            if (book1.Equals(book2))
            {
                return true;
            }
            return book1.CompareTo(book2) < 0 ? true : false;
        }

        public static bool operator >=(Book book1, Book book2)
        {
            if (book1.Equals(book2))
            {
                return true;
            }
            return !(book1<=book2);
        }

        public bool Equals(Book other)
        {
            return Name.Equals(other.Name) && Author.Equals(other.Author) && Category.Equals(other.Category) 
                && Language.Equals(other.Language) && PublicationDate.Equals(other.PublicationDate) && Isbn.Equals(other.Isbn);
        }

        public int CompareTo(Book other)
        {
            if (!Name.Equals(other.Name))
                return Name.CompareTo(other.Name);
            if (!Author.Equals(other.Author))
                return Author.CompareTo(other.Author);
            if (!Category.Equals(other.Category))
                return Category.CompareTo(other.Category);
            if (!Language.Equals(other.Language))
                return Language.CompareTo(other.Language);
            if (!PublicationDate.Equals(other.PublicationDate))
                return PublicationDate.CompareTo(other.PublicationDate);
            if (!Isbn.Equals(other.Isbn))
                return Isbn.CompareTo(other.Isbn);
            return 0;
        }

        public override string ToString()
        {
            if (Name != null && Author != null && Category != null && Language != null && PublicationDate != null && Isbn != null)
            {
                return String.Format("Book({0}; {1}; {2}; {3}; {4}; {5})", Isbn, Name, Author, Category, Language, PublicationDate.ToString("dd/MM/yyyy"));
            }
            return base.ToString();            
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        Dictionary<Book, DateTime> RecievedBooks { get; set; }

        public Person()
        {
            RecievedBooks = new Dictionary<Book, DateTime>();
        }

        public Person(string name, string surname)
        {
            Name = name;
            Surname = surname;
            RecievedBooks = new Dictionary<Book, DateTime>();
        }
    }
}
