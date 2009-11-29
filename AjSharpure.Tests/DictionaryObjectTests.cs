namespace AjSharpure.Tests
{
    using System;
    using System.Text;
    using System.Collections;
    using System.Linq;

    using AjSharpure;
    using AjSharpure.Language;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionaryObjectTests
    {
        private DictionaryObject dictionary;

        [TestInitialize]
        public void SetUpDictionary()
        {
            Hashtable values = new Hashtable();
            values["one"] = 1;
            values["two"] = 2;
            values["three"] = 3;

            this.dictionary = new DictionaryObject(values);
        }

        [TestMethod]
        public void BeReadOnly()
        {
            Assert.IsTrue(this.dictionary.IsReadOnly);
        }

        [TestMethod]
        public void BeFixedSize()
        {
            Assert.IsTrue(this.dictionary.IsFixedSize);
        }

        [TestMethod]
        public void BeSynchronized()
        {
            Assert.IsTrue(this.dictionary.IsSynchronized);
            object obj = this.dictionary.SyncRoot;

            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(IDictionary));
        }

        [TestMethod]
        public void HaveTheThreeDefinedKeysAndValues()
        {
            Assert.IsNotNull(this.dictionary.Keys);

            Assert.AreEqual(3, this.dictionary.Keys.Count);
            Assert.IsNotNull(this.dictionary.Values);
            Assert.AreEqual(3, this.dictionary.Values.Count);

            Assert.AreEqual(1, this.dictionary["one"]);
            Assert.AreEqual(2, this.dictionary["two"]);
            Assert.AreEqual(3, this.dictionary["three"]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfSetNewKeyValue()
        {
            this.dictionary["four"] = 4;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfResetKeyValue()
        {
            this.dictionary["three"] = 30;
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfAddKeyValue()
        {
            this.dictionary.Add("four", 4);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfRemoveKeyValue()
        {
            this.dictionary.Remove("three");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RaiseIfClear()
        {
            this.dictionary.Clear();
        }

        [TestMethod]
        public void GetTheSameObjectWithNullMetadata()
        {
            IObject iobj = this.dictionary.WithMetadata(null);

            Assert.IsNotNull(iobj);
            Assert.IsTrue(iobj == this.dictionary);
        }

        [TestMethod]
        public void GetNewDictionaryWithSameValuesUsingNotNullMetadata()
        {
            IObject iobj = this.dictionary.WithMetadata(FakePersistentMap.Instance);

            Assert.IsInstanceOfType(iobj, typeof(DictionaryObject));

            DictionaryObject newdict = (DictionaryObject)iobj;

            Assert.IsNotNull(newdict.Keys);

            Assert.AreEqual(3, newdict.Keys.Count);
            Assert.IsNotNull(newdict.Values);
            Assert.AreEqual(3, newdict.Values.Count);

            Assert.AreEqual(1, newdict["one"]);
            Assert.AreEqual(2, newdict["two"]);
            Assert.AreEqual(3, newdict["three"]);
        }

        [TestMethod]
        public void GetEnumerator()
        {
            IDictionaryEnumerator enumerator = this.dictionary.GetEnumerator();

            Assert.IsNotNull(enumerator);

            Hashtable auxdict = new Hashtable(this.dictionary);

            Assert.AreEqual(3, auxdict.Count);

            while (enumerator.MoveNext())
            {
                Assert.IsTrue(auxdict.ContainsKey(enumerator.Key));
                Assert.AreEqual(auxdict[enumerator.Key], enumerator.Value);
                auxdict.Remove(enumerator.Key);
            }

            Assert.AreEqual(0, auxdict.Count);
        }

        [TestMethod]
        public void ContainsDefinedKey()
        {
            Assert.IsTrue(this.dictionary.ContainsKey("three"));
        }

        [TestMethod]
        public void GetEntryAtDefinedKey()
        {
            DictionaryEntry entry = this.dictionary.EntryAt("three");

            Assert.AreEqual("three", entry.Key);
            Assert.AreEqual(3, entry.Value);
        }

        [TestMethod]
        public void AssociateReturnTheSameObjectIfEntryIsInOriginalObject()
        {
            IAssociative assoc = this.dictionary.Associate("three", 3);

            Assert.IsNotNull(assoc);
            Assert.IsTrue(assoc == this.dictionary);
        }

        [TestMethod]
        public void AssociateReturnNewDictionaryWithKeyRedefined()
        {
            IAssociative assoc = this.dictionary.Associate("three", 30);

            Assert.IsNotNull(assoc);
            Assert.IsFalse(assoc == this.dictionary);

            Assert.IsInstanceOfType(assoc, typeof(IDictionary));

            IDictionary dict = (IDictionary)assoc;

            Assert.IsNotNull(dict.Keys);

            Assert.AreEqual(3, dict.Keys.Count);
            Assert.IsNotNull(dict.Values);
            Assert.AreEqual(3, dict.Values.Count);

            Assert.AreEqual(1, dict["one"]);
            Assert.AreEqual(2, dict["two"]);
            Assert.AreEqual(30, dict["three"]);
        }

        [TestMethod]
        public void AssociateReturnNewDictionaryWithNewEntry()
        {
            IAssociative assoc = this.dictionary.Associate("four", 4);

            Assert.IsNotNull(assoc);
            Assert.IsFalse(assoc == this.dictionary);

            Assert.IsInstanceOfType(assoc, typeof(IDictionary));

            IDictionary dict = (IDictionary)assoc;

            Assert.IsNotNull(dict.Keys);
            Assert.AreEqual(4, dict.Keys.Count);
            Assert.IsNotNull(dict.Values);
            Assert.AreEqual(4, dict.Values.Count);

            Assert.AreEqual(1, dict["one"]);
            Assert.AreEqual(2, dict["two"]);
            Assert.AreEqual(3, dict["three"]);
            Assert.AreEqual(4, dict["four"]);
        }

        [TestMethod]
        public void AssociateLeaveOriginalObjectUnaltered()
        {
            IAssociative assoc = this.dictionary.Associate("four", 4);

            Assert.IsNotNull(this.dictionary.Keys);
            Assert.AreEqual(3, this.dictionary.Keys.Count);
            Assert.IsNotNull(this.dictionary.Values);
            Assert.AreEqual(3, this.dictionary.Values.Count);

            Assert.AreEqual(1, this.dictionary["one"]);
            Assert.AreEqual(2, this.dictionary["two"]);
            Assert.AreEqual(3, this.dictionary["three"]);
        }

        [TestMethod]
        public void RetrieveValueAt()
        {
            object result = this.dictionary.ValueAt("three");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(3, (int)result);
        }

        [TestMethod]
        public void RetrieveValueAtWithNotFoundObject()
        {
            object result = this.dictionary.ValueAt("four", 4);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(int));
            Assert.AreEqual(4, (int)result);
        }

        [TestMethod]
        public void NotBeEquivalentToNull()
        {
            Assert.IsFalse(this.dictionary.Equiv(null));
        }

        [TestMethod]
        public void NotBeEquivalentToANonIDictionaryObject()
        {
            Assert.IsFalse(this.dictionary.Equiv(1));
            Assert.IsFalse(this.dictionary.Equiv("foo"));
        }

        [TestMethod]
        public void BeEquivalentToItself()
        {
            Assert.IsTrue(this.dictionary.Equiv(this.dictionary));
        }

        [TestMethod]
        public void BeEquivalentToADictionaryWithSameEntries()
        {
            Hashtable dict = new Hashtable(this.dictionary);

            Assert.IsTrue(this.dictionary.Equiv(dict));
            Assert.IsTrue((new DictionaryObject(dict)).Equiv(this.dictionary));
        }

        [TestMethod]
        public void BeNotEquivalentToADictionaryWithSameKeysAndDifferentValues()
        {
            Hashtable dict = new Hashtable(this.dictionary);
            dict["three"] = 30;

            Assert.IsFalse(this.dictionary.Equiv(dict));
            Assert.IsFalse((new DictionaryObject(dict)).Equiv(this.dictionary));
        }

        [TestMethod]
        public void BeNotEquivalentToADictionaryWithAdditionalEntry()
        {
            Hashtable dict = new Hashtable(this.dictionary);
            dict["four"] = 4;

            Assert.IsFalse(this.dictionary.Equiv(dict));
            Assert.IsFalse((new DictionaryObject(dict)).Equiv(this.dictionary));
        }

        [TestMethod]
        public void BeNotEquivalentToAnEmptyDictionary()
        {
            Assert.IsFalse(this.dictionary.Equiv(this.dictionary.Empty));
            Assert.IsFalse(this.dictionary.Empty.Equiv(this.dictionary));
        }

        [TestMethod]
        public void BeConvertedToSequence()
        {
            ISequence sequence = this.dictionary.ToSequence();

            Assert.IsNotNull(sequence);

            object first = sequence.First();

            Assert.IsNotNull(first);
            Assert.IsInstanceOfType(first, typeof(DictionaryEntry));
            Assert.AreEqual(first, this.dictionary.EntryAt(((DictionaryEntry)first).Key));

            object second = sequence.Next().First();

            Assert.IsNotNull(second);
            Assert.IsInstanceOfType(second, typeof(DictionaryEntry));
            Assert.AreEqual(second, this.dictionary.EntryAt(((DictionaryEntry)second).Key));

            object third = sequence.Next().Next().First();

            Assert.IsNotNull(third);
            Assert.IsInstanceOfType(third, typeof(DictionaryEntry));
            Assert.AreEqual(third, this.dictionary.EntryAt(((DictionaryEntry)third).Key));

            Assert.IsTrue(first != second);
            Assert.IsTrue(first != third);

            object rest = sequence.Next().Next().Next();

            Assert.IsNull(rest);
        }

        [TestMethod]
        public void ConsWithExistingEntryShoulGetTheSameDictionary()
        {
            DictionaryEntry entry = this.dictionary.EntryAt("one");

            IPersistentCollection coll = this.dictionary.Cons(entry);

            Assert.IsNotNull(coll);
            Assert.IsTrue(coll == this.dictionary);
        }

        [TestMethod]
        public void ConsWithEquivalentDictionaryShoulGetTheSameDictionary()
        {
            IDictionary equivdict = new Hashtable(this.dictionary);

            IPersistentCollection coll = this.dictionary.Cons(equivdict);

            Assert.IsNotNull(coll);
            Assert.IsTrue(coll == this.dictionary);
        }

        [TestMethod]
        public void ConsWithNewEntryShoulNewDictionary()
        {
            DictionaryEntry entry = new DictionaryEntry();
            
            entry.Key = "four";
            entry.Value = 4;

            IPersistentCollection coll = this.dictionary.Cons(entry);

            Assert.IsNotNull(coll);
            Assert.IsTrue(coll != this.dictionary);

            Assert.AreEqual(4, coll.Count);

            Assert.IsInstanceOfType(coll, typeof(IDictionary));

            IDictionary dict = (IDictionary)coll;

            Assert.IsNotNull(dict.Keys);
            Assert.AreEqual(4, dict.Keys.Count);
            Assert.IsNotNull(dict.Values);
            Assert.AreEqual(4, dict.Values.Count);

            Assert.AreEqual(1, dict["one"]);
            Assert.AreEqual(2, dict["two"]);
            Assert.AreEqual(3, dict["three"]);
            Assert.AreEqual(4, dict["four"]);
        }
    }
}
