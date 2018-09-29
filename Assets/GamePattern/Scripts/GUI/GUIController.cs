using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

    public static GUIController main;

    [HideInInspector]
    public TypeShow typeShow;
    [HideInInspector]
    public bool isSpread;
    [HideInInspector]
    public GameObject image;
    [HideInInspector]
    public bool enableGoogleAnalytics;
    void Awake()
    {
        main = this;
        InitImage();
    }

    void OnEnable()
    {
        //if (GoogleAnalyticsV4.instance{
        //    GoogleAnalyticsV4.instance.LogScreen(gameObject.name);
        //}
    }

    void OnDisable()
    {

    }

    void InitImage()
    {
        if (image == null)
        {
            image = transform.Find("Image").gameObject;
        }
    }

    public void RunForward() { Run(true); }

    public void RunReverse() { Run(false);  }

    [System.Obsolete("Using RunForward/RunReverse instead")]
    public void Run() { Run(true);  }

    public void Run(bool forward)
    {
        InitImage();
        switch (typeShow)
        {
            case TypeShow.FadeInOut:
                var fade = image.GetComponent<FadeInOut>();
                if (forward)
                {
                    fade.OnClickFadeIn();
                }
                else
                {
                    fade.OnClickFadeOut();
                }
                break;
            case TypeShow.TweenPosition:
                var pos = image.GetComponent<TweenPosition>();
                if (forward)
                {
                    pos.PlayForward();
                }
                else
                {
                    pos.PlayReverse();
                }
                break;
            case TypeShow.TweenScale:
                var scale = image.GetComponent<TweenScale>();
                if (forward)
                {
                    scale.PlayForward();
                }
                else
                {
                    scale.PlayReverse();
                }
                break;
        }
    }

    #region GUI EDITOR FUNCTION

    public void AddShowComponent(TypeShow typeShow)
    {
        InitImage();
        switch (typeShow)
        {
            case TypeShow.FadeInOut:
                image.AddComponent<CanvasGroup>();
                image.AddComponent<FadeInOut>();
                break;
            case TypeShow.TweenPosition:
                var pos = image.AddComponent<TweenPosition>();
                pos.enabled = false;
                break;
            case TypeShow.TweenScale:
                var scale = image.AddComponent<TweenScale>();
                scale.enabled = false;
                break;
            default:
                Debug.LogError(typeShow.ToString() + "is not EXITS. ERROR!");
                break;

        }
    }

    public void DestroyShowComponent(TypeShow typeShow)
    {
        InitImage();
        switch (typeShow)
        {
            case TypeShow.FadeInOut:
                DestroyImmediate(image.GetComponent<FadeInOut>());
                DestroyImmediate(image.GetComponent<CanvasGroup>());
                break;
            case TypeShow.TweenPosition:
                DestroyImmediate(image.GetComponent<TweenPosition>());
                break;
            case TypeShow.TweenScale:
                DestroyImmediate(image.GetComponent<TweenScale>());
                break;
            default:
                Debug.LogError(typeShow.ToString() + "is not EXITS. ERROR!");
                break;

        }
    }
    #endregion
}
