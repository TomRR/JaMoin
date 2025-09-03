using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ja.training.csharp._02_FormatWith
{
    public static class StringExtensions
    {
        private static readonly Regex PlaceholderRegex = new Regex(@"\{([A-Za-z0-9_.]+)(?::([^}]+))?\}", RegexOptions.Compiled);
        /// <summary>
        /// interpoliert {PropName} mit den Inhalten von Properties vom übergebenen param
        /// </summary>
        /// <param name="format">das String-Template</param>
        /// <param name="param">das Objekt mit Inhalten, die ins Template interpoliert werden</param>
        /// <param name="formatPrefix">ist für Benutzer unwichtig, hilft aber bei Rekursion</param>
        /// <returns>den String format nur mit Inhalten aus param</returns>
        public static string FormatWith(this string format, object param, string formatPrefix = "")
        {
            if (string.IsNullOrEmpty(format) || param == null)
            {
                return format;
            }
            
            format = PlaceholderRegex.Replace(format, match =>
            {
                var propPath = match.Groups[1].Value;      // z.B. Trainer.FavoriteLecture.Title
                var formatString = match.Groups[2].Value; // z.B. yyyy.MM

                // object value = ResolvePropertyPath(param, propPath);
                object value = ResolvePropertyPath(param, propPath);
                if (value == null)
                {
                    return string.Empty;
                }

                // wenn ein Formatstring angegeben ist (z.B. Datumsformat)
                if (!string.IsNullOrEmpty(formatString) && value is IFormattable formattable)
                {
                    return formattable.ToString(formatString, null);
                }
                
                return value.ToString();
            });

            return format;
        }
        
        private static object ResolvePropertyPath(object obj, string path)
        {
            var current = obj;
            foreach (var part in path.Split('.'))
            {
                if (current == null) return null;
        
                var type = current.GetType();
                var prop = type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null)
                    return null;
        
                current = prop.GetValue(current);
            }
            return current;
        }
    }
}
