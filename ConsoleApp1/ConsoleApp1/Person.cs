using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Dictionary<Book, DateTime> RecievedBooks { get; set; }

        public Person()
        {
            RecievedBooks = new Dictionary<Book, DateTime>();
        }

        public Person(string name, string surname, Book book, DateTime returnDate)
        {
            Name = name;
            Surname = surname;
            RecievedBooks = new Dictionary<Book, DateTime>();
            RecievedBooks.Add(book, returnDate);
        }

        public override string ToString()
        {
            if (Name != null && Surname != null && RecievedBooks != null)
            {
                if (RecievedBooks.Count == 0)
                {
                    return String.Format("{0,-10} | {1,-10} | {2,-150}", Name, Surname, "No books taken");
                }
                string msg = "";
                foreach (var book in RecievedBooks)
                {
                    msg += String.Format("{0,-10} | {1,-10} | {2:-10} | {3,-150}", Name, Surname, book.Key, book.Value.ToString());
                }
                return msg;
            }
            return base.ToString();
        }
    }
}
