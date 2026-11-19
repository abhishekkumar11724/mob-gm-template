using Nimbus.Core;
using Cysharp.Threading.Tasks;

namespace Nimbus.Social;

public interface ISocialService : IService
{
    void Share(string text, string? imagePath = null);

    void RequestRating();

    UniTask<string> GenerateReferralLinkAsync(string campaign);

    void OpenUrl(string url);
}
