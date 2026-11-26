using System;
using System.Collections.Generic;
using Bootstrap;
using Summer;
using UnityEngine;
using Zenject;


public class FirstStage
{
    
}
public class SecondStage
{
    
}
public class ThirdStage
{
    
}

public class BootstrapInstaller : MonoInstaller
{
    [SerializeField] private Module3 module3;
    [SerializeField] private BootstrapStarter _bootstrapStarter;
    [SerializeField] private CoroutineRunner _runner;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<CoroutineRunner>().FromInstance(_runner).AsSingle();
        
        
        Container.BindInterfacesAndSelfTo<Module1>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Module3>().FromInstance(module3).AsSingle();
        Container.BindInterfacesAndSelfTo<Module2>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Module4>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Module5>().FromNew().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Module6>().FromNew().AsSingle().NonLazy();
        
        
        Container.BindInterfacesAndSelfTo<IInitializer>().FromMethod(CreateInitializer).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Bootstrap.Bootstrap>().FromNew().AsSingle().NonLazy();
        Container.Inject(_bootstrapStarter);
    }

    // 3 стадии
    
    // 5 модулей
    
    // во второй стадии ассинхрон
    
    // а в 3 стадии нужно прописать зависимость от ассинхрона из 2 стадии и что-нибудь из первой
    
    
    private IInitializer CreateInitializer(InjectContext context)
    {
        var stages = new List<Type>()
        {
            typeof(FirstStage),
            typeof(SecondStage),
            typeof(ThirdStage)
        };
        
        var modules = context.Container.ResolveAll<IExecutableModule>();
        return new Initializer(stages, modules);
    }

}
