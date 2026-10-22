using System;
using UnityEngine;

public interface IInputAdapter
{
    event Action AttackPressedEvent;
    event Action AttackReleasedEvent;
    bool AttackIsHold();
    
    event Action ReloadPressedEvent;
}

public class InputAdapter : MonoBehaviour, IInputAdapter
{
    public event Action AttackPressedEvent;
    public event Action AttackReleasedEvent;
    public event Action ReloadPressedEvent;
    private bool _attackHold;
    
    public bool AttackIsHold()
    {
        throw new NotImplementedException();
    }
    
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackPressedEvent?.Invoke();
        }
        
        _attackHold = Input.GetKey(KeyCode.Mouse0);
        
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            AttackReleasedEvent?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadPressedEvent?.Invoke();
        }
    }
}
