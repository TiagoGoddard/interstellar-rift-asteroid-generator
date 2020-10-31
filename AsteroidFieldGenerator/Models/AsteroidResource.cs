using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AsteroidFieldGenerator.Models
{
    class AsteroidResource
    {
        public int minAmount { get; set; }
        public int maxAmount { get; set; }

        public AsteroidResource()
        {
            minAmount = 0;
            maxAmount = 1;
        }

        public AsteroidResource(int minAmount, int maxAmount)
        {
            this.minAmount = minAmount;
            this.maxAmount = maxAmount; 
        }
    }

}
