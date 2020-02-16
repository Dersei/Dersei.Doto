using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Dersei.Doto.Extensions;
using NUnit.Framework;

namespace Dersei.Doto.Tests.Runtime
{
    internal class RandomExtensionsTest
    {
        [Test]
        public void RandomItemListTest()
        {
            var testList = Enumerable.Range(0, 1).ToList();
            Assert.Contains(testList.RandomItem(), testList);
            Assert.DoesNotThrow(() => testList.RandomItem());
        }
        
        [Test]
        public void RandomItemArrayTest()
        {
            var testList = Enumerable.Range(0, 1).ToArray();
            Assert.Contains(testList.RandomItem(), testList);
            Assert.DoesNotThrow(() => testList.RandomItem());
        }
        
        [Test]
        public void RandomItemDictionaryTest()
        {
            var testList = new Dictionary<int, string>
            {
                [1] = "1"
            };
            Assert.Contains(testList.RandomItem(), testList.Values);
            Assert.DoesNotThrow(() => testList.RandomItem());
        }

        [Test]
        public void RandomFloatIsInclusive()
        {
            var value = new System.Random().NextFloat(1, 1);
            Assert.That(1 <= value && value <= 1, value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
