namespace AjSharpure.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UtilitiesTests
    {
        [TestMethod]
        public void ShouldGetZeroHashToNull()
        {
            Assert.AreEqual(0, Utilities.Hash(null));
        }

        [TestMethod]
        public void ShouldRecognizeSymbolAsEvaluable()
        {
            Symbol symbol = Symbol.Create("foo");

            Assert.IsTrue(Utilities.IsEvaluable(symbol));
        }

        [TestMethod]
        public void ShouldRecognizeConstantsAsEvaluable()
        {
            Assert.IsFalse(Utilities.IsEvaluable("foo"));
            Assert.IsFalse(Utilities.IsEvaluable(123));
        }

        [TestMethod]
        public void ShouldRecognizeIListAsEvaluable()
        {
            IList list = new ArrayList();

            list.Add(1);
            list.Add(2);
            list.Add(3);

            Assert.IsTrue(Utilities.IsEvaluable(list));
        }

        [TestMethod]
        public void ShouldRecognizeArrayAsNotEvaluable()
        {
            Assert.IsFalse(Utilities.IsEvaluable(new int[] { 1, 2, 3 }));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToConvertArrayToExpression()
        {
            Utilities.ToExpression(new int[] { 1, 2, 3 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldRaiseIfTryToConvertStringToExpression()
        {
            Utilities.ToExpression("foo");
        }

        [TestMethod]
        public void ShouldPrintInteger()
        {
            Assert.AreEqual("1", Utilities.PrintString(1));
            Assert.AreEqual("123", Utilities.PrintString(123));
        }

        [TestMethod]
        public void ShouldPrintString()
        {
            Assert.AreEqual("foo", Utilities.PrintString("foo"));
            Assert.AreEqual("bar", Utilities.PrintString("bar"));
        }

        [TestMethod]
        public void NullShouldBeEquivalentToNull()
        {
            Assert.IsTrue(Utilities.Equiv(null, null));
        }

        [TestMethod]
        public void NullShouldBeNotEquivalentToNotNull()
        {
            Assert.IsFalse(Utilities.Equiv(null, 1));
            Assert.IsFalse(Utilities.Equiv(1, null));
        }

        [TestMethod]
        public void ShouldConvertNullSequenceToNull()
        {
            Assert.IsNull(Utilities.ToSequence(null));
        }

        [TestMethod]
        public void ShouldConvertEmptySequenceToNull()
        {
            Assert.IsNull(Utilities.ToSequence(EmptyList.Instance));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldRaiseIfTryToConvertIntegerToSequence()
        {
            Utilities.ToSequence(2);
        }

        [TestMethod]
        public void ShouldConvertEnumeratorSequenceToItself()
        {
            ISequence sequence = EnumeratorSequence.Create((new int[] { 1, 2, 3 }).GetEnumerator());

            Assert.IsTrue(sequence == Utilities.ToSequence(sequence));
        }

        [TestMethod]
        public void ShouldEvaluateNilFalseAsFalseAndEverythingElseToNotFalse()
        {
            Assert.IsTrue(Utilities.IsFalse(null));
            Assert.IsTrue(Utilities.IsFalse(false));

            Assert.IsFalse(Utilities.IsFalse(true));
            Assert.IsFalse(Utilities.IsFalse("false"));
            Assert.IsFalse(Utilities.IsFalse(0));
            Assert.IsFalse(Utilities.IsFalse(123));
            Assert.IsFalse(Utilities.IsFalse("foo"));
            Assert.IsFalse(Utilities.IsFalse(Symbol.Create("false")));
            Assert.IsFalse(Utilities.IsFalse(Symbol.Create("true")));
            Assert.IsFalse(Utilities.IsFalse(Symbol.Create("nil")));
        }

        [TestMethod]
        public void ShouldCompareNumbers()
        {
            Assert.AreEqual(0, Utilities.Compare(0, 0));
            Assert.AreEqual(0, Utilities.Compare(1, 1));
            Assert.AreEqual(-1, Utilities.Compare(0, 1));
            Assert.AreEqual(-1, Utilities.Compare(1, 2));
            Assert.AreEqual(1, Utilities.Compare(1, 0));
            Assert.AreEqual(1, Utilities.Compare(2, 1));
        }

        [TestMethod]
        public void ShouldCompareStrings()
        {
            Assert.AreEqual(0, Utilities.Compare(string.Empty, string.Empty));
            Assert.AreEqual(0, Utilities.Compare("foo", "foo"));
            Assert.AreEqual(-1, Utilities.Compare("bar", "foo"));
            Assert.AreEqual(-1, Utilities.Compare("aa", "ab"));
            Assert.AreEqual(1, Utilities.Compare("foo", "bar"));
            Assert.AreEqual(1, Utilities.Compare("ab", "aa"));
        }

        [TestMethod]
        public void ShouldCompareSymbols()
        {
            Assert.AreEqual(0, Utilities.Compare(Symbol.Create("a"), Symbol.Create("a")));
            Assert.AreEqual(0, Utilities.Compare(Symbol.Create("foo"), Symbol.Create("foo")));
            Assert.AreEqual(-1, Utilities.Compare(Symbol.Create("bar"), Symbol.Create("foo")));
            Assert.AreEqual(-1, Utilities.Compare(Symbol.Create("aa"), Symbol.Create("ab")));
            Assert.AreEqual(1, Utilities.Compare(Symbol.Create("foo"), Symbol.Create("bar")));
            Assert.AreEqual(1, Utilities.Compare(Symbol.Create("ab"), Symbol.Create("aa")));
        }

        [TestMethod]
        public void ShouldReturnNullType()
        {
            Assert.IsNull(Utilities.GetType(null));
        }

        [TestMethod]
        public void ShouldReturnNullForUnknownType()
        {
            Assert.IsNull(Utilities.GetType("123"));
        }

        [TestMethod]
        public void ShouldReturnSystemIOFileInfoTypeUsingString()
        {
            Type type = Utilities.GetType("System.IO.FileInfo");

            Assert.IsNotNull(type);
            Assert.AreEqual("System.IO.FileInfo", type.FullName);
        }

        [TestMethod]
        public void ShouldReturnSystemIOFileInfoTypeUsingSymbol()
        {
            Type type = Utilities.GetType(Symbol.Create("System.IO.FileInfo"));

            Assert.IsNotNull(type);
            Assert.AreEqual("System.IO.FileInfo", type.FullName);
        }

        [TestMethod]
        public void ShouldGetSimpleEquals()
        {
            Assert.IsTrue(Utilities.Equals(null, null));
            Assert.IsTrue(Utilities.Equals("foo", "foo"));
            Assert.IsTrue(Utilities.Equals(Symbol.Create("bar"), Symbol.Create("bar")));

            Machine machine = new Machine();
            machine.CreateNamespace("foo");

            Assert.IsTrue(Utilities.Equals(Variable.Intern(machine, "foo","bar"), Variable.Intern(machine, "foo","bar")));
        }

        [TestMethod]
        public void ShouldGetTwoListsAsEquals()
        {
            IList list1 = new ArrayList();
            IList list2 = new ArrayList();

            for (int k=1; k<=10; k++) 
            {
                list1.Add(k);
                list2.Add(k);
            }

            Assert.IsTrue(Utilities.Equals(list1, list2));
            Assert.IsTrue(Utilities.Equals(list2, list1));
        }

        [TestMethod]
        public void ShouldGetTwoListsAsNotEquals()
        {
            IList list1 = new ArrayList();
            IList list2 = new ArrayList();

            for (int k = 1; k <= 10; k++)
            {
                list1.Add(k);
                list2.Add(k);
            }

            list2.Add(11);

            Assert.IsFalse(Utilities.Equals(list1, list2));
            Assert.IsFalse(Utilities.Equals(list2, list1));
        }

        [TestMethod]
        public void ShouldGetTwoArraysAsEquals()
        {
            Assert.IsTrue(Utilities.Equals(new int[] {1,2,3}, new int[] {1,2,3}));
        }

        [TestMethod]
        public void ShouldGetTwoArraysAsNotEquals()
        {
            Assert.IsFalse(Utilities.Equals(new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3 }));
            Assert.IsFalse(Utilities.Equals(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3, 4 }));
        }

        [TestMethod]
        public void ShouldConvertSymbolToItselfAsIObject()
        {
            Symbol symbol = Symbol.Create("foo");

            IObject result = Utilities.ToObject(symbol);

            Assert.IsNotNull(result);
            Assert.IsTrue(result == symbol);
        }

        [TestMethod]
        public void ShouldConvertListToIObject()
        {
            IList list = new ArrayList(new int[] { 1, 2, 3 });

            IObject result = Utilities.ToObject(list);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList));

            IList newlist = (IList)result;

            Assert.AreEqual(3, newlist.Count);
            Assert.AreEqual(1, newlist[0]);
            Assert.AreEqual(2, newlist[1]);
            Assert.AreEqual(3, newlist[2]);
        }

        [TestMethod]
        public void ShouldConvertDictionaryToIObject()
        {
            IDictionary dictionary = new Hashtable();

            dictionary["one"] = 1;
            dictionary["two"] = 2;
            dictionary["three"] = 3;

            IObject result = Utilities.ToObject(dictionary);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IDictionary));

            IDictionary newdict = (IDictionary)result;

            Assert.AreEqual(3, newdict.Count);
            Assert.IsTrue(newdict.Contains("one"));
            Assert.AreEqual(1, newdict["one"]);
            Assert.IsTrue(newdict.Contains("two"));
            Assert.AreEqual(2, newdict["two"]);
            Assert.IsTrue(newdict.Contains("three"));
            Assert.AreEqual(3, newdict["three"]);
        }
    }
}
