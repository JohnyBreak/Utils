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

        public Promise PreExecute()// TODO надо ли заводить отдельные промисы под каждый инит
        {
            _promise = _original.PreExecute();
            return _promise;
        }

        public Promise Execute()
        {
            _promise = _original.Execute();
            return _promise;
        }

        public Promise PostExecute()
        {
            _promise = _original.PostExecute();
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