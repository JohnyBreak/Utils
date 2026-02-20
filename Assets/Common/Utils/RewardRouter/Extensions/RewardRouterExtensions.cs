using Services.RewardRouter.Stub;

namespace Services.RewardRouter.Extensions
{
    public static class RewardRouterExtensions
    {
        public static void RegisterRouteUnit<T>(this RewardRouter router, EntityTypeCode[] typeCodes, T unit)
            where T : class, IRewardRouterUnit
        {
            RegisterRouteUnit(router, typeCodes, (IRewardRouterUnit)unit);
        }

        public static void RegisterRouteUnit(this RewardRouter router, EntityTypeCode[] typeCodes, IRewardRouterUnit unit)
        {
            if (unit is IEmitter emitter)
            {
                foreach (var typeCode in typeCodes)
                {
                    router.RegisterEmitter(typeCode, emitter.Emit);    
                }
            }

            if (unit is ICollector collector)
            {
                foreach (var typeCode in typeCodes)
                {
                    router.RegisterCollector(typeCode, collector.Collect);
                }
            }
        }
        
        public static (EntityId Id, int Amount) Empty(this IRewardRouter _)
            => (new EntityId(default, default), default);

        public static (EntityId Id, int Amount) Raw(this IRewardRouter _, in EntityId id, int amount)
            => (id, amount);

        public static (EntityId Id, int Amount) Resource(this IRewardRouter _, ResourceType resourceType, int amount)
            => (new EntityId(EntityTypeCode.Resources, (int)resourceType), amount);

    }
}