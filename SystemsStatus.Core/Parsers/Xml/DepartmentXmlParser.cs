using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemsStatus.Core.Data.Entities;
using System.Xml;
using System.Xml.Linq;
using Castle.Core.Logging;
using System.Xml.Schema;
using System.IO;
using System.Reflection;

namespace SystemsStatus.Core.Parsers.Xml
{
    public class DepartmentXmlParser : IXmlParser<Department>
    {
        //private readonly string _schemaFile = "Departments.xsd";
        private readonly Stream _schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SystemsStatus.Core.Parsers.Xml.Schemas.Departments.xsd");

        public IList<string> Errors { get; set; }
        public ILogger Logger { get; set; }

        public DepartmentXmlParser()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Parses string and returns an enumerable of departments parsed
        /// </summary>
        /// <param name="strObjects">XML string of departments to be parsed</param>
        /// <returns>Enumerable of all departments parsed</returns>
        public IEnumerable<Department> Parse(string strObjects)
        {
            XDocument xdoc = XDocument.Parse(strObjects);
            IEnumerable<Department> departments = new List<Department>();

            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat("Parsing XML for Departments: {0}", strObjects);
                }

                XNamespace ns = "http://cortland.edu/Departments.xsd";

                departments = from entry in xdoc.Descendants(ns + "Department")
                                  select new Department()
                                  {
                                      Name = entry.Element(ns + "Name").Value,
                                      Code = entry.Element(ns + "Code").Value
                                  };

                if (Logger.IsDebugEnabled)
                {
                    Logger.DebugFormat("Departments found in XML: {0}", departments.Count());

                    foreach (var department in departments)
                    {
                        Logger.Debug("Parsing department:");
                        Logger.DebugFormat("Name: {0}", department.Name);
                        Logger.DebugFormat("Code: {0}", department.Code);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
            }

            return departments.ToList();
        }

        /// <summary>
        /// Validates the passed string of objects
        /// </summary>
        /// <param name="strObjects">String of objects to be validated before parsing</param>
        /// <returns>True/False</returns>
        public bool Validate(string strObjects)
        {
            try
            {
                XmlSchema schema = XmlSchema.Read(_schemaStream, null);
                XmlSchemaSet schemas = new XmlSchemaSet();
                schemas.Add(schema);

                Logger.Debug("Validating XML...");

                XDocument xdoc = XDocument.Parse(strObjects);

                xdoc.Validate(schemas, (o, e) =>
                    {
                        this.Errors.Add(e.Message);
                        Logger.Warn(e.Message);
                    });

                Logger.Debug("XML validation complete");

                if (this.Errors.Count > 0)
                {
                    Logger.Debug("XML validation failed");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Debug("XML validation complete");
                Logger.Debug("XML validation failed");
                Logger.Warn(ex.Message);
                this.Errors.Add(ex.Message);
                return false;
            }

            Logger.Debug("XML validation passed!");

            return true;
        }

        /// <summary>
        /// Returns a list of errors if there are any. Call this after a failed Validate.
        /// </summary>
        /// <returns>List of all errors</returns>
        public IList<string> GetErrors()
        {
            return this.Errors;
        }
    }
}
