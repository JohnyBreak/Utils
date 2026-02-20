using System;

namespace Services.RewardRouter.Units
{
    public class RewardRouterUnitNull : ICollector, IEmitter
    {
        public (EntityId entity, int amount)[] Collect(RoutingContext ctx, EntityId entity, int amount)
        {
            return Array.Empty<(EntityId entity, int amount)>();
        }

        public (EntityId entity, int amount)[] Emit(RoutingContext ctx, EntityId entity, int amount)
        {
            return Array.Empty<(EntityId entity, int amount)>();
        }
    }
}