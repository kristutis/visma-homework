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
    public class OverridesTests
    {
        [TestMethod]
        public void EqualsOperator_EqualBooks_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            bool result = book1 == book2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EqualsMethod_EqualBooks_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            bool result = book1.Equals(book2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotEqualsOperator_NotEqualBooks_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Now, "12343");
            bool result = book1 != book2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_NullObject_ReturnsFalse()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "12343");
            bool result1 = null == book1;
            bool result2 = book1 == null;
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void GreaterThan_Greater_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "11");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Now, "11");
            bool result = book1 < book2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LessThan_Less_ReturnsTrue()
        {
            Book book1 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "22");
            Book book2 = new Book("sample name", "sample author", "cat", "en", DateTime.Today, "11");
            bool result = book1 > book2;
            Assert.IsTrue(result);
        }
    }
}
