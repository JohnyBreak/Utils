using UnityEngine;
using Zenject;

namespace Source.Scripts.App
{
    public class App : MonoBehaviour
    {
        [Inject] private AppController _controller;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnApplicationPause(bool isPaused)
        {
            _controller.Pause(isPaused);
        }

        private void OnApplicationQuit()
        {
            _controller.Quit();
        }
    }
}