namespace AppInit.InitSteps.Signals
{
    public readonly struct SceneInitReadySignal
    {
        public readonly int GroupId;

        public SceneInitReadySignal(int groupId)
        {
            GroupId = groupId;
        }
    }
}
