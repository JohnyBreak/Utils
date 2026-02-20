using System;
using Services.RewardRouter.Stub;

namespace Services.RewardRouter.Units
{
    public class RewardRouterUnitStats : ICollector, IEmitter
    {
        public (EntityId entity, int amount)[]? Collect(RoutingContext ctx, EntityId entity, int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (amount > 0)
            {
                var type = (PlayerStatsType)entity.Id;
                ctx.Controller.Stats.UpdateStat(type, -amount);
            }

            return null;
        }

        public (EntityId entity, int amount)[]? Emit(RoutingContext ctx, EntityId entity, int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            if (amount > 0)
            {
                var type = (PlayerStatsType)entity.Id;
                ctx.Controller.Stats.UpdateStat(type, amount);
            }

            return null;
        }
    }
}