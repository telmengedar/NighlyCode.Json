using System;
using System.Collections.Generic;
using System.Reflection;
using NightlyCode.Json.Models;

namespace NightlyCode.Json.Extensions {
    
    /// <summary>
    /// extension methods for dictionaries
    /// </summary>
    public static class DictionaryExtensions {

        /// <summary>
        /// reads a type from dictionary values
        /// </summary>
        /// <param name="dictionary">dictionary providing values</param>
        /// <param name="type">type to read</param>
        /// <returns>instantiated type filled with values from dictionary</returns>
        public static object ReadType(this IDictionary<string, object> dictionary, Type type) {
            object host = Activator.CreateInstance(type);
            Model typemodel = Model.Get(type);
            foreach (KeyValuePair<string, object> kvp in dictionary) {
                PropertyInfo property = typemodel.GetProperty(kvp.Key);
                if (property == null)
                    continue;
                if (property.PropertyType.IsArray)
                    property.SetValue(host, kvp.Value.ReadValueAsArray(property.PropertyType.GetElementType()));
                else property.SetValue(host, kvp.Value.ReadStructureValue(property.PropertyType));
            }

            return host;
        }
    }
}