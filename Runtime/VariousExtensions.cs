using System;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dersei.Doto.Extensions
{
    public static class VariousExtensions
    {
        public static bool IsAbout(this float first, float second, float precisionMultiplier = 8)
        {
            return Mathf.Abs(second - first) < Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(first), Mathf.Abs(second)),
                       Mathf.Epsilon * precisionMultiplier);
        }

        public static bool IsNotZero(this float value) => Mathf.Abs(value) > Mathf.Epsilon;

        public static bool IsAboutZero(this float value) => Mathf.Abs(value) < Mathf.Epsilon;

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string ToStringInv<T>(this T @this) where T : IConvertible
        {
            return @this.ToString(CultureInfo.InvariantCulture);
        }

        public static Vector4 ToVector4(this DateTime @this)
        {
            return new Vector4(@this.Year, @this.Month, @this.Day, (float) @this.TimeOfDay.TotalSeconds);
        }

        public static T RealNull<T>(this T obj) where T : Object
        {
            return obj ? obj : null;
        }

        public static bool IsNullOrUnityNull(this object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if (!(obj is Object o)) return false;
            return o == null;
        }

        public static T IfNull<T>(this T self, T defaultValue) where T : Object
        {
            return self == null ? defaultValue : self;
        }
    }
}