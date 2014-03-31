using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Parsers
{
    /// <summary>
    /// This interface is implemented by all parsers to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="T">Main class type this parser works on</typeparam>
    public interface IParser<T> where T : class
    {
        /// <summary>
        /// Parses string and returns an enumerable of objects parsed
        /// </summary>
        /// <param name="strObjects">String of objects to be parsed</param>
        /// <returns>Enumerable of all parsed objects</returns>
        IEnumerable<T> Parse(string strObjects);

        /// <summary>
        /// Validates the passed string of objects
        /// </summary>
        /// <param name="strObjects">String of objects to be validated before parsing</param>
        /// <returns>True/False</returns>
        bool Validate(string strObjects);

        /// <summary>
        /// Returns a list of errors if there are any. Call this after a failed Validate.
        /// </summary>
        /// <returns>List of all errors</returns>
        IList<string> GetErrors();
    }
}
