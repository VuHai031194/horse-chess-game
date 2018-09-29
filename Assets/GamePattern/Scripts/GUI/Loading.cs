using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour {

    public RectTransform loadingBox;
    public Text txtLoading;

    private AsyncOperation async = null; // When assigned, load is in progress.

    private float widthLoadingBox;

    private float countTime = 0;
    private int countText = 0;
    private IEnumerator LoadALevel(string levelName)
    {
        yield return new WaitForSeconds(1);
        async = SceneManager.LoadSceneAsync(levelName);
        yield return async;
    }

    void Awake()
    {
        widthLoadingBox = loadingBox.rect.width;
        loadingBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
        //Debug.Log("Width Loading: " + widthLoadingBox);
        
        txtLoading.text = "Loading.";
        if (PlayerPrefs.GetInt("playfirst") == 0)
        {
            StartCoroutine(LoadALevel("Tutorial"));
            PlayerPrefs.SetInt("playfirst", 1);
        }
        else
        {
            StartCoroutine(LoadALevel("Game"));
        }

    }

    void Update()
    {
        countTime += Time.deltaTime * 2;
        if (countTime > 1)
        {
            countTime -= 1;
            countText++;
            switch (countText % 3)
            {
                case 0: txtLoading.text = "Loading."; break;
                case 1: txtLoading.text = "Loading.."; break;
                case 2: txtLoading.text = "Loading..."; break;
            }
        }

        if (async != null)
        {
            //Debug.Log(async.progress * 100);
            loadingBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthLoadingBox * async.progress);
        }
    }

    
}
