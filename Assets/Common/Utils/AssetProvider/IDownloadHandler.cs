using Cysharp.Threading.Tasks;

namespace AssetProvider
{
    public interface IDownloadHandler
    {
        UniTaskVoid Handle(AssetLoader assetLoader);
        bool IsInProgress();
    }
}