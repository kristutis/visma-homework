using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    public class Books
    {
        public List<Book> BooksList { get; private set; }

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

        public Book GetBook(string isbn)
        {
            foreach (var book in BooksList)
            {
                if (book.Isbn == isbn)
                {
                    return book;
                }
            }
            return null;
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

        public void Delete(string isbn)
        {
            foreach (var book in BooksList)
            {
                if (book.Isbn == isbn)
                {
                    BooksList.Remove(book);
                    Console.WriteLine(book.ToString() + "\nRemoved sucessfully!");
                    return;
                }
            }
            Console.WriteLine("Book with isbn " + isbn + " does not exist!");
        }

        public bool Exists(string isbn)
        {
            foreach (var book in BooksList)
            {
                if (book.Isbn == isbn)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Book> GetFilteredData(string field, string option = null, string order = null)
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
