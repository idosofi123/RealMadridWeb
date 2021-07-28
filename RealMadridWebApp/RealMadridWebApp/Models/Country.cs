﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RealMadridWebApp.Models
{
    public class Country
    {

        public int CountryID { get; set; }

        public string CountryName { get; set; }

        public string ImagePath { get; set; }

        [JsonIgnore]
        public List<Player> Players { get; set; }
    }
}
