using Summer;
using UnityEngine;
using Zenject;

public class BootstrapStarter : MonoBehaviour
{
    [Inject] private readonly Bootstrap.Bootstrap _bootstrap;
    
    private void Start()
    {
        _bootstrap.Start();
    }
}
