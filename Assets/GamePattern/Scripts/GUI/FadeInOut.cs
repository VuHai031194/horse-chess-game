using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour {

    [HideInInspector]
    public TypeFade typeFade;
    [HideInInspector]
    public CanvasGroup cg;

    void Awake()
    {
        InitCG();
    }

    void InitCG()
    {
        if (cg == null)
        {
            cg = GetComponent<CanvasGroup>();
        }
    }

    public void OnClickFadeIn()
    {
        InitCG();
        cg.transform.parent.gameObject.SetActive(true);
        StartCoroutine(Fade(TypeFade.In));
    }

    public void OnClickFadeOut()
    {
        StartCoroutine(Fade(TypeFade.Out));
    }

    // Update is called once per frame
    IEnumerator Fade(TypeFade type)
    {
        switch(type)
        {
            case TypeFade.In:
              
                
                while (cg.alpha < 1)
                {
                    cg.alpha += Time.deltaTime * 3;
                    yield return null;
                }
                cg.interactable = true;
            break;
            
            case TypeFade.Out:
                cg.interactable = false;
                while (cg.alpha > 0)
                {
                    cg.alpha -= Time.deltaTime * 5;
                    yield return null;
                }
                cg.transform.parent.gameObject.SetActive(false);
            break;
        }
        
    }
}
