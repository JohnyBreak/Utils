using System;

namespace Bootstrap
{
    public class MainStage
    {
    
    }
    
    public interface ISummerModule
    {
        int GetOrder();

        Type GetStage()
        {
            return typeof(MainStage);
        }
    }
}