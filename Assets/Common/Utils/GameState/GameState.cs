using System;

namespace GameState
{
    public class GameState
    {
        public enum State 
        {
            None = 0,
            Paused = 1,
            GamePlay = 2
        }
    
        public State CurrentState { get; private set; }
        public event Action<State> GameStateChangedEvent;


        public void SetState(State newState) 
        {
            if (newState == CurrentState) return;

            CurrentState = newState;
            GameStateChangedEvent?.Invoke(newState);
        }
    }
}

