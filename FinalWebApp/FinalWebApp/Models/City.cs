using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public Contry contry { get; set; }
        public int ContryId { get; set; }
        public ICollection <Hotel> Hotels{ get; set; }
    }
}
