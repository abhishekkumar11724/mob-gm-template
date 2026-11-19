using Cysharp.Threading.Tasks;

namespace Nimbus.Consent;

public interface ICmpProvider
{
    UniTask InitializeAsync();

    UniTask RequestConsentAsync();

    bool HasConsent(ConsentPurpose purpose);

    void ShowPrivacySettings();
}
