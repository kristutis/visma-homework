using ConsoleApp1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestLibrary.UnitTest
{
    [TestClass]
    public class OverridesTests
    {
        [TestMethod]
        public void Equals_EqualBooks_True()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            bool result1 = book1 == book2;
            bool result2 = book1.Equals(book2);
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void NotEquals_NotEqualBooks_True()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Now, "12343");
            bool result = book1 != book2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_NullObject_False()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            bool result1 = null == book1;
            bool result2 = book1 == null;
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void GreaterThan_Greater_True()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "1");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "22");
            bool result = book1 > book2;
            Assert.IsFalse(result);
        }
    }
}
