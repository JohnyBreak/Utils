using System;

namespace Common.BSM.Runtime
{
    public class Transition
    {
        public readonly IState To;
        public readonly Func<bool> Condition;

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }

        public bool IsValid()
        {
            return To != null && Condition != null;
        }

        public static Transition GetEmpty()
        {
            return new Transition(null, null);
        }
    }
}