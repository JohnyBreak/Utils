using System;
using System.Collections.Generic;
using Services.RewardRouter.Extensions;
using Services.RewardRouter.Units;

namespace Services.RewardRouter
{
    public class RewardRouterBuilder
    {
        private static readonly Dictionary<Type, EntityTypeCode[]> RoutersMap = new()
        {
            { typeof(RewardRouterUnitNull), new[] { EntityTypeCode.Undefined } },
            { typeof(RewardRouterUnitResources), new[] { EntityTypeCode.Resources } },
            { typeof(RewardRouterUnitStats), new[] { EntityTypeCode.Stats } }
        };

        public IRewardRouter Build()
        {
            var router = new RewardRouter();

            IRewardRouterUnit[] units = new IRewardRouterUnit[]
            {
                new RewardRouterUnitNull(),
                new RewardRouterUnitResources()
            };
            
            foreach (var unit in units)
            {
                var type = unit.GetType();

                if (!RoutersMap.TryGetValue(type, out var typeCodes))
                {
                    throw new InvalidOperationException($"RewardRouter unable to resolve routing units for '{type.Name}'");
                }

                router.RegisterRouteUnit(typeCodes, unit);
            }

            return router;
        }
    }
}