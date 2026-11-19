using Cysharp.Threading.Tasks;
using Nimbus.Core;

namespace Nimbus.Consent;

public interface IConsentService : IService
{
    bool HasConsent(ConsentPurpose purpose);

    void SetCmpProvider(ICmpProvider provider);

    UniTask RequestConsentAsync();

    UniTask<ATTStatus> RequestATTAsync();

    void Revoke(ConsentPurpose purpose);

    void ShowPrivacySettings();
}
