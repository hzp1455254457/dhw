//广告多方接入接口
using System;
public interface IAdInterface 
{
    public void ShowFullVideo(Action callback = null);
    public void CloseFullVideo();
    public void ShowRewardVideo(string tag,Action callback = null);
    public void CloseRewardVideo();
    public void ShowTableVideo(string tag, Action callback = null);
    public void CloseTableVideo();

    public void ShowBanner();
    public void CloseBanner();
    public void ShowFeed(Action callback =null);
    public void CloseFeed();
    //public int getRewardVideoAdEcpm();
    //public int getTableVideoAdEcpm();
    //public int getFullScreenVideoAdEcpm();
    //public bool isBannerExposured();
    //public bool isRewardVideoLoaded();


}
