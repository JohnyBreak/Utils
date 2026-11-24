using System;

namespace Bootstrap.Strategies
{
    public interface IGetDependenciesStrategy
    {
        bool TryGetDependencies(object obj, out Type[] dependencies);
    }
}