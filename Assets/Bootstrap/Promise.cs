namespace Bootstrap
{
    public class PromiseStatusHolder
    {
        public SummerStatus Status;
    }

    public class Promise
    {
        private SummerStatus _status;
        private bool _completed;
        //public Action OnComplete;
        
        public void Complete(SummerStatus status)
        {
            _completed = true;
            _status = status;
        }

        public SummerStatus GetStatus()
        {
            return _status;
        }

        public bool IsCompleted()
        {
            return _completed;
        }

        public static Promise Success()
        {
            return new Promise() { _status = SummerStatus.Success, _completed = true };
        }
    }
}