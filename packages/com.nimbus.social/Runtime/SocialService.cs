using System;
using Cysharp.Threading.Tasks;
using Nimbus.Attribution;
using Nimbus.Core;
using UnityEngine;

namespace Nimbus.Social;

public class SocialService : ISocialService
{
    private readonly IAttributionService _attributionService;

    public SocialService(IAttributionService attributionService)
    {
        _attributionService = attributionService ?? throw new ArgumentNullException(nameof(attributionService));
    }

    public virtual void Share(string text, string? imagePath = null)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        Log.Social.Info($"Share requested: {text} (Image: {imagePath ?? "none"})");
    }

    public virtual void RequestRating()
    {
        Log.Social.Info("Rating requested");
    }

    public virtual UniTask<string> GenerateReferralLinkAsync(string campaign)
    {
        if (campaign == null)
        {
            throw new ArgumentNullException(nameof(campaign));
        }

        Log.Social.Info($"Generating referral link for campaign: {campaign}");
        return UniTask.FromResult($"https://game.link/referral?campaign={campaign}");
    }

    public virtual void OpenUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            Log.Social.Warning("Attempted to open an empty URL.");
            return;
        }

        Log.Social.Info($"Opening URL: {url}");
        Application.OpenURL(url);
    }
}
