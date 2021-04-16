using System;
using System.Collections.Generic;

namespace ConsoleApp1
{    
    class Program
    {
        const string QUIT_COMMAND = "quit";
        const string HELLO_MESSAGE = "hello";
        const string GOODBYE_MESSAGE = "Thank you for using our product. Have a good day.";

        static void Main(string[] args)
        {
            Console.WriteLine(HELLO_MESSAGE);
            //readDataFromFile()
            string command = "";
            while (command != QUIT_COMMAND)
            {
                command = Console.ReadLine();
                ProcessInput(command);
            }
            Console.WriteLine(GOODBYE_MESSAGE);
        }

        static void ProcessInput(string command)
        {
            Console.WriteLine(command);
        }
    }

    public class Books
    {
        public List<Book> BooksList { get; set; }

        public Books()
        {
        }

        public Books(List<Book> books)
        {
            BooksList = books;
        }

        public void AddBook(Book book)
        {
            BooksList.Add(book);
        }

        public void RemoveBook(string name)
        {

        }
    }
    
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Isbn { get; set; }

        public Book()
        { 
        }

        public Book(string name, string author, string category, DateTime publicationDate, string isbn)
        {
            Name = name;
            Author = author;
            Category = category;
            PublicationDate = publicationDate;
            Isbn = isbn;
        }
    }
}
