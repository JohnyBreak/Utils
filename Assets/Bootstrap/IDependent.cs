using System;

namespace Bootstrap
{
    public interface IDependent
    {
        Type[] GetDependencies();
    }
}