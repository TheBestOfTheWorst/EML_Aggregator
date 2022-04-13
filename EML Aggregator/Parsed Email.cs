using System;

namespace EML_Aggregator
{
    internal class ParsedEmail
    {
        public String From { get; set; }
        public String To { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
    }
}