using System;

namespace Bootstrap.Editor
{
    public interface ISummerModule
    {
        int GetOrder();

        Type GetStage()
        {
            return typeof(MainStage);
        }
    }
}