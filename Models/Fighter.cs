using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationFramework.Models
{
    public class Fighter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }

        public ICollection<Fight> Fights { get; set; }
    }
}