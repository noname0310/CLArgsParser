using Microsoft.VisualStudio.TestTools.UnitTesting;
using CLArgsParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLArgsParser.Tests
{
    [TestClass()]
    public class SliceTests
    {
        [TestMethod()]
        public void SliceTest()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                new Slice("test val", -1, 8);
            });
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                new Slice("test val", 0, 9);
            });
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                new Slice("test val", int.MaxValue, 8);
            });
            Assert.ThrowsException<IndexOutOfRangeException>(() =>
            {
                new Slice("test val", 0, int.MinValue);
            });
        }

        [TestMethod()]
        public void ToStringTest()
        {
            Assert.AreEqual("as", new Slice("asd", 0, 2).ToString());
        }

        [TestMethod()]
        public void CompareToTest()
        {
            Assert.IsTrue(0 == ((Slice)"test").CompareTo((Slice)"test"));
            Assert.IsTrue(0 == "test".Slice(0, 4).CompareTo((Slice)"test"));
            Assert.IsTrue(0 == "testasd".Slice(0, 4).CompareTo((Slice)"test"));
            Assert.IsTrue(0 == "asdtestasd".Slice(3, 7).CompareTo((Slice)"test"));

            Assert.IsTrue(0 == ((Slice)"test").CompareTo("test".Slice(0, 4)));
            Assert.IsTrue(0 == ((Slice)"test").CompareTo("asdtestasd".Slice(3, 7)));

            Assert.IsTrue(((Slice)"test") == "test".Slice(0, 4));
            Assert.IsFalse(((Slice)"test") != "test".Slice(0, 4));

            Assert.IsTrue(0 != ((Slice)"test").CompareTo("testasd".Slice(0, 5)));
            Assert.IsTrue(0 != ((Slice)"test").CompareTo("testasd".Slice(0, 3)));
            Assert.IsTrue(0 != ((Slice)"test").CompareTo("testasd".Slice(0, 1)));
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            Slice slice = (Slice)"asdfasdf";
            string expect = "asdfasdf";
            StringBuilder result = new();
            foreach (var item in slice)
            {
                result.Append(item);
            }
            Assert.AreEqual(expect, result.ToString());
        }

        [TestMethod()]
        public void TrimTest()
        {
            Slice a;
            a = "    str1234567  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().Trim(), a.Trim().ToString());
            a = (Slice)"    str12   34567  ";
            Assert.AreEqual(a.ToString().Trim(), a.Trim().ToString());
            a = "    str 12345 67  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().Trim(), a.Trim().ToString());
        }

        [TestMethod()]
        public void TrimStartTest()
        {
            Slice a;
            a = "    str1234567  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().TrimStart(), a.TrimStart().ToString());
            a = (Slice)"    str12   34567  ";
            Assert.AreEqual(a.ToString().TrimStart(), a.TrimStart().ToString());
            a = "    str 12345 67  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().TrimStart(), a.TrimStart().ToString());
        }

        [TestMethod()]
        public void TrimEndTest()
        {
            Slice a;
            a = "    str1234567  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().TrimEnd(), a.TrimEnd().ToString());
            a = (Slice)"    str12   34567  ";
            Assert.AreEqual(a.ToString().TrimEnd(), a.TrimEnd().ToString());
            a = "    str 12345 67  ".Slice(5, 7);
            Assert.AreEqual(a.ToString().TrimEnd(), a.TrimEnd().ToString());
        }

        [TestMethod()]
        public void SliceFromSliceTest()
        {
            Assert.AreEqual("asdfghjk".Slice(2, 6).ToString().Slice(1, 3), "asdfghjk".Slice(2, 6).SliceFromSlice(1, 3));
        }

        [TestMethod()]
        public void SubSliceTest()
        {
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(0, 4), "asdfasdfasdfasdfsss".Substring(0, 4));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(1, 4), "asdfasdfasdfasdfsss".Substring(1, 4));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(2, 7), "asdfasdfasdfasdfsss".Substring(2, 7));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(3, 6), "asdfasdfasdfasdfsss".Substring(3, 6));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(4, 3), "asdfasdfasdfasdfsss".Substring(4, 3));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(0), "asdfasdfasdfasdfsss".Substring(0));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(1), "asdfasdfasdfasdfsss".Substring(1));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(2), "asdfasdfasdfasdfsss".Substring(2));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(3), "asdfasdfasdfasdfsss".Substring(3));
            Assert.AreEqual(((Slice)"asdfasdfasdfasdfsss").SubSlice(4), "asdfasdfasdfasdfsss".Substring(4));
        }

        [TestMethod()]
        public void ExpendStartTest()
        {
            Slice slice = "abcdefghijklmnop".Slice(5, 10);
            Assert.AreEqual("cdefghij", slice.ExpendStart(3));
        }

        [TestMethod()]
        public void ExpendEndTest()
        {
            Slice slice = "abcdefghijklmnop".Slice(0, 5);
            Assert.AreEqual(slice.ExpendEnd(4), "abcdefghi");
        }
    }
}