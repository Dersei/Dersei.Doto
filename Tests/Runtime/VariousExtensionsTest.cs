using System;
using System.Globalization;
using Dersei.Doto.Extensions;
using NUnit.Framework;
using UnityEngine;

namespace Dersei.Doto.Tests.Runtime
{
    internal class VariousExtensionsTest
    {
        [Test]
        public void CultureInvariantTest()
        {
            const float value = 0.001f;
            Assert.AreEqual(value.ToStringInv(), value.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void IsAboutZeroTest()
        {
            var valueLess = Mathf.Epsilon / 2f;
            var valueMore = Mathf.Epsilon * 2f;
            Assert.True(valueLess.IsAboutZero());
            Assert.False(valueMore.IsAboutZero());
        }

        [Test]
        public void IsNotZeroTest()
        {
            var valueLess = Mathf.Epsilon / 2f;
            var valueMore = Mathf.Epsilon * 2f;
            Assert.False(valueLess.IsNotZero());
            Assert.True(valueMore.IsNotZero());
        }

        [Test]
        public void IsAboutTest()
        {
            const float valueLess = 0f;
            const float valueMore = 1E-10f;
            Assert.True(valueLess.IsAbout(0));
            Assert.False(valueMore.IsAbout(0));
        }

        [Test]
        public void ContainsTest()
        {
            const string value = "Give a man a fire and he's warm for a day, but set fire to him and he's warm for the rest of his life.";
            const string toCheck = "fire";
            const string toCheckFail = "FIRE";
            Assert.True(value.Contains(toCheck));
            Assert.False(value.Contains(toCheckFail));
        }
        
        [Test]
        public void ContainsIgnoreCaseTest()
        {
            const string value = "Give a man a fire and he's warm for a day, but set fire to him and he's warm for the rest of his life.";
            const string toCheck = "fire";
            const string toCheck2 = "FIRE";
            Assert.True(value.Contains(toCheck));
            Assert.True(value.Contains(toCheck2, StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void UnityNullTest()
        {
            var unityObject = new UnityEngine.Object();
            Assert.False(ReferenceEquals(unityObject, null));
            Assert.True(unityObject.IsNullOrUnityNull());
            Assert.Throws<NullReferenceException>(() => _ = unityObject.name);
        }
        
        [Test]
        public void UnityNullOperatorTest()
        {
            var unityObject = new UnityEngine.Object();
            Assert.Throws<NullReferenceException>(() => _ = unityObject?.name);
            Assert.DoesNotThrow(() => _ = unityObject.RealNull()?.name);
            unityObject = null;
            Assert.DoesNotThrow(() => _ = unityObject.RealNull()?.name);
        }
    }
}