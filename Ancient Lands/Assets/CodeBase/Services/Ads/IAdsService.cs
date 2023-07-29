using System;

namespace CodeBase.Services.Ads
{
    public interface IAdsService:IService
    {
    event Action RewardedVideoReady;
    bool IsRewardedVideoReady { get; set; }
    int Reward { get;}
    void Initialize();
    void ShowAd(Action onVideoFineshed);
    }
}