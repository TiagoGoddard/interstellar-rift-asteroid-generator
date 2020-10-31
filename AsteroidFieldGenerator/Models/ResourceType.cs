using System;
using System.Collections.Generic;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    public class ResourceType
    {
        public enum Type
        {
            Common,
            Rare
        }

        public enum CommonResource
        {
            RT_IronOre,
            RT_CopperOre,
            RT_Water,
            RT_SiliconOre,
            RT_ZincOre,
            RT_Nitrogen,
            RT_Carbon
        }
        public enum RareResource
        {
            RT_TungstenOre,
            RT_UraniumOre,
            RT_AluminiumOre,
            RT_LithiumOre,
            RT_LeadOre,

            RT_Gold,
            RT_Silver,
            RT_Osmium,
            RT_Iridium,
            RT_Mercury,
            RT_Platinum,
            RT_Fluorite,
            RT_Beryllium,
            RT_Quartz,
            RT_Manganese,
            RT_Titanium
        }
    }
}
