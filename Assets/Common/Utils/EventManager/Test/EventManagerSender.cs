using Common.Utils.EventManager;
using Common.Utils.EventManager.Test;
using UnityEngine;

public class EventManagerSender : MonoBehaviour
{
    private void Awake()
    {
        new EventManager().Init();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.Trigger<ExampleSignal, int>(6);
        }
    }
}
