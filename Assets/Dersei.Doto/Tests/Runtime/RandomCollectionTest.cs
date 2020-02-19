using System;
using System.Globalization;
using Dersei.Doto.Collections;
using Dersei.Doto.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace Dersei.Doto.Tests.Runtime
{
    internal class RandomCollectionTest
    {
        [Test]
        public void CountsTest()
        {
            var rCollection = new RandomCollection<string>(("Test", 1), ("Value", 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
        }

        [Test]
        public void AddTest()
        {
            var rCollection = new RandomCollection<string>(("Test", 1), ("Value", 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            rCollection.Add("NewValue", 10);
            Assert.AreEqual(3, rCollection.UniqueCount);
            Assert.AreEqual(16, rCollection.Count);
        }

        [Test]
        public void RemoveTest()
        {
            var rCollection = new RandomCollection<string>(("Test", 1), ("Value", 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            rCollection.Remove("Test", 1);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            rCollection.Remove("Value", 4);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(1, rCollection.Count);
        }

        [Test]
        public void GetRandomTest()
        {
            var rCollection = new RandomCollection<string>(("Test", 1), ("Value", 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            var result = rCollection.GetRandom();
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            Assert.That(result == "Test" || result == "Value");
        }
        
        [Test]
        public void GetRandomLoopTest()
        {
            var rCollection = new RandomCollection<string>(("Test", 1), ("Value", 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            while (rCollection.Count > 0)
            {
                rCollection.GetRandom();
            }
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(0, rCollection.Count); 
        }

        private class TestValue
        {
            public readonly string Value;
            public TestValue(string value) => Value = value;
        }

        private struct TestStructValue
        {
            public readonly int Value;
            public TestStructValue(int value) => Value = value;
        }
        
        [Test]
        public void RemoveCustomClassTest()
        {
            var item1 = new TestValue("Test");
            var item2 = new TestValue("Value");
            var rCollection = new RandomCollection<TestValue>((item1, 1), (item2, 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            rCollection.Remove(item1, 1);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            rCollection.Remove(item2, 4);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(1, rCollection.Count);
        }
        
        [Test]
        public void GetRandomCustomClassTest()
        {
            var rCollection = new RandomCollection<TestValue>((new TestValue("Test"), 1), (new TestValue("Value"), 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            var result = rCollection.GetRandom();
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            Assert.That(result.Value == "Test" || result.Value == "Value");
        }
        
        [Test]
        public void RemoveCustomStructTest()
        {
            var item1 = new TestStructValue(1);
            var item2 = new TestStructValue(234);
            var rCollection = new RandomCollection<TestStructValue>((item1, 1), (item2, 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            rCollection.Remove(item1, 1);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            rCollection.Remove(item2, 4);
            Assert.AreEqual(1, rCollection.UniqueCount);
            Assert.AreEqual(1, rCollection.Count);
        }
        
        [Test]
        public void GetRandomCustomStructTest()
        {
            var rCollection = new RandomCollection<TestStructValue>((new TestStructValue(1), 1), (new TestStructValue(234), 5));
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(6, rCollection.Count);
            var result = rCollection.GetRandom();
            Assert.AreEqual(2, rCollection.UniqueCount);
            Assert.AreEqual(5, rCollection.Count);
            Assert.That(result.Value == 1 || result.Value == 234);
        }
    }
}