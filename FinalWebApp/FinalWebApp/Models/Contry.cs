using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
    public class Contry
    {
        public int Id { get; set; }
        public string       Name  { get; set; }

        public ICollection<City> cities { get; set; }
    }
}
