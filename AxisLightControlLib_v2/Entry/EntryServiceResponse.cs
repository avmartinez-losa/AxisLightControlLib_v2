using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace AxisLightControlLib_v2.Entry
{
    public class EntryServiceResponse
    {

        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Content { get; set; }
        public XElement SOAPContent { get; set; }
    }
}
