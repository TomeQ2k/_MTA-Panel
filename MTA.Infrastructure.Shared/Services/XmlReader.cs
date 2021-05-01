using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MTA.Core.Application.Services;

namespace MTA.Infrastructure.Shared.Services
{
    public class XmlReader : IXmlReader
    {
        public IEnumerable<XElement> GetDescendantNodes(string filePath, Predicate<XElement> predicate = null)
        {
            XDocument xmlFile = XDocument.Load(filePath);

            return predicate == null
                ? from node in xmlFile.DescendantNodes()
                select (XElement) node
                : from node in xmlFile.DescendantNodes()
                where predicate((XElement) node)
                select (XElement) node;
        }
    }
}