using Common.Utils.EventManager;
using Common.Utils.EventManager.Test;
using UnityEngine;

public class EventManagerReceiver : MonoBehaviour
{
    void Start()
    {
        EventManager.Listen<ExampleSignal, int>(this, Action);
    }

    private void Action(int obj)
    {
        Debug.LogError(obj);
    }
}
