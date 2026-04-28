using AppInit.InitSteps.Signals;
using Zenject;

namespace Source.Scripts.AppInit
{
    public class SceneReadyNotifier : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly int _groupId;

        public SceneReadyNotifier(SignalBus signalBus, int groupId)
        {
            _signalBus = signalBus;
            _groupId = groupId;
        }

        public void Initialize()
        {
            _signalBus.Fire(new SceneInitReadySignal(_groupId));
        }
    }
}