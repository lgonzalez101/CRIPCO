using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Calendario
{
    public class CalendarioViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public string icon { get; set; }
        public string[] className { get; set; }
        public string description { get; set; }
    }
}