namespace Common.BSM.Runtime
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    } 
}