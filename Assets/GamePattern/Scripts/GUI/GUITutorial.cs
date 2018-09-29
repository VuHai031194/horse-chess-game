using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public class GUITutorial : MonoBehaviour {

    public Text txtAnnouce;
    public Button btnNext;
    private int step = 0;
    private string[] strAnnouce = new string[] {
        "Chess board when starting game",
        "Choose horse to move",
        "Choose where you want to horse go to",
        "Horse can move 8 direction",
        "Horse can't move",
        "Capture Horse",
        "Get 1 score",
        "Get 2 score",
        "Get 6 score first to WIN",
        "Thinking over 30s will be lose"
    };

    void Start()
    {
        GUITime.main.isUpdateLogic = false;
        Init();
    }

    public void Init()
    {
        step = 0;
        txtAnnouce.text = strAnnouce[0];
    }

    public void OnClickNextStep()
    {
        switch (step)
        {
            case 0:
                GUIEvents.main.RunForward("TutorialPlay0");
                GUIEvents.main.RunReverse("TutorialCurtain");
                txtAnnouce.text = strAnnouce[1];
                btnNext.interactable = false;
                //FEN.Default = "nnn2nnn/8/8/8/8/8/8/NNN2NNN b 00";
                //GameManager.main.Init();
                step++;
                break;
            case 1:
                GUIEvents.main.RunForward("TutorialPlay2");
                txtAnnouce.text = strAnnouce[3];
                step++;
                FEN.Default = "nnn2nnn/8/8/8/8/8/8/1NN2NNN b 00";
                GameManager.main.Init();
                btnNext.interactable = false;
                StartCoroutine(ActiveBtnNext());
                break;
            case 2:
                GUIEvents.main.RunReverse("TutorialPlay2");
                GUIEvents.main.RunForward("TutorialPlay3");
                txtAnnouce.text = strAnnouce[4];
                FEN.Default = "nn3nnn/8/8/8/8/8/8/1N3NNN b 00";
                GameManager.main.Init();
                step++;
                btnNext.interactable = false;
                StartCoroutine(ActiveBtnNext());
                break;
            case 3:
                // Capture
                GUIEvents.main.RunReverse("TutorialCurtain");
                GUIEvents.main.RunReverse("TutorialPlay3");
                GUIEvents.main.RunForward("TutorialPlay4");
                FEN.Default = "nn3nnn/8/3n4/8/4N3/8/8/1NN2NNN b 00";
                GameManager.main.Init();

                btnNext.interactable = false;
                txtAnnouce.text = strAnnouce[5];
                step++;
                break;
            case 4: 
                // Get Score
                GUIEvents.main.RunReverse("TutorialCurtain");
                GUIEvents.main.RunForward("TutorialPlay6");
                FEN.Default = "nn3nnn/n7/3N4/8/8/8/8/1NN2NNN b 00";
                GameManager.main.Init();
       
                btnNext.interactable = false;
                txtAnnouce.text = strAnnouce[7];
                step++;
                break;
            case 5:
                // Get Score
                GUIEvents.main.RunReverse("TutorialCurtain");
                GUIEvents.main.RunForward("TutorialPlay8");
                FEN.Default = "nn3nnn/n7/2N5/8/8/8/8/1NN2NNN b 00";
                GameManager.main.Init();
       
                btnNext.interactable = false;
                txtAnnouce.text = strAnnouce[6];
                step++;
                break;
            case 6:
                txtAnnouce.text = strAnnouce[8];
                step++;
                //SceneManager.LoadScene("Game");
                break;
            case 7: 
                txtAnnouce.text = strAnnouce[9];
                step++;
                break;
            case 8:
#if UNITY_5_3
                SceneManager.LoadScene("Game");
#else
                Application.LoadLevel("Game");
#endif
                break;
        }
    }

    public void OnClickHorseFirst()
    {
        GUIPlay.main.OnClickCell(57);
        GUIEvents.main.RunReverse("TutorialPlay0");
        GUIEvents.main.RunForward("TutorialPlay1");
        txtAnnouce.text = strAnnouce[2];
    }

    public void OnClickCells(int index)
    {
        GUIPlay.main.OnClickCell(index);
        GUIEvents.main.RunReverse("TutorialPlay1");
        GUIEvents.main.RunForward("TutorialCurtain");
        StartCoroutine(ActiveBtnNext());
    }

    public void OnClickCellsSecond(int index)
    {
        GUIPlay.main.OnClickCell(index);
        btnNext.interactable = false;
        GUIEvents.main.RunReverse("TutorialPlay4");
        GUIEvents.main.RunForward("TutorialPlay5");
    }

    public void OnClickCellsThird(int index)
    {
        GUIPlay.main.OnClickCell(index);
        StartCoroutine(ActiveBtnNext());
        GUIEvents.main.RunReverse("TutorialPlay5");
        GUIEvents.main.RunForward("TutorialCurtain");
    }

    public void OnClickHorseSecond(int index)
    {
        GUIPlay.main.OnClickCell(index);
        GUIEvents.main.RunForward("TutorialPlay7");
        GUIEvents.main.RunReverse("TutorialPlay6");
        
    }

    public void OnClickScoreTwo()
    {
        GUIPlay.main.OnClickCell(4);
        GUIEvents.main.RunReverse("TutorialPlay7");
        GUIEvents.main.RunForward("TutorialCurtain");
        StartCoroutine(ActiveBtnNext());
    }

    public void OnClickHorseThird(int index)
    {
        GUIPlay.main.OnClickCell(index);
        GUIEvents.main.RunForward("TutorialPlay9");
        GUIEvents.main.RunReverse("TutorialPlay8");

    }

    public void OnClickScoreOne()
    {
        GUIPlay.main.OnClickCell(3);
        GUIEvents.main.RunReverse("TutorialPlay9");
        GUIEvents.main.RunForward("TutorialCurtain");
        StartCoroutine(ActiveBtnNext());
    }

    IEnumerator ActiveBtnNext()
    {
        yield return new WaitForSeconds(1f);
        btnNext.interactable = true;
    }

    public void SkipTutorial()
    {
        step = 8;
        OnClickNextStep();
    }
}
