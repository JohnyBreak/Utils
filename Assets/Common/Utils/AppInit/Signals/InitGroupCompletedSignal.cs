namespace AppInit.InitSteps.Signals
{
    public readonly struct InitGroupCompletedSignal
    {
        public readonly int GroupId;

        public InitGroupCompletedSignal(int groupId) => GroupId = groupId;
    }
}
