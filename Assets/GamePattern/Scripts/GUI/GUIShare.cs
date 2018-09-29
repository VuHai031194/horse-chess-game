using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIShare : MonoBehaviour {

    public Button btnShare;
    public CanvasGroup objShare;

    private bool isScale = false;

    void OnEnable()
    {
        btnShare.interactable = true;
        objShare.alpha = 0;
        //objShare.transform.localScale = new Vector3(1, 0, 0);
        isScale = false;
    }

    void OnDisable()
    {
        btnShare.interactable = false;

    }

    public void OnClickShare()
    {
        isScale = !isScale;
        if (isScale)
        {
            StartCoroutine(FadeIn(true));
        }
        else
        {
            StartCoroutine(FadeIn(false));
        }
    }

    public IEnumerator FadeIn(bool fadeIn)
    {
        int speed = fadeIn ? 1 : -1;
        while (true)
        {
            yield return null;
            objShare.alpha += speed * 0.1f;
            if (fadeIn && objShare.alpha >= 1)
                break;
            else if (!fadeIn && objShare.alpha <= 0)
                break;
                
        }
    }
}
