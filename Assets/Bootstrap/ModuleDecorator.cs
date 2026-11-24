using System;

namespace Bootstrap
{
    public class ModuleDecorator : IExecutableModule, IInitialized
    {
        private readonly IExecutableModule _original;
        private Promise _promise;
        
        public ModuleDecorator(IExecutableModule original)
        {
            _original = original;
        }

        public Promise Execute()
        {
            _promise = _original.Execute();
            return _promise;
        }

        public bool IsInitialized()
        {
            return _promise.GetStatus() == SummerStatus.Success;
        }

        public Type GetOriginalType()
        {
            return _original.GetType();
        }
    }
}