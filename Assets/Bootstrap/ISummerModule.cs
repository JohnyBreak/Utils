using System;

namespace Bootstrap
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