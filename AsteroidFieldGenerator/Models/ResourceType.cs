using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    public class ResourceType
    {
        public enum Type
        {
            Common = 1,
            Rare = 2
        }

        public enum CommonResource
        {
            RT_IronOre = 1,
            RT_CopperOre = 2,
            RT_Water = 3,
            RT_SiliconOre = 4,
            RT_ZincOre = 5,
            RT_Nitrogen = 6,
            RT_Carbon = 18
        }
        public enum RareResource
        {
            RT_TungstenOre = 93,
            RT_UraniumOre = 57,
            RT_AluminiumOre = 59,
            RT_LithiumOre = 99,
            RT_LeadOre = 56,

            RT_Gold = 24,
            RT_Silver = 27,
            RT_Osmium = 25,
            RT_Iridium = 26,
            RT_Mercury = 28,
            RT_Platinum = 29,
            RT_Fluorite = 61,
            RT_Beryllium = 41,
            RT_Quartz = 77,
            RT_Manganese = 101,
            RT_Titanium = 102
        }
    }
}
