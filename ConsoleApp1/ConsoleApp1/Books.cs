using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
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
                string data = "Books list (count=" + BooksList.Count + ")\n";
                foreach (var book in BooksList)
                {
                    data += book.ToString() + "\n";
                }
                return data;
            }
            return base.ToString();
        }

        public List<Book> GetFilteredData(string field, string option, string order = null, List<Person> people = null)
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
                    if (people == null)
                    {
                        Console.WriteLine("Error no passed people to filter method");
                        return new List<Book>();
                    }
                    if (people.Count == 0)
                    {
                        Console.WriteLine("All books are available!");
                        return new List<Book>();
                    }
                    books = GetTakenBooks(people);
                    break;
                case "available":
                    books = BooksList;
                    break;
                default:
                    Console.WriteLine("Unknown field");
                    return new List<Book>();
            }

            List<Book> GetTakenBooks(List<Person> people)
            {
                List<Book> takenBooks = new List<Book>();
                foreach (var person in people)
                {
                    foreach (var book in person.RecievedBooks.Keys)
                    {
                        takenBooks.Add(book);
                    }
                }                
                return takenBooks;
            }

            if (order != null)
            {
                if (order == "ascending")
                {
                    var bs = from book in books
                             orderby book ascending
                             select book;
                    return bs.ToList();
                }
                else
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
}
