using Cactus.Blade.Collection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Collection.Test.System.Collections.Generic.NameCollection
{
    [TestClass]
    public class NameCollectionTest
    {
        [TestMethod]
        public void ConstructorThrowsWhenValuesIsNull()
        {
            static void Action() => new NameCollection<Foo>(null, f => f.Name);

            Assert.ThrowsException<ArgumentNullException>(Action);
        }

        [TestMethod]
        public void ConstructorThrowsWhenGetNameIsNull()
        {
            var values = new[] { new Foo("bar") };

            void Action() => new NameCollection<Foo>(values, null);

            Assert.ThrowsException<ArgumentNullException>(Action);
        }

        [TestMethod]
        public void ConstructorThrowsWhenThereIsMoreThanOneDefaultValue()
        {
            var values = new[] { new Foo("default"), new Foo("default") };

            void Action() => new NameCollection<Foo>(values, f => f.Name);

            Assert.ThrowsException<ArgumentException>(Action, "Cannot have more than one default value.*Parameter name: values");
        }

        [TestMethod]
        public void ConstructorThrowsWhenThereIsMoreThanOneValueWithTheSameName()
        {
            var values = new[] { new Foo("bar"), new Foo("bar") };

            void Action() => new NameCollection<Foo>(values, f => f.Name);

            Assert.ThrowsException<ArgumentException>(Action, "Cannot have more than one value with the same name: bar.*Parameter name: values");
        }

        [TestMethod]
        public void ConstructorDoesNotThrowWhenThereIsMoreThanOneDefaultValueAndStrictIsFalse()
        {
            var values = new[] { new Foo("default"), new Foo("default") };

            Action action = () => new NameCollection<Foo>(values, f => f.Name, strict: false);

        }

        [TestMethod]
        public void ConstructorDoesNotThrowWhenThereIsMoreThanOneValueWithTheSameNameAndStrictIsFalse()
        {
            var values = new[] { new Foo("bar"), new Foo("bar") };

            Action action = () => new NameCollection<Foo>(values, f => f.Name, strict: false);
        }

        [TestMethod]
        public void ConstructorSetsStringComparerToOrdinalIgnoreCaseWhenNotSpecified()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.StringComparer, StringComparer.OrdinalIgnoreCase);
        }

        [TestMethod]
        public void ConstructorSetsStringComparer()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name, StringComparer.InvariantCulture);

            Assert.AreEqual(collection.StringComparer, StringComparer.InvariantCulture);
        }

        [TestMethod]
        public void ConstructorSetsDefaultNameToDefaultWhenNotSpecified()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.DefaultName, "default");
        }

        [TestMethod]
        public void ConstructorSetsDefaultNameToDefaultWhenNull()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name, defaultName: null);

            Assert.AreEqual(collection.DefaultName, "default");
        }

        [TestMethod]
        public void ConstructorSetsDefaultNameToDefaultWhenEmptyString()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name, defaultName: "");

            Assert.AreEqual(collection.DefaultName, "default");
        }

        [TestMethod]
        public void DefaultValuePropertyIsNullWhenNoValueHasADefaultName()
        {
            var values = new[] { new Foo("bar") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.IsNull(collection.DefaultName);
        }

        [TestMethod]
        public void DefaultValuePropertyIsTheValueWithANullName()
        {
            var defaultFoo = new Foo(null);
            var values = new[] { new Foo("bar"), defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.DefaultName, defaultFoo);
        }

        [TestMethod]
        public void DefaultValuePropertyIsTheValueWithAnEmptyName()
        {
            var defaultFoo = new Foo("");
            var values = new[] { new Foo("bar"), defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.DefaultValue, defaultFoo);
        }

        [TestMethod]
        public void DefaultValuePropertyIsTheValueWithANameEqualToDefaultNameProperty()
        {
            var defaultFoo = new Foo("default");
            var values = new[] { new Foo("bar"), defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.DefaultValue, defaultFoo);
        }

        [TestMethod]
        public void NamedValuesPropertyContainsTheValuesWithANonDefaultName()
        {
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { barFoo, bazFoo, new Foo("default") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.NamedValues, new[] { barFoo, bazFoo });
        }

        [TestMethod]
        public void CountPropertyReturnsTheNumberOfNamedValuesWhenDefaultValueIsNull()
        {
            var values = new[] { new Foo("bar"), new Foo("baz") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.Count, 2);
        }

        [TestMethod]
        public void CountPropertyReturnsTheNumberOfNamedValuesPlusOneWhenDefaultValueIsNotNull()
        {
            var values = new[] { new Foo("bar"), new Foo("baz"), new Foo("default") };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.Count, 2);
        }

        [TestMethod]
        public void EnumerableGetEnumeratorMethodReturnsTheDefaultValueThenTheNamedValues()
        {
            var barFoo = new Foo("bar");
            var defaultFoo = new Foo("default");

            var values = new[] { barFoo, defaultFoo };

            global::System.Collections.IEnumerable collection = new NameCollection<Foo>(values, f => f.Name);

            var enumerator = collection.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, defaultFoo);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, barFoo);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void EnumerableGetEnumeratorMethodReturnsJustTheNamedValuesIfDefaultValueIsNull()
        {
            var barFoo = new Foo("bar");

            var values = new[] { barFoo };

            global::System.Collections.IEnumerable collection = new NameCollection<Foo>(values, f => f.Name);

            var enumerator = collection.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, barFoo);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void GetEnumeratorMethodReturnsTheDefaultValueThenTheNamedValues()
        {
            var barFoo = new Foo("bar");
            var defaultFoo = new Foo("default");

            var values = new[] { barFoo, defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            using var enumerator = collection.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, defaultFoo);
            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, barFoo);
            Assert.IsFalse(enumerator.MoveNext());
        }

        [TestMethod]
        public void GetEnumeratorMethodReturnsJustTheNamedValuesIfDefaultValueIsNull()
        {
            var barFoo = new Foo("bar");

            var values = new[] { barFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            using var enumerator = collection.GetEnumerator();

            Assert.IsTrue(enumerator.MoveNext());
            Assert.AreEqual(enumerator.Current, barFoo);
            Assert.IsFalse(enumerator.MoveNext());
        }

        public void IReadOnlyDictionaryKeysPropertyReturnsTheNamesOfTheValuesOfTheCollection(string defaultValueName)
        {
            var barFoo = new Foo("bar");
            var defaultFoo = new Foo(defaultValueName);

            var values = new[] { barFoo, defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);
            IReadOnlyDictionary<string, Foo> dictionary = collection;

            Assert.AreEqual(dictionary.Keys, new[] { "default", "bar" });
        }

        [TestMethod]
        public void IReadOnlyDictionaryValuesPropertyReturnsTheNamedCollection()
        {
            var barFoo = new Foo("bar");
            var defaultFoo = new Foo("default");

            var values = new[] { barFoo, defaultFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);
            IReadOnlyDictionary<string, Foo> dictionary = collection;

            Assert.AreEqual(dictionary.Values, collection);
        }

        [TestMethod]
        public void TryGetValueMethodRetrievesANonDefaultValueWhenGivenAMatchingName()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.IsTrue(collection.TryGetValue("bar", out var value));
            Assert.AreEqual(value, barFoo);
            Assert.IsTrue(collection.TryGetValue("baz", out value));
            Assert.AreEqual(value, bazFoo);
        }

        [TestMethod]
        public void TryGetValueMethodReturnsFalseWhenGivenADefaultNameAndThereIsNoDefaultValue()
        {
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            foreach (var defaultName in new[] { "default", null, "" })
            {
                Assert.IsFalse(collection.TryGetValue(defaultName, out _));
            }
        }

        [TestMethod]
        public void TryGetValueMethodReturnsFalseWhenGivenANonDefaultNameThatIsNotFound()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.IsFalse(collection.TryGetValue("qux", out _));
        }

        [TestMethod]
        public void IndexerRetrievesANonDefaultValueWhenGivenAMatchingName()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            var value = collection["bar"];
            Assert.AreEqual(value, barFoo);

            value = collection["baz"];
            Assert.AreEqual(value, bazFoo);
        }

        [TestMethod]
        public void IndexerThrowsWhenGivenADefaultNameAndThereIsNoDefaultValue()
        {
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            foreach (var defaultName in new[] { "default", null, "" })
            {
                void Action()
                {
                    var dummy = collection[defaultName];
                }

                Assert.ThrowsException<KeyNotFoundException>(Action,
                    "The named collection does not have a default value.");
            }
        }

        [TestMethod]
        public void IndexerThrowsWhenGivenANonDefaultNameThatIsNotFound()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            void Action()
            {
                var dummy = collection["qux"];
            }

            Assert.ThrowsException<KeyNotFoundException>(Action,
                "The given name was not present in the named collection: qux.");
        }

        public void ContainsMethodReturnsTrueWhenGivenADefaultNameAndThereIsADefaultValue(string name)
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.Contains("default"), true);
            Assert.AreEqual(collection.Contains(null), true);
            Assert.AreEqual(collection.Contains(""), true);
        }

        [TestMethod]
        public void ContainsMethodReturnsTrueWhenGivenAMatchingName()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.Contains("bar"), false);
            Assert.AreEqual(collection.Contains("baz"), false);
        }

        [TestMethod]
        public void ContainsMethodReturnsFalseWhenGivenADefaultNameAndThereIsNoDefaultValue()
        {
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            foreach (var defaultName in new[] { "default", null, "" })
            {
                Assert.AreEqual(collection.Contains(defaultName), false);
            }
        }

        [TestMethod]
        public void ContainsMethodReturnsFalseWhenGivenANonDefaultNameThatIsNotFound()
        {
            var defaultFoo = new Foo("default");
            var barFoo = new Foo("bar");
            var bazFoo = new Foo("baz");
            var values = new[] { defaultFoo, barFoo, bazFoo };

            var collection = new NameCollection<Foo>(values, f => f.Name);

            Assert.AreEqual(collection.Contains("qux"), false);
        }

        [TestMethod]
        public void IsDefaultNameMethodReturnsTrueGivenNull()
        {
            var collection = new NameCollection<Foo>(Enumerable.Empty<Foo>(), f => f.Name);

            Assert.IsTrue(collection.IsDefaultName(null));
        }

        [TestMethod]
        public void IsDefaultNameMethodReturnsTrueGivenEmptyString()
        {
            var collection = new NameCollection<Foo>(Enumerable.Empty<Foo>(), f => f.Name);

            Assert.IsTrue(collection.IsDefaultName(""));
        }

        [TestMethod]
        public void IsDefaultNameMethodReturnsTrueGivenDefaultName()
        {
            var collection = new NameCollection<Foo>(Enumerable.Empty<Foo>(), f => f.Name);

            Assert.IsTrue(collection.IsDefaultName("default"));
        }

        [TestMethod]
        public void IsDefaultNameMethodReturnsFalseGivenAnyOtherString()
        {
            var collection = new NameCollection<Foo>(Enumerable.Empty<Foo>(), f => f.Name);

            Assert.IsFalse(collection.IsDefaultName("literally anything else"));
        }

        public class Foo
        {
            public Foo(string name) => Name = name;
            public string Name { get; }
        }
    }
}
