using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    public class CommonResource : Resource
    {
        public ResourceType.CommonResource commonResource;

        public CommonResource(ResourceType.CommonResource resource)
        {
            this.type = ResourceType.Type.Common;
            this.resource = resource;
            this.commonResource = resource;
        }
    }
}
