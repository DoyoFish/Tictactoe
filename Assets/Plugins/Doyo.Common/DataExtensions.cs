using System;
using System.Collections.Generic;
using System.Linq;

namespace Doyo.Common
{
    public static class DataExtensions
    {
        public static byte ParseByte(this string source, byte defaultValue = default(byte))
        {
            byte value;
            if (!byte.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static short ParseShort(this string source, short defaultValue = default(short))
        {
            short value;
            if (!short.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static ushort ParseUShort(this string source, ushort defaultValue = default(ushort))
        {
            ushort value;
            if (!ushort.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static int ParseInt(this string source, int defaultValue = default(int))
        {
            int value;
            if (!int.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static uint ParseUInt(this string source, uint defaultValue = default(uint))
        {
            uint value;
            if (!uint.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static long ParseLong(this string source, long defaultValue = default(long))
        {
            long value;
            if (!long.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static ulong ParseUlong(this string source, ulong defaultValue = default(ulong))
        {
            ulong value;
            if (!ulong.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static float ParseFloat(this string source, float defaultValue = default(float))
        {
            float value;
            if (!float.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static double ParseDouble(this string source, double defaultValue = default(double))
        {
            double value;
            if (!double.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static DateTime ParseDateTime(this string source, DateTime defaultValue = default(DateTime))
        {
            DateTime value;
            if (!DateTime.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static bool ParseBool(this string source, bool defaultValue = default(bool))
        {
            bool value;
            if (!bool.TryParse(source, out value))
            {
                return defaultValue;
            }
            return value;
        }

        public static IEnumerable<T> TryConveterArr<T>(this IEnumerable<string> sources, Func<string, T> converter)
        {
            IEnumerable<T> arr = sources.Select(s => converter.Invoke(s));
            return arr;
        }

        public static bool HasTag(this int data, int digital)
        {
            bool hasTag = ((data >> digital - 1) & 1) == 1;
            return hasTag;
        }

        public static bool HasTag(this byte data, int digital)
        {
            bool hasTag = ((data >> digital - 1) & 1) == 1;
            return hasTag;
        }
    }
}
