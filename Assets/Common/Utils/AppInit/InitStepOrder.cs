using System;
using System.Collections.Generic;

namespace AppInit.InitSteps
{
    public class InitStepOrder : Attribute
    {
        public readonly int GroupId;
        public readonly IReadOnlyList<Type> Dependencies;

        public InitStepOrder(int groupId, params Type[] dependencies)
        {
            GroupId = groupId;
            Dependencies = dependencies;
        }

        public static readonly InitStepOrder Default = new(0);
    }
}