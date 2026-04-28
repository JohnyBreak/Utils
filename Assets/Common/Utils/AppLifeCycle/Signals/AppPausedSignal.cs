namespace Source.Scripts.App
{
    public readonly struct AppPausedSignal
    {
        public readonly bool IsPaused;

        public AppPausedSignal(bool isPaused)
        {
            IsPaused = isPaused;
        }
    }
}
