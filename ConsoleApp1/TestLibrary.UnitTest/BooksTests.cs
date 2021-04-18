using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary.UnitTest
{
    [TestClass]
    public class BooksTests
    {
        [TestMethod]
        public void ExistsByIsbn_ExistingIsbn_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "22");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Now, "11");
            Books books = new Books(new List<Book>() { book1, book2 });
            bool result = books.Exists(book1.Isbn);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckFilter_FilterByAuthor_ReturnFilteredData()
        {
            Book book1 = new Book("sample name1", "sample_author1", "cat", "en", DateTime.Today, "111");
            Book book2 = new Book("sample name2", "sample author2", "cat", "en", DateTime.Now, "222");
            Book book3 = new Book("sample_name3", "sample author3", "cat", "en", DateTime.Now, "333");
            Book book4 = new Book("sample_name4", "sample_author4", "cat", "en", DateTime.Now, "444");
            Books books = new Books(new List<Book>() { book1, book2, book3, book4 });
            List<Book> result = books.GetFilteredData("author", "sample_author", "ascending");
            Assert.AreEqual(result[0], book1);
            Assert.AreEqual(result[1], book4);
        }
    }
}
