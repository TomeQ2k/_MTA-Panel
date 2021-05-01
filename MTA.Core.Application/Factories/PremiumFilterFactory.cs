using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Filters;

namespace MTA.Core.Application.Factories
{
    public class PremiumFilterFactory : Attribute, IFilterFactory
    {
        public int Cost { get; init; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var premiumFilter = serviceProvider.GetService<PremiumFilter>();

            premiumFilter.Cost = Cost;

            return premiumFilter;
        }
    }
}