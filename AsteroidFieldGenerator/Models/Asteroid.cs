using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    class Asteroid
    {
        public double chance { get; set; }

        List<AsteroidResource<CommonResource>> minMaxResources { get; set; }
        List<AsteroidResource<RareResource>> minMaxRareResources { get; set; }

        public Asteroid()
        {
            minMaxResources = new List<AsteroidResource<CommonResource>>();
            minMaxRareResources = new List<AsteroidResource<RareResource>>();
        }
    }
}