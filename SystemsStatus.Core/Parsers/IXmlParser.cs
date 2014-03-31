using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemsStatus.Core.Parsers
{
    public interface IXmlParser<T> : IParser<T> where T : class
    {

    }
}
