using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TypeGui
{
    Default,
    Menu,
    Info,
    Guide,
    GameOver

}
public enum TypeShow
{
    TweenPosition,
    TweenScale,
    FadeInOut
}

public enum TypeFade
{
    In,
    Out
}
public class GUIEvents : MonoBehaviour {

    public static GUIEvents main;

    public List<GUIController> guiControllers = new List<GUIController>();
    private Dictionary<string, GUIController> dicGui = new Dictionary<string, GUIController>();

    public string nameScene;

    void Awake()
    {
        main = this;
        for (int i = 0; i < guiControllers.Count; i++)
        {
            dicGui.Add(guiControllers[i].name, guiControllers[i]);
        }

        //if (GoogleAds.main)
        //    GoogleAds.main.RequestInterstitial();
    }

    public void RunForward(string nameGui)
    {
        Run(nameGui, true);
    }

    public void RunReverse(string nameGui)
    {
        Run(nameGui, false);
    }

    private void Run(string nameGui, bool forward)
    {
        if (forward)
        {
            dicGui[nameGui].RunForward();
        }else{
            dicGui[nameGui].RunReverse();
        }
    }

    void Init()
    {
    }

    public void RateApp()
    {
        switch(Application.platform){
            case RuntimePlatform.Android:
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.htbros.horseschess");
                break;
            case RuntimePlatform.IPhonePlayer:
                Application.OpenURL("https://itunes.apple.com/us/app/horses-chess-game/id1116154036&mt=8");
                break;
            default: 
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.htbros.horseschess");
                break;
        }
           
    }

    public void LikePage()
    {

        Application.OpenURL("");
    }

    public void LikeMoreGames()
    {
        Application.OpenURL("");
    }
}
