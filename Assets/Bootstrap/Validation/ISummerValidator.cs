using System.Collections.Generic;

namespace Bootstrap.Validation
{
    public interface ISummerValidator
    {
        void Validate(List<IExecutableModule> modules);
    }
}