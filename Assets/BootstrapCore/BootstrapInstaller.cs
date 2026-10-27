using Bootstrap;
using Summer;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Module2 _module2;
    [SerializeField] private BootstrapStarter _bootstrapStarter;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Module1>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Module2>().FromInstance(_module2).AsSingle();
        Container.BindInterfacesAndSelfTo<Module3>().FromNew().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<Initializer>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Bootstrap.Bootstrap>().FromNew().AsSingle().NonLazy();
        Container.Inject(_bootstrapStarter);
    }
}
