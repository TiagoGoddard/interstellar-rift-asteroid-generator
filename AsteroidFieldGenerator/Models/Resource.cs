using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace AsteroidFieldGenerator.Models
{
    public class Resource
    {
        protected ResourceType.Type type { get; set;  }
        protected Enum resource;
    }
}
