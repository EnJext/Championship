using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFramework.Models
{
    public class FightPageViewModel
    {
        public int FighterId { get; set;}
        public int FightId { get; set; }
        public DateTime Time { get; set; }
        public int Rounds { get; set; }
        public string Winner { get; set; }
        public string Losser { get; set; }

        public int Judge1 { get; set; }
        public int Judge2 { get; set; }
        public int Judge3 { get; set; }
    }
}