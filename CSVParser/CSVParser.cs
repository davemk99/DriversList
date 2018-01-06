using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVParser
{
    /// <summary>
    /// CSVParser parses CSV formatted string to IEnumerable<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">T model type</typeparam>
    /// <code>
    /// CSVParser <PersonModel> parser=new CSVParser<PersonModel>(@""name","lastName"")
    /// </code>
    public class CSVParser<T> where T : new()
    {
        private IList<string> propertyNames;

        /// <summary>
        /// Constructs CSVParser with model description string formatted in csv.
        /// </summary>
        /// <param name="modelDescriptionCsv">Model Description string</param>
        public CSVParser(string modelDescriptionCsv)
        {
            propertyNames = ParseLine(modelDescriptionCsv).ToList();
        }

        /// <summary>
        ///Parses CSV to IEnumerable<typeparamref name="T"/>
        /// </summary>
        /// <param name="csv">CSV string without model description</param>
        /// <returns> Returns IEnumerable<typeparamref name="T"/> where T is model</returns>
        public IEnumerable<T> ParseCSV(string csv)
        {
            var lines = csv.Split('\n');
            foreach (var item in lines)
            {
                var parsed = ParseLine(item);
                var obj = Activator.CreateInstance<T>();
                for (int i = 0; i < Math.Min(propertyNames.Count, parsed.Count()); i++)
                {
                    var sanitizedPropertyName = SanitizePropertyString(propertyNames.ElementAt(i));
                    if (DoesPropertyExist(sanitizedPropertyName, typeof(T)))
                    {
                        typeof(T).GetTypeInfo().GetDeclaredProperty(sanitizedPropertyName).SetValue(obj, parsed.ElementAt(i).Trim('\"'));
                    }
                }
                yield return obj;
            }
        }

        private static bool DoesPropertyExist(string propertyName, Type type)
        {
            foreach (var item in type.GetTypeInfo().DeclaredProperties)
            {
                if (item.Name == propertyName)
                {
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<string> ParseLine(string csvLine)
        {
            var str = new StringBuilder();
            var hasValueStarted = false;
            for (int i = 0; i < csvLine.Length; i++)
            {
                if (csvLine[i] == '\"')
                {
                    if (!hasValueStarted)
                    {
                        hasValueStarted = true;
                    }
                    else
                    {
                        yield return str.ToString();
                        hasValueStarted = false;
                        str = new StringBuilder();
                    }
                    continue;
                }
                else if (csvLine[i] == ',')
                {
                    if (!hasValueStarted) continue;
                }
                str.Append(csvLine[i]);
            }
        }

        private static string SanitizePropertyString(string str)
        {
            var partiallySanitized = str.Replace("\"", "").Replace(" ", "");
            return Regex.Replace(partiallySanitized, @"(\(.*\))", "");
        }
    }
}