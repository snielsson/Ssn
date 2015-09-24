// Copyright 2015 Stig Schmidt Nielsson. All rights reserved.
using System;
using System.Reflection;
using NUnit.Framework;
using Ssn.Utils.Extensions;

namespace Ssn.Utils.Tests.Extensions {
    public class AssemblyExtensionsTests {
        [Test]
        public void VersionStringWorks() {
            Assembly sut = Assembly.GetExecutingAssembly();
            string versionString = sut.VersionString();
            Assert.AreEqual("1.0.0.0", versionString); // "1.0.0.0" is the version from the AssembleInfo of the current test assembly.
        }

        [Test]
        public void BuildTimeWorks() {
            Assembly sut = Assembly.GetExecutingAssembly();
            DateTime buildTime = sut.BuildTime();
            Assert.AreEqual(DateTime.Today, buildTime.Date); // Assumes the current test assembly is built today.
        }

        [Test]
        public void GetAssemblyInfoWorks() {
            Assembly sut = Assembly.GetExecutingAssembly();
            string info = sut.GetAssemblyInfo();
            Assert.True(info.Matches(@"Assembly: .*, version= \d+\.\d+\.\d+\.\d+, buildtime= \d\d\d\d-\d\d-\d\d \d\d:\d\d:\d\d UTC"));
        }
    }
}