using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ConsoleApp1
{
    public class Book : IComparable<Book>, IEquatable<Book>
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        [JsonPropertyName("language")]
        public string Language { get; set; }
        [JsonPropertyName("published")]
        public DateTime PublicationDate { get; set; }
        [JsonPropertyName("isbn")]
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
            if (object.ReferenceEquals(book1, null))
            {
                return object.ReferenceEquals(book2, null);
            }
            return book1.Equals(book2);
        }

        public static bool operator !=(Book book1, Book book2)
        {
            if (object.ReferenceEquals(book1, null) || object.ReferenceEquals(book2, null))
            {
                return !(book1==book2);
            }
            return !book1.Equals(book2);
        }

        public static bool operator <(Book book1, Book book2)
        {
            return book1.CompareTo(book2) < 0 ? true : false;
        }

        public static bool operator >(Book book1, Book book2)
        {
            return !(book1 < book2);
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
            return !(book1 <= book2);
        }

        public bool Equals(Book other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return object.ReferenceEquals(this, null);
            }
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
                return String.Format("{0,-14} | {1,-42} | {2,-20} | {3,-20} | {4,-3} | {5,-15}", Isbn, Name, Author, Category, Language, PublicationDate.ToString("dd/MM/yyyy"));
            }
            return base.ToString();
        }
    }
}
