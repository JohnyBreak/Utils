using AppInit.InitSteps;
using Zenject;

namespace Source.Scripts.AppInit
{
    public class StartupInitStepsInstaller : Installer<StartupInitStepsInstaller>
    {
        public override void InstallBindings()
        {
            // ============ Initialization steps ============ //
            
            // Container
            //     .BindInterfacesAndSelfTo<LoadingInitStep>()
            //     .AsSingle()
            //     .Lazy();
            
            // ============================================== //
            
            Container
                .BindInterfacesAndSelfTo<SceneReadyNotifier>()
                .AsSingle()
                .WithArguments(InitStepGroup.StartScene)
                .NonLazy();

            Container.BindExecutionOrder<SceneReadyNotifier>(int.MaxValue);
        }
    }
}
