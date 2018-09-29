using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class GUITypePlay : MonoBehaviour {

    public Sprite sprPVP, sprPVC;

    private Image imgTypePlay;
    void Awake()
    {
        if (PlayerPrefs.GetInt("inittypeplay") == 0)
        {
            Defs.typePlay = TypePlay.PVC;
            PlayerPrefs.SetInt("inittypeplay", 1);

        }
        else
        {
            Defs.SetTypePlay(PlayerPrefs.GetInt("typeplay"));
        }

        SetTypePlay();
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("typeplay", (int)Defs.typePlay);
    }

    public void OnClickTypePlay()
    {
        if (Defs.typePlay == TypePlay.PVC)
            Defs.typePlay = TypePlay.PVP;
        else if (Defs.typePlay == TypePlay.PVP)
            Defs.typePlay = TypePlay.PVC;

        SetTypePlay();
    }


    void SetTypePlay()
    {
        if (imgTypePlay == null)
        {
            imgTypePlay = gameObject.GetComponent<Image>();
        }

        switch (Defs.typePlay)
        {
            case TypePlay.PVP: imgTypePlay.sprite = sprPVP;
                break;
            case TypePlay.PVC: imgTypePlay.sprite = sprPVC;
                break;
            default: imgTypePlay.sprite = sprPVC;
                break;
        }
    }
}
