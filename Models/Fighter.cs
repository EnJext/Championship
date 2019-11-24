using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationFramework.Models
{
    public class Fighter
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public int Age { get; set;}
        public float Weight { get; set;}
        public ICollection<Fight> WinFights { get; set;}
        public ICollection<Fight> LoseFights { get; set;}

        [NotMapped]
        public ICollection<Fight> Fights => WinFights;
    }
}