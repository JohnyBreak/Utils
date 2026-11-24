using System;

namespace Bootstrap.Strategies
{
    public class InterfaceDependenciesGetter : IGetDependenciesStrategy
    {
        public bool TryGetDependencies(object obj, out Type[] dependencies)
        {
            if (!Accepted(obj))
            {
                dependencies = null;
                return false;
            }

            dependencies = ((IDependent)obj).GetDependencies();
            return true;
        }
        
        private bool Accepted(object obj)
        {
            return obj is IDependent;
        }
    }
}