using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    public class RareResource : Resource
    {
        public ResourceType.RareResource rareResource;

        public RareResource(ResourceType.RareResource resource)
        {
            this.type = ResourceType.Type.Common;
            this.resource = resource;
            this.rareResource = resource;
        }
    }
}
