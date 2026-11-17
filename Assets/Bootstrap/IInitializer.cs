using System.Collections;

namespace Bootstrap
{
    public interface IInitializer
    {
        IEnumerator Start();
        bool IsComplete();
    }
}