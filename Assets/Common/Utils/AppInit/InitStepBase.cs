using System;
using UniRx;
using UnityEngine;

namespace AppInit.InitSteps
{
    public abstract class InitStepBase : IDisposable
    {
        public enum StateCode
        {
            None = 0,
            InProgress,
            Error,
            Done
        }

        public StateCode State { get; private set; }

        private bool _isCompletedOnce;

        protected CompositeDisposable CompositeDisposable { get; private set; }

        public virtual bool Once => false;

        public Exception Error { get; private set; }

        protected virtual bool Prerequisites()
        {
            return true;
        }

        protected abstract void OnStart();
        protected virtual void OnDone(bool isSuccess) { }

        protected void Done()
        {
            OnDone(true);

            if(!_isCompletedOnce)
            {
                _isCompletedOnce = true;
            }

            CompositeDisposable?.Dispose();
            CompositeDisposable = null;

            State = StateCode.Done;
        }

        protected void Fail() =>
            Fail(new Exception());

        protected void Fail(string message) =>
            Fail(new Exception(message));
        
        protected void Fail(Exception exception)
        {
            OnDone(false);

            Error = exception;
            
            Debug.LogException(exception);
            
            State = StateCode.Error;
        }

        public void Start()
        {
            if(State == StateCode.InProgress)
            {
                throw new InvalidOperationException("Unable to start Init Step which is already started");
            }

            if(Once && _isCompletedOnce)
            {
                State = StateCode.Done;
                return;
            }

            if(!Prerequisites())
            {
                return;
            }

            CompositeDisposable = new CompositeDisposable();

            State = StateCode.InProgress;
            OnStart();
        }

        public void Reset()
        {
            Error = null;
            State = StateCode.None;
        }

        public virtual void Dispose()
        {
            CompositeDisposable?.Dispose();
        }
    }
}