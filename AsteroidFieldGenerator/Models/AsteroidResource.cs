using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AsteroidFieldGenerator.Models
{
    class AsteroidResource<T> where T : Resource
    {
        public Resource resource { get; set; }
        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public AsteroidResource(T resource, int minAmount, int maxAmount)
        {
            this.resource = resource;
            this.minAmount = minAmount;
            this.maxAmount = maxAmount;
        }
    }

}
