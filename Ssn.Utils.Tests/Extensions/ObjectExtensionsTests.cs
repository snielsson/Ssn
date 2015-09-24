// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System.IO;
using System.Text;
using Ssn.Utils.Extensions;
using NUnit.Framework;
using Ssn.TestUtils;
namespace Ssn.Utils.Tests.Extensions {
    //TODO: use Ssn.TestUtils from a nuget package instead of direct reference.
    public class ObjectExtensionsTests : TestBase {
        private class A {
            public B B { get; set; }
            public C C { get; set; }
        }

        private class B {
            public B() {
                NestedInBProp = new NestedInB();
            }
            public NestedInB NestedInBProp { get; set; }

            public class NestedInB {
                public string Name { get; set; }
            }
        }

        private class C {
            public A A { get; set; }
        }

        private class DerivedFromA : A {
            public string Name { get; set; }
        }

        private struct SomeStruct {
            private string _name;
            public SomeStruct(string name) {
                _name = name;
            }
            public string Name { get { return _name; } private set { _name = value; } }
        }

        private class SomeClass {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private DerivedFromA CreateA() {
            var a = new DerivedFromA();
            a.Name = Create<string>();
            a.B = new B {
                NestedInBProp = {
                    Name = Create<string>()
                }
            };
            a.C = new C();
            return a;
        }

        [Test]
        public void CloneWorks() {
            var someObj = new SomeClass {
                Id = 1,
                Name = "bla"
            };
            SomeClass someObjClone = someObj.Clone();
            Assert.False(someObj.Equals(someObjClone));
            Assert.False(ReferenceEquals(someObj, someObjClone));

            DerivedFromA a = CreateA();
            DerivedFromA aClone = a.Clone();
            Assert.False(ReferenceEquals(a, aClone));
            string aJson = a.ToPrettyJson();
            string aCloneJson = aClone.ToPrettyJson();
            Assert.AreEqual(aJson, aCloneJson);
        }

        [Test]
        public void ToJsonWorks() {
            DerivedFromA a = CreateA();
            string json = a.ToJson();
            Assert.True(json.Contains(a.Name));
            Assert.True(json.Contains(a.B.NestedInBProp.Name));
        }
        [Test]
        public void ToPrettyJsonWorks() {
            DerivedFromA a = CreateA();
            string json = a.ToPrettyJson();
            Assert.True(json.Contains(a.Name));
            Assert.True(json.Contains(a.B.NestedInBProp.Name));
        }
        [Test]
        public void DumpWorks() {
            var sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);
            DerivedFromA a = CreateA();
            a.Dump(textWriter);
            string str = sb.ToString();
            Assert.True(str.Contains(a.Name));
        }
    }
}