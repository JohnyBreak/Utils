using Common.BSM.Runtime;
using UnityEngine;

namespace Common.BSM.External
{
    public class FSMTest : MonoBehaviour
    {
        private BehaviourStateMachine _fsm;
        private void Start()
        {
            ConcreteState state1 = new ConcreteState(() => {Debug.Log("enter 1"); },
                () => {Debug.Log("update 1"); },
                () => {Debug.Log("exit 1"); }
            ); 
            
            ConcreteState state2 = new ConcreteState(() => {Debug.Log("enter 2"); },
                () => {Debug.Log("update 2"); },
                () => {Debug.Log("exit 2"); }
            );
            
            ConcreteState state3 = new ConcreteState(() => {Debug.Log("enter 3"); },
                () => {Debug.Log("update 3"); },
                () => {Debug.Log("exit 3"); }
            );
            
            ConcreteState state5 = new ConcreteState(() => {Debug.Log("enter 5"); },
                () => {Debug.Log("update 5"); },
                () => {Debug.Log("exit 5"); }
            );
            _fsm = new BehaviourStateMachine();
            
            _fsm.AddTransition(state1, new Transition(state2, () => Input.GetKeyDown(KeyCode.Alpha2)));
            _fsm.AddTransition(state2, new Transition(state1, () => Input.GetKeyDown(KeyCode.Alpha1)));
            //_fsm.AddTransition(state3, new Transition(state3, () => Input.GetKeyDown(KeyCode.Alpha3)));
            _fsm.AddTransition(new Transition(state5, () => Input.GetKeyDown(KeyCode.Alpha5)));
            
            _fsm.Init(state1);
        }

        private void Update()
        {
            _fsm.Update();
        }
    }
}