using System;

namespace Bootstrap.Strategies
{
    public class AttributeDependenciesGetter : IGetDependenciesStrategy
    {
        public bool TryGetDependencies(object obj, out Type[] dependencies)
        {
            if (!Accepted(obj))
            {
                dependencies = null;
                return false;
            }

            dependencies = GetDependencies(obj);
            return true;
        }

        private bool Accepted(object obj)
        {
            return SummerHelper.HasSummerAttributes<SummerSettingsAttribute>(obj);
        }
        
        private Type[] GetDependencies(object obj)
        {
            var attributes = SummerHelper.GetSummerAttributes(obj);
            
            var result = attributes[0].Dependencies;
            
            return result;
        }
    }
}