using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MTA.Core.Application.Services
{
    public interface IXmlReader
    {
        IEnumerable<XElement> GetDescendantNodes(string filePath, Predicate<XElement> predicate = null);
    }
}