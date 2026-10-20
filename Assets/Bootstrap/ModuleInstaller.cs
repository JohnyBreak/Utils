using Summer;
using UnityEngine;
using Zenject;

public class ModuleInstaller : MonoInstaller
{
    [SerializeField] private Module2 _module2;
    [SerializeField] private Bootstrap _bootstrap;
    
    public override void InstallBindings()
    {
        Container.Bind<Module1>().FromNew().AsSingle().NonLazy();
        Container.Bind<Module2>().FromInstance(_module2).AsSingle();
        Container.Bind<Module3>().FromNew().AsSingle().NonLazy();
        
        Container.Inject(_bootstrap);
    }
}
