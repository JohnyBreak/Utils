using System;
using AppInit.InitSteps;
using AppInit.InitSteps.Signals;
using UnityEngine;
using Zenject;

namespace Source.Scripts.App
{
    public class AppController : IInitializable, IDisposable
    {
        private readonly StepIniter _stepIniter;
        private readonly SignalBus _signalBus;

        public AppController(StepIniter stepIniter, SignalBus signalBus)
        {
            _stepIniter = stepIniter;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SceneInitReadySignal>(OnSceneInitReady);
        }

        public void Pause(bool isPaused)
        {
            Debug.Log($"AppController.Pause: {isPaused}");
            _signalBus.Fire(new AppPausedSignal(isPaused));
        }

        public void Quit()
        {
            Debug.Log("AppController.Quit");
            _signalBus.Fire(new AppQuittingSignal());
        }

        private void OnSceneInitReady(SceneInitReadySignal signal)
        {
            Debug.Log($"AppController.OnSceneInitReady: group {signal.GroupId}");
            _stepIniter.Run(signal.GroupId);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SceneInitReadySignal>(OnSceneInitReady);
        }
    }
}