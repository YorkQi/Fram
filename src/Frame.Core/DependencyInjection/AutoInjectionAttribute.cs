﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Frame.AspNetCore.DependencyInjection
{
    public class AutoInjectionAttribute : Attribute
    {
        public AutoInjectionAttribute(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }

        public ServiceLifetime Lifetime { get; set; }
    }
}
