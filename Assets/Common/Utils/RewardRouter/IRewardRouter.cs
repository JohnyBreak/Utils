using Services.RewardRouter.Stub;

namespace Services.RewardRouter
{
    public readonly struct RoutingContext
    {
        public readonly IUserProfileController Controller;

        public RoutingContext(IUserProfileController controller)
        {
            Controller = controller;
        }
    }
    
    public interface IRewardRouterUnit { }

    public interface ICollector : IRewardRouterUnit
    {
        (EntityId entity, int amount)[]? Collect(RoutingContext ctx, EntityId entity, int amount);
    }

    public interface IEmitter : IRewardRouterUnit
    {
        (EntityId entity, int amount)[]? Emit(RoutingContext ctx, EntityId entity, int amount);
    }
    
    public interface IRewardRouter
    {
        public void Route(RoutingContext ctx, (EntityId entity, int amount) collect, (EntityId entity, int amount) emit);
    }
}

