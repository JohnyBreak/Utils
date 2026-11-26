
namespace Bootstrap
{
    public interface IExecutableModule
    {
        Promise PreExecute();
        Promise Execute();
        Promise PostExecute();
    }
}