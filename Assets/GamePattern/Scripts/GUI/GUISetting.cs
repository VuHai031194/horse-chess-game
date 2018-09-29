using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUISetting : MonoBehaviour {

    public InputField txtNamePlayer;
    public Text txtThinkingLevel;
    public Text txtGoFirst;
    public Button btnOK;

    private string namePlayer;
    private int thinkingLevel;
    private int goFirst;

    void OnEnable()
    {
        
        GetValue();
  
        SetThinkingLevel(this.thinkingLevel);
        SetGoFirst(this.goFirst);
        SetName(this.namePlayer);
    }

    void OnDisable()
    {
        SetValue();
    }

    void SetValue()
    {
        //namePlayer = txtNamePlayer.text;
        PlayerPrefs.SetString("nameplayer", namePlayer);
        PlayerPrefs.SetInt("thinkinglevel", thinkingLevel);
        PlayerPrefs.SetInt("gofirst", goFirst);
    }

    void GetValue()
    {
        namePlayer = PlayerPrefs.GetString("nameplayer");
        thinkingLevel = PlayerPrefs.GetInt("thinkinglevel");
        goFirst = PlayerPrefs.GetInt("gofirst");
    }

    public void OnInputName()
    {
        this.namePlayer = txtNamePlayer.text;
        SetName(this.namePlayer);
    }

    public void OnChangeThinkingLevel(bool increase)
    {
        int maxLevel = 3;
        int minLevel = 1;
        if (increase)
        {
            this.thinkingLevel += 1;
            if (this.thinkingLevel > maxLevel)
            {
                this.thinkingLevel = minLevel;
            }
        }
        else
        {
            this.thinkingLevel -= 1;
            if (this.thinkingLevel < minLevel)
            {
                this.thinkingLevel = maxLevel;
            }
        }
        SetThinkingLevel(this.thinkingLevel);
    }

    public void OnChangGoFirst()
    {
        this.goFirst = (goFirst == 0) ? 1 : 0;
        SetGoFirst(this.goFirst);
    }


    void SetName(string namePlayer)
    {
        txtNamePlayer.text = namePlayer;
        btnOK.enabled = namePlayer.Length > 0;
    }

    void SetThinkingLevel(int thinkingLevel)
    {
        string[] levelString = new string[]{
            "begin", "medium", "hard"
        }; 
        txtThinkingLevel.text = levelString[thinkingLevel - 1];
    }

    void SetGoFirst(int goFist)
    {
        txtGoFirst.text = goFirst > 0 ? "BLUE" : "RED";
    }
}
