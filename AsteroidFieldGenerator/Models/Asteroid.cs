using AsteroidFieldGenerator.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    class Asteroid
    {
        public decimal chance { get; set; }

        public Dictionary<string, AsteroidResource> minMaxResources { get; set; }
        public Dictionary<string, AsteroidResource> minMaxRareResources { get; set; }

        public Asteroid()
        {
            this.minMaxResources = new Dictionary<string, AsteroidResource>();
            this.minMaxRareResources = new Dictionary<string, AsteroidResource>();
        }
        public Asteroid(Dictionary<string, AsteroidResource> minMaxResources, Dictionary<string, AsteroidResource> minMaxRareResources)
        {
            this.minMaxResources = minMaxResources;
            this.minMaxRareResources = minMaxRareResources;
        }
    }
}