using Services.RewardRouter.Extensions;
using Services.RewardRouter.Stub;

namespace Services.RewardRouter
{
    public class RewardRouterTest
    {
        public void TestRoute()
        {
            RewardRouterBuilder builder = new RewardRouterBuilder();
            
            var router = builder.Build();

            var collect = router.Empty();
            var emit = router.Resource(ResourceType.Gold, 5);
            router.Route(new RoutingContext(new UserProfileController()), collect, emit);
        }
    }
}