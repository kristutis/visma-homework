using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
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
