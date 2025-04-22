﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SpellWork.Extensions;

public static class Extensions
{
    /// <summary>
    /// Reverses a string
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Reverse(this string s)
    {
        return new string(s.ToCharArray().Reverse().ToArray());
    }

    public static StringBuilder AppendFormatIfNotNull(this StringBuilder builder, string format, params object[] arg)
    {
        return arg[0].ToUInt32() != 0 ? builder.AppendFormat(format, arg) : builder;
    }

    // Append Format Line
    public static StringBuilder AppendFormatLine(this StringBuilder builder, string format, params object[] arg0)
    {
        return builder.AppendFormat(format, arg0).AppendLine();
    }

    public static StringBuilder AppendFormatLineIfNotNull(this StringBuilder builder, string format, int arg)
    {
        return arg != 0 ? builder.AppendFormat(format, arg).AppendLine() : builder;
    }

    public static StringBuilder AppendFormatLineIfNotNull(this StringBuilder builder, string format, uint arg)
    {
        return arg != 0 ? builder.AppendFormat(format, arg).AppendLine() : builder;
    }

    public static StringBuilder AppendFormatLineIfNotNull(this StringBuilder builder, string format, float arg)
    {
        return arg != 0 ? builder.AppendFormat(format, arg).AppendLine() : builder;
    }

    public static StringBuilder AppendFormatLineIfNotNull(this StringBuilder builder, string format, string arg)
    {
        if (!string.IsNullOrEmpty(arg))
        {
            return builder.AppendFormat(format, arg).AppendLine();
        }

        return builder;
    }

    public static uint ToUInt32(this object val)
    {
        if (val == null)
            return 0;
        var valStr = val.ToString();

        uint num;
        if (valStr.StartsWith("0x"))
            uint.TryParse(valStr.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
        else
            uint.TryParse(valStr, out num);
        return num;
    }

    public static int ToInt32(this object val)
    {
        if (val == null)
            return 0;
        var valStr = val.ToString();

        int num;
        if (valStr.StartsWith("0x"))
            int.TryParse(valStr.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
        else
            int.TryParse(valStr, out num);
        return num;
    }

    public static float ToFloat(this object val)
    {
        if (val == null)
            return 0.0f;

        float num;
        float.TryParse(val.ToString().Replace(',', '.'), out num);
        return num;
    }

    public static ulong ToUlong(this object val)
    {
        if (val == null)
            return 0U;
        var valStr = val.ToString();

        ulong num;
        if (valStr.StartsWith("0x"))
            ulong.TryParse(valStr.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num);
        else
            ulong.TryParse(valStr, out num);
        ulong.TryParse(val.ToString(), out num);
        return num;
    }

    public static string NormalizeString(this string text, string remove = null)
    {
        var str = string.Empty;
        if (!string.IsNullOrEmpty(remove))
            text = text.Replace(remove, string.Empty);

        foreach (var s in text.Split('_'))
        {
            var i = 0;
            foreach (var c in s.ToCharArray())
            {
                str += i == 0 ? c.ToString().ToUpper() : c.ToString().ToLower();
                i++;
            }

            str += " ";
        }

        return str.Remove(str.Length - 1);
    }

    /// <summary>
    /// Compares the text on the partial occurrence of a string and ignore case
    /// </summary>
    /// <param name="text">The original text, which will search entry</param>
    /// <param name="compareText">String which will be matched with the original text</param>
    /// <returns>Boolean(true or false)</returns>
    public static bool ContainsText(this string text, string compareText)
    {
        return text.ToUpper().IndexOf(compareText.ToUpper(), StringComparison.CurrentCultureIgnoreCase) != -1;
    }

    /// <summary>
    /// Compares the text on the partial occurrence of a string and ignore case
    /// </summary>
    /// <param name="text">The original text, which will search entry</param>
    /// <param name="compareText">Array strings which will be matched with the original text</param>
    /// <returns>Boolean(true or false)</returns>
    public static bool ContainsText(this string text, string[] compareText)
    {
        return compareText.Any(str => text.IndexOf(str, StringComparison.CurrentCultureIgnoreCase) != -1);
    }

    public static bool ContainsElement(this uint[] array, uint[] value)
    {
        return array.Length == value.Length && array.Where((t, i) => (t & value[i]) != 0).Any();
    }

    /// <summary>
    /// Checks if the specified value in a given array
    /// </summary>
    /// <param name="array">Array in which to search</param>
    /// <param name="value">Meaning Search</param>
    /// <returns>true or false</returns>
    public static bool ContainsElement<T>(this T[] array, T value) where T : IComparable
    {
        return array.Any(i => i.Equals(value));
    }

    public static T GetValue<T>(this Dictionary<uint, T> dictionary, uint key)
    {
        T value;
        dictionary.TryGetValue(key, out value);
        return value;
    }

    public static string GetFullName(this Enum @enum)
    {
        var field = @enum.GetType().GetField(@enum.ToString());
        var attrs = (FullNameAttribute[])field?.GetCustomAttributes(typeof(FullNameAttribute), false);

        return attrs?.Length > 0 ? attrs[0].FullName : @enum.ToString();
    }

    public static bool IsEmpty(this String str)
    {
        return str == String.Empty;
    }
}
