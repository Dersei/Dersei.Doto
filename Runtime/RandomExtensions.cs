using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dersei.Doto.Extensions
{
    public static class RandomExtensions
    {
        public static T RandomItem<T>(this List<T> @this)
        {
            var index = Random.Range(0, @this.Count);
            return @this[index];
        }

        public static T RandomItem<T>(this List<T> @this, System.Random random)
        {
            if (@this.Count == 0)
                return default;
            var index = random.Next(0, @this.Count);
            return @this[index];
        }

        public static T RandomItem<T>(this T[] @this)
        {
            if (@this.Length == 0)
                return default;
            var index = Random.Range(0, @this.Length);
            return @this[index];
        }

        public static T RandomItem<T>(this T[] @this, System.Random random)
        {
            if (@this.Length == 0)
                return default;
            var index = random.Next(0, @this.Length);
            return @this[index];
        }

        public static T RandomItem<T>(this IEnumerable<T> @this)
        {
            if (@this is List<T> list)
            {
                return list.RandomItem();
            }

            var enumerable = @this.ToList();
            var index = Random.Range(0, enumerable.Count);
            return enumerable.ElementAt(index);
        }
        
        public static T RandomItem<T>(this IEnumerable<T> @this, System.Random random)
        {
            if (@this is List<T> list)
            {
                return list.RandomItem();
            }

            var enumerable = @this.ToList();
            var index = random.Next(0, enumerable.Count);
            return enumerable.ElementAt(index);
        }

        public static T2 RandomItem<T1, T2>(this Dictionary<T1, T2> @this)
        {
            var index = Random.Range(0, @this.Values.Count);
            return @this.Values.ElementAt(index);
        }
        
        public static T2 RandomItem<T1, T2>(this Dictionary<T1, T2> @this, System.Random random)
        {
            var index = random.Next(0, @this.Values.Count);
            return @this.Values.ElementAt(index);
        }

        public static float NextFloat(this System.Random @this, float minValue, float maxValue)
        {
            var value = (float) @this.NextDouble();
            return value * (maxValue - minValue) + minValue;
        }

        public static Vector3 NextVector3(this Random _, Vector3 minValues, Vector3 maxValues)
        {
            var x = Random.Range(minValues.x, maxValues.x);
            var y = Random.Range(minValues.y, maxValues.y);
            var z = Random.Range(minValues.z, maxValues.z);
            return new Vector3(x, y, z);
        }

        public static Vector3 NextVector3(this System.Random @this, Vector3 minValues, Vector3 maxValues)
        {
            var x = (float) @this.NextDouble() * (maxValues.x - minValues.x) + minValues.x;
            var y = (float) @this.NextDouble() * (maxValues.y - minValues.y) + minValues.y;
            var z = (float) @this.NextDouble() * (maxValues.z - minValues.z) + minValues.z;
            return new Vector3(x, y, z);
        }
    }
}