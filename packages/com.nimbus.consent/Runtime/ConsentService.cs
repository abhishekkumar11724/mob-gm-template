using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Nimbus.Consent.Events;
using Nimbus.Core;

namespace Nimbus.Consent;

public sealed class ConsentService : IConsentService
{
    private readonly HashSet<ConsentPurpose> _consentedPurposes = new();
    private ICmpProvider? _cmpProvider;

    public bool HasConsent(ConsentPurpose purpose)
    {
        if (_cmpProvider != null)
        {
            return _cmpProvider.HasConsent(purpose);
        }

        return _consentedPurposes.Contains(purpose);
    }

    public void SetCmpProvider(ICmpProvider provider)
    {
        _cmpProvider = provider;

        EventBus.Publish(new ConsentChanged());
    }

    public async UniTask RequestConsentAsync()
    {
        if (_cmpProvider != null)
        {
            await _cmpProvider.InitializeAsync();
            await _cmpProvider.RequestConsentAsync();
        }
        else
        {
            _consentedPurposes.Add(ConsentPurpose.Advertising);
            _consentedPurposes.Add(ConsentPurpose.Analytics);
            _consentedPurposes.Add(ConsentPurpose.Personalization);
        }

        EventBus.Publish(new ConsentChanged());
    }

    public async UniTask<ATTStatus> RequestATTAsync()
    {
        return await ATTPromptHelper.RequestAsync();
    }

    public void Revoke(ConsentPurpose purpose)
    {
        if (_consentedPurposes.Remove(purpose))
        {
            EventBus.Publish(new ConsentChanged());
        }
    }

    public void ShowPrivacySettings()
    {
        if (_cmpProvider != null)
        {
            _cmpProvider.ShowPrivacySettings();
        }
    }
}
