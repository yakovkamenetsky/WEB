﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalWebApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public ICollection<Contry> Contries { get; set; }
    }
}
