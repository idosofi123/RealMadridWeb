
using RealMadridWebApp.Models;
using System.Collections.Generic;

namespace RealMadridWebApp
{

    public struct PositionGroup
    {

        public Position Key { get; set; }

        public List<Player> Players { get; set; }

        public int count { get; set; }
    }
}
