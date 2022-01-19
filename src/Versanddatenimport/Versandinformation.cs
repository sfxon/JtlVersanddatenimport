using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Versanddatenimport
{
    public class Versandinformation
    {
        public string Id { get; set; }
        public DateTime Versanddatum { get; set; }
        public string TrackingId { get; set; }
        public string VersandInfo { get; set; }
    }
}
