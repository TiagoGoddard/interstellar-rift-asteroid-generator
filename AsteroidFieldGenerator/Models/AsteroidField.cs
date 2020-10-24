using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace AsteroidFieldGenerator.Models
{
    class AsteroidField
    {
        public List<Asteroid> asteroids { get; set; }

        public AsteroidField () {
            this.asteroids = new List<Asteroid>();
        }
    }
}
