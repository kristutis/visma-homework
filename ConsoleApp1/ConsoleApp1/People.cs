using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ConsoleApp1
{
    class People
    {
        public List<Person> Clients { get; private set; }
        public People()
        {
            Clients = new List<Person>();
        }

        public People(List<Person> clients)
        {
            Clients = clients;
        }

        public bool ClientExists(string name, string surname)
        {
            foreach (var client in Clients)
            {
                if (client.Name == name && client.Surname == surname)
                {
                    return true;
                }
            }
            return false;
        }

        public Person GetClient(string name, string surname)
        {
            foreach (var client in Clients)
            {
                if (client.Name == name && client.Surname == surname)
                {
                    return client;
                }
            }
            return null;
        }

        public Book ReturnBook(string name, string surname, string isbn)
        {
            var client = GetClient(name, surname);
            foreach (var book in client.RecievedBooks)
            {
                if (book.Key.Isbn == isbn)
                {
                    DateTime returnDate = book.Value;
                    if (DateTime.Now > returnDate)
                    {
                        Console.WriteLine("Sorry I'm late. I didn't want to come");
                    }
                    client.RecievedBooks.Remove(book.Key);
                    Console.WriteLine("Returned book:\n" + book.Key.ToString());
                    return book.Key;
                }
            }
            Console.WriteLine(String.Format("Client {0} {1} does not have a book with isbn {2}!", name, surname, isbn));
            return null;
        }

        public bool TakeBook(string name, string surname, Book book, DateTime returnDate)
        {
            DateTime now = DateTime.Now;
            int monthsApart = 12 * (returnDate.Year - now.Year) + returnDate.Month - now.Month;
            if (now > returnDate)
            {
                Console.WriteLine("Incorrect return date");
                return false;
            }
            if (monthsApart > 2)
            {
                Console.WriteLine("Cannot take a book for a longer period than two months");
                return false;
            }
            if (!ClientExists(name, surname))
            {
                Person newPerson = new Person(name, surname, book, returnDate);
                Clients.Add(newPerson);
                Console.WriteLine("Recieved book:\n" + book.ToString());
                return true;
            }
            foreach (var client in Clients)
            {
                if (client.Name == name && client.Surname == surname)
                {
                    if (client.RecievedBooks.Count >= 3)
                    {
                        Console.WriteLine("Taking more than 3 books is not allowed");
                        return false;
                    }
                    client.RecievedBooks.Add(book, returnDate);
                    Console.WriteLine("Recieved book:\n" + book.ToString());
                    return true;
                }
            }
            return false;
        }

        public void Sort(string order)
        {
            if (order == "ascending")
            {
                var clients = from client in Clients
                         orderby client.Name ascending
                         select client;
                Clients = clients.ToList();
            }
            else
            {
                var clients = from client in Clients
                              orderby client.Name descending
                              select client;
                Clients = clients.ToList();
            }
        }

        public List<Book> GetTakenBooks()
        {
            List<Book> books = new List<Book>();
            if (Clients.Count == 0)
            {
                return books;
            }
            foreach (var client in Clients)
            {
                foreach (var book in client.RecievedBooks)
                {
                    books.Add(book.Key);
                }
            }
            return books;
        }

        public override string ToString()
        {
            if (Clients.Count == 0)
            {
                return "No books are taken from the library.";
            }
            string msg = "Taken books:\n";
            foreach (var client in Clients)
            {
                foreach (var book in client.RecievedBooks)
                {
                    msg += String.Format("Name: {0} | Surname: {1} | ReturnDate: {2} | Book: {3}\n", client.Name, client.Surname, book.Value, book.Key.ToString());
                }
            }
            return msg;
        }
    }
}
