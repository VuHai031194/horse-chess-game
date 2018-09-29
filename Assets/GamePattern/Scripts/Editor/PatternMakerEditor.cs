using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.IO;
#if UNITY_5_3
using UnityEditor.SceneManagement;
#endif

[InitializeOnLoad]
public class PatternMakerEditor : EditorWindow {
    
    private static PatternMakerEditor window;
    private Vector2 scrollViewVector;
    private static int selected;
    private static bool isMoreAds;
    string[] toolbarStrings = new string[] { "Editor", "Settings", "Shop", "In-apps", "Ads", "GUI", "Rate", "Help" };
    
    private TypeGui typeGui;
    Dictionary<string, GameObject> guiPrefabs = new Dictionary<string,GameObject>();

    [MenuItem("Window/Pattern Maker Editor")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one
        window = (PatternMakerEditor)EditorWindow.GetWindow(typeof(PatternMakerEditor));
        window.titleContent.text = "Pattern Editor";
        window.Show();
    }

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(PatternMakerEditor));
    }

    // which tab want to focus on
    void OnFocus()
    {
#if UNITY_5_3
        if (EditorSceneManager.GetActiveScene().name == "Game" || EditorSceneManager.GetActiveScene().name == "Tutorial")
#else
        if(Application.loadedLevelName == "Game" || Application.loadedLevelName == "Tutorial")
#endif
        {
            typeGui = TypeGui.Default;
            
        }
    }
    void OnGUI()
    {
        GUI.changed = false;
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        int oldSelected = selected;
        selected = GUILayout.Toolbar(selected, toolbarStrings, new GUILayoutOption[] { GUILayout.Width(450) });
        GUILayout.EndHorizontal();

        scrollViewVector = GUI.BeginScrollView(new Rect(25, 45, position.width - 30, position.height), scrollViewVector, new Rect(0, 0, 400, 1600));
        GUILayout.Space(-30);

        if (oldSelected != selected)
            scrollViewVector = Vector2.zero;

#if UNITY_5_3
        if (EditorSceneManager.GetActiveScene().name == "Game" || EditorSceneManager.GetActiveScene().name == "Tutorial")
#else
        if(Application.loadedLevelName == "Game" || Application.loadedLevelName == "Tutorial")
#endif
        {
            switch (selected)
            {
                case 0: GUIShowEditor(); break;
                case 1: GUIShowSettings(); break;
                case 2: GUIShowShops(); break;
                case 3: GUIShowInAppSettings(); break;
                case 4: GUIShowAds(); break;
                case 5: GUIShowDialogs(); break;
                case 6: GUIShowRate(); break;
                case 7: GUIShowHelp(); break;

            }
        }
        else
        {
            GUIShowWarning();
        }

        GUI.EndScrollView();

#if UNITY_5_3
        if (GUI.changed)
            EditorSceneManager.MarkAllScenesDirty();
#endif

    }

    void GUIShowWarning()
    {
        GUILayout.Space(10);
        GUILayout.Label("CAUTION!", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(600) });
        GUILayout.Label("Please open scene - game ( Assets/GameParttern/Scenes/Game.unity )", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(600) });

    }

    #region GUIShowEditor
    void GUIShowEditor()
    {
        //Debug.LogWarning("Show Gui Editor");
        // write something here game play
    }
    #endregion

    #region GUIShowSettings
    void GUIShowSettings()
    {
        //Debug.LogWarning("Show Gui Settings");
        // write something here
        //GameManager lm = Camera.main.GetComponent<GameManager>();
        //GUILayout.Label("Game settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });

        //if (GUILayout.Button("Reset to default", new GUILayoutOption[] { GUILayout.Width(150) }))
        //{
        //    ResetSettings();
        //}

        //GUILayout.Space(10);

        //bool oldFacebookEnable = lm.FacebookEnable;
        //GUILayout.BeginHorizontal();

        //lm.FacebookEnable = EditorGUILayout.Toggle("Enable Facebook", lm.FacebookEnable, new GUILayoutOption[] {
        //   GUILayout.Width (50),
        //    GUILayout.MaxWidth (200)
        //});
        //if (GUILayout.Button("Install", new GUILayoutOption[] { GUILayout.Width(100) }))
        //{
        //    Application.OpenURL("https://origincache.facebook.com/developers/resources/?id=facebook-unity-sdk-7.4.0.zip");
        //}
        //if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        //{
        //    Application.OpenURL("https://docs.google.com/document/d/1bTNdM3VSg8qu9nWwO7o7WeywMPhVLVl8E_O0gMIVIw0/edit?usp=sharing");
        //}

        //GUILayout.EndHorizontal();

        //if (oldFacebookEnable != lm.FacebookEnable)
        //{
        //    SetScriptingDefineSymbols();
        //    GUILayout.Space(10);
            
        //    //SetScriptingDefineSymbols();
        //    if (lm.FacebookEnable)
        //    {
        //        var googleAds = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/FB"));
        //        googleAds.name = "FB";

        //    }
        //    else
        //    {
        //        DestroyImmediate(GameObject.Find("FB"));
        //    }
        //}


        //if (lm.FacebookEnable)
        //{
        //    string txt = "   if (FBScript.main)\n" +
        //    "   {\n" +
        //    "      FBScript.main.ShareLink();\n" +
        //    "      FBScript.main.ShareFeed();\n" +
        //    "      FBScript.main.InviteFriends();\n" +
        //    "      FBScript.main.GraphRetrievePhoto();\n" +
        //    "      FBScript.main.TakeScreenShoot();\n" +
        //    "      So on...\n" +
        //    "   }\n" +
        //    "   PS: Add many above funtions to every where you want";
        //    GUILayout.TextField(txt, 1000, new GUILayoutOption[] { GUILayout.Width(400), GUILayout.Height(150) });
        //}
      
        //if (lm.FacebookEnable)
        //{
        //    GUILayout.BeginHorizontal();
        //    GUILayout.Space(15);
        //    GUILayout.Label("menu Facebook-> Edit settings", new GUILayoutOption[] { GUILayout.Width(300) });
        //    GUILayout.EndHorizontal();
        //}

    }

    void ResetSettings()
    {

    }
    #endregion

    #region GUIShowShops
    void GUIShowShops()
    {
        //Debug.LogWarning("Show Gui Shops");
        // write something here
    }
    #endregion

    #region GUIShowInAppSettings
    void GUIShowInAppSettings()
    {
        //Debug.LogWarning("Show Gui Settings");
        // write something here
    
        GUILayout.Label("In-apps settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });

        if (GUILayout.Button("Reset to default", new GUILayoutOption[] { GUILayout.Width(150) }))
        {
            //ResetInAppsSettings();
        }

        GUILayout.Space(10);
        //bool oldenableInApps = lm.enableInApps;

        GUILayout.BeginHorizontal();
        bool enableInApps = false;
        enableInApps = EditorGUILayout.Toggle("Enable In-apps", enableInApps, new GUILayoutOption[] {
            GUILayout.Width (180)
        });
        if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        {
            Application.OpenURL("https://docs.google.com/document/d/1HeN8JtQczTVetkMnd8rpSZp_TZZkEA7_kan7vvvsMw0#bookmark=id.b1efplsspes5");
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.Label("Install: Windows->Services->\n In-app Purchasing - ON->Import", new GUILayoutOption[] { GUILayout.Width(400) });
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        //if (oldenableInApps != lm.enableInApps)
        //{
        //    //SetScriptingDefineSymbols();
        //}


        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        string[] InAppIDs = new string[4];
        for (int i = 0; i < 4; i++)
        {
            InAppIDs[i] = EditorGUILayout.TextField("Product id " + (i + 1), "", new GUILayoutOption[] {
                GUILayout.Width (300),
                GUILayout.MaxWidth (300)
            });

        }
        GUILayout.Space(10);

        GUILayout.Label("Android:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        //GUILayout.Label(" Put Google license key into the field \n from the google play account ", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(300) });
        //GUILayout.Space(10);

        //lm.GoogleLicenseKey = EditorGUILayout.TextField("Google license key", lm.GoogleLicenseKey, new GUILayoutOption[] {
        //    GUILayout.Width (300),
        //    GUILayout.MaxWidth (300)
        //});

        GUILayout.Space(10);
        if (GUILayout.Button("Android account help", new GUILayoutOption[] { GUILayout.Width(400) }))
        {
            Application.OpenURL("http://developer.android.com/google/play/billing/billing_admin.html");
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        GUILayout.BeginVertical();

        GUILayout.Space(10);
        GUILayout.Label("iOS:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);

        GUILayout.BeginVertical();

        //GUILayout.Label(" StoreKit library must be added \n to the XCode project, generated by Unity ", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(300) });
        GUILayout.Space(10);
        if (GUILayout.Button("iOS account help", new GUILayoutOption[] { GUILayout.Width(400) }))
        {
            Application.OpenURL("https://developer.apple.com/library/ios/qa/qa1329/_index.html");
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    #endregion

    
    #region GUIShowAds
    void GUIShowAds()
    {
        //GameManager lm = Camera.main.GetComponent<GameManager>();
        ////InitScript initscript = Camera.main.GetComponent<InitScript>();

        ////GOOGLE MOBILE ANAS
        //GUILayout.Label("Analytics settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
        //bool oldenableGoogleMobileAnalytics = initscript.enableGoogleMobileAnalytics;
        //GUILayout.BeginHorizontal();
        //initscript.enableGoogleMobileAnalytics = EditorGUILayout.Toggle("Enable Google Mobile Ana", initscript.enableGoogleMobileAnalytics, new GUILayoutOption[] {
        //    GUILayout.Width (50),
        //    GUILayout.MaxWidth (200)
        //});
        //if (GUILayout.Button("Install", new GUILayoutOption[] { GUILayout.Width(100) }))
        //{
        //    Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases/download/v3.0.1/GoogleMobileAds.unitypackage");
        //}
        //if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        //{
        //    Application.OpenURL("https://docs.google.com/document/d/1I69mo9yLzkg35wtbHpsQd3Ke1knC5pf7G1Wag8MdO-M/edit?usp=sharing");
        //}

        //GUILayout.EndHorizontal();

        //GUILayout.Space(10);
        //if (oldenableGoogleMobileAnalytics != initscript.enableGoogleMobileAnalytics)
        //{

        //    //SetScriptingDefineSymbols();
        //    if (initscript.enableGoogleMobileAnalytics)
        //    {
        //        var googleAds = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GAv4"));
        //        googleAds.name = "GAv4";
                
        //    }
        //    else
        //    {
        //        DestroyImmediate(GameObject.Find("GAv4"));
        //    }
        //}

        
        //if (initscript.enableGoogleMobileAnalytics)
        //{
        //    string txt = "if (GoogleAnalyticsV4.instance)\n" +
        //    "{\n" +
        //    "   GoogleAnalyticsV4.instance.LogScreen(\"gameobject.name\");\n" +
        //    "   GoogleAnalyticsV4.instance.LogEvent(\"Shop\", \"Buy\", \"Heal Point\", 10);\n" +
        //    "   GoogleAnalyticsV4.instance.LogSocial(\"FB\", \"Share\", \"Target\");\n" +
        //    "}\n" +
        //    "PS: Add this script to OnEnable in GUIController";
        //    GUILayout.TextField(txt, 1000, new GUILayoutOption[] { GUILayout.Width(450), GUILayout.Height(100) });
        //}

        //GUILayout.Label("Ads settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
        
        ////GOOGLE MOBILE ADS

        //bool oldenableGoogleMobileAds = initscript.enableGoogleMobileAds;
        //GUILayout.BeginHorizontal();
        //initscript.enableGoogleMobileAds = EditorGUILayout.Toggle("Enable Google Mobile Ads", initscript.enableGoogleMobileAds, new GUILayoutOption[] {
        //    GUILayout.Width (50),
        //    GUILayout.MaxWidth (200)
        //});
        //if (GUILayout.Button("Install", new GUILayoutOption[] { GUILayout.Width(100) }))
        //{
        //    Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases/download/v3.0.1/GoogleMobileAds.unitypackage");
        //}
        //if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        //{
        //    Application.OpenURL("https://docs.google.com/document/d/1I69mo9yLzkg35wtbHpsQd3Ke1knC5pf7G1Wag8MdO-M/edit?usp=sharing");
        //}

        //GUILayout.EndHorizontal();

        //GUILayout.Space(10);
        //if (oldenableGoogleMobileAds != initscript.enableGoogleMobileAds)
        //{

        //    SetScriptingDefineSymbols();
        //    if (initscript.enableGoogleMobileAds)
        //    {
        //        var googleAds = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GoogleAds"));
        //        googleAds.name = "GoogleAds";
        //    }
        //    else
        //    {
        //        DestroyImmediate(GameObject.Find("GoogleAds"));
        //    }
        //}

        //if (initscript.enableGoogleMobileAds)
        //{
        //    string txt = "if (GoogleAds.main)\n" +
        //    "{\n" +
        //    "   GoogleAds.main.RequestBanner();\n" +
        //    "   GoogleAds.main.RequestInterstitial();\n" +
        //    "   GoogleAds.main.ShowBanner();\n" +
        //    "   GoogleAds.main.ShowInterstitial();\n" +
        //    "}\n" +
        //    "PS: Add this script to OnEnable in GUIController";
        //    GUILayout.TextField(txt, 1000, new GUILayoutOption[] { GUILayout.Width(450), GUILayout.Height(120) });
        //}

        //isMoreAds = GUILayout.Toggle(isMoreAds, "More Ads", new GUILayoutOption[]{ GUILayout.Width(100)});
        //if (isMoreAds && false)
        //{
        //    //UNITY ADS
        //    bool oldenableAds = initscript.enableUnityAds;
        //    GUILayout.BeginHorizontal();
        //    initscript.enableUnityAds = EditorGUILayout.Toggle("Enable Unity ads", initscript.enableUnityAds, new GUILayoutOption[] {
        //    GUILayout.Width (200)
        //});
        //    GUILayout.Label("Install: Windows->\n Services->Ads - ON", new GUILayoutOption[] { GUILayout.Width(130) });
        //    if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        //    {
        //        Application.OpenURL("https://docs.google.com/document/d/1HeN8JtQczTVetkMnd8rpSZp_TZZkEA7_kan7vvvsMw0");
        //    }

        //    GUILayout.EndHorizontal();

        //    GUILayout.Space(10);

        //    if (oldenableAds != initscript.enableUnityAds)
        //        SetScriptingDefineSymbols();
        //    if (initscript.enableUnityAds)
        //    {
        //        GUILayout.BeginHorizontal();
        //        GUILayout.Space(20);
        //        initscript.rewardedGems = EditorGUILayout.IntField("Rewarded gems", initscript.rewardedGems, new GUILayoutOption[] {
        //        GUILayout.Width (200),
        //        GUILayout.MaxWidth (200)
        //    });
        //        GUILayout.EndHorizontal();
        //        GUILayout.Space(10);
        //    }

        //    //CHARTBOOST ADS

        //    GUILayout.BeginHorizontal();
        //    bool oldenableChartboostAds = initscript.enableChartboostAds;
        //    initscript.enableChartboostAds = EditorGUILayout.Toggle("Enable Chartboost Ads", initscript.enableChartboostAds, new GUILayoutOption[] {
        //    GUILayout.Width (50),
        //    GUILayout.MaxWidth (200)
        //});
        //    if (GUILayout.Button("Install", new GUILayoutOption[] { GUILayout.Width(100) }))
        //    {
        //        Application.OpenURL("http://cboo.st/unity_v6-3-0");
        //    }
        //    if (GUILayout.Button("Help", new GUILayoutOption[] { GUILayout.Width(80) }))
        //    {
        //        Application.OpenURL("https://docs.google.com/document/d/1ibnQbuxFgI4izzyUtT45WH5m1ab3R5d1E3ke3Wrb10Y");
        //    }

        //    GUILayout.EndHorizontal();

        //    GUILayout.Space(10);
        //    if (oldenableChartboostAds != initscript.enableChartboostAds)
        //    {
        //        SetScriptingDefineSymbols();
        //        if (initscript.enableChartboostAds)
        //        {
                    
        //        }

        //    }
        //    if (initscript.enableChartboostAds)
        //    {
        //        GUILayout.BeginHorizontal();
        //        GUILayout.Space(20);
        //        EditorGUILayout.LabelField("menu Chartboost->Edit settings", new GUILayoutOption[] {
        //        GUILayout.Width (50),
        //        GUILayout.MaxWidth (200)
        //    });
        //        GUILayout.EndHorizontal();
        //        GUILayout.BeginHorizontal();
        //        GUILayout.Space(20);
        //        EditorGUILayout.LabelField("Put ad ID to appropriate platform to prevent crashing!", EditorStyles.boldLabel, new GUILayoutOption[] {
        //        GUILayout.Width (100),
        //        GUILayout.MaxWidth (400)
        //    });
        //        GUILayout.EndHorizontal();

        //        GUILayout.Space(10);
        //    }

            

        //}

        //GUILayout.Space(10);

        //GUILayout.Label("Ads controller:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });

        //EditorGUILayout.Space();

        //GUILayout.Label("Event:               Status:                            Show every:", new GUILayoutOption[] { GUILayout.Width(350) });


        //foreach (AdEvents item in initscript.adsEvents)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    item.gameEvent = (GameState)EditorGUILayout.EnumPopup(item.gameEvent, new GUILayoutOption[] { GUILayout.Width(100) });
        //    item.adType = (AdType)EditorGUILayout.EnumPopup(item.adType, new GUILayoutOption[] { GUILayout.Width(150) });
        //    item.everyLevel = EditorGUILayout.IntPopup(item.everyLevel, new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new GUILayoutOption[] { GUILayout.Width(100) });

        //    EditorGUILayout.EndHorizontal();

        //}

        //EditorGUILayout.Space();

        //EditorGUILayout.BeginHorizontal();
        //if (GUILayout.Button("Add"))
        //{
        //    AdEvents adevent = new AdEvents();
        //    adevent.everyLevel = 1;
        //    initscript.adsEvents.Add(adevent);
        //}
        //if (GUILayout.Button("Delete"))
        //{
        //    if (initscript.adsEvents.Count > 0)
        //        initscript.adsEvents.Remove(initscript.adsEvents[initscript.adsEvents.Count - 1]);
        //}
        //GUILayout.Space(10);
    }
    #endregion

    #region GUIShowDialogs
    void GUIShowDialogs()
    {
        //Debug.LogWarning("Show Gui Dialogs");
        // write something here

        GUIEvents ge = Camera.main.GetComponent<GUIEvents>();
        ge.guiControllers.Clear();

        // find gui
        GameObject obj = GameObject.Find("CanvasGlobal").transform.Find(name).gameObject;
        if (obj == null)
            return;
        GUILayout.Label("GUI elements:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
        
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        typeGui = (TypeGui)EditorGUILayout.EnumPopup(typeGui, new GUILayoutOption[] { GUILayout.Width(100) });
        if (GUILayout.Button("Add GUI", new GUILayoutOption[] { GUILayout.Width(100) }))
        {
       
            //Debug.Log(typeGui.ToString());
            
            string guiName = "GUI_" + typeGui.ToString();
            if (guiPrefabs.ContainsKey(guiName) == false)
            {
                guiPrefabs.Add(guiName, Resources.Load<GameObject>("Prefabs/" + guiName));
            }

            var gui = Instantiate<GameObject>(guiPrefabs[guiName]);
            gui.transform.SetParent(obj.transform);
            gui.transform.SetSiblingIndex(obj.transform.childCount);
            
            gui.transform.localPosition = Vector3.zero;
            //gui.GetComponent<RectTransform>().anchoredPosition = guiPrefabs[guiName].GetComponent<RectTransform>().anchoredPosition;
            //gui.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, guiPrefabs[guiName].GetComponent<RectTransform>().rect.width);
            //gui.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, guiPrefabs[guiName].GetComponent<RectTransform>().rect.height);
            gui.transform.localScale = Vector3.one;
            gui.name = guiName + "_" + obj.transform.childCount.ToString();

            GUIController gc = gui.GetComponent<GUIController>();
            gc.AddShowComponent(gc.typeShow);

        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.Label("Gui Name:          Action:                Show/Hide:", new GUILayoutOption[] { GUILayout.Width(350) });

        GUILayout.Space(10);
        GUILayout.BeginVertical();
        int counChild = obj.transform.childCount;
        for (int i = 0; i < counChild; i++)
        {
            if (i < obj.transform.childCount)
            {
                var child = obj.transform.GetChild(i);
                if (child == null)
                    return;
                ge.guiControllers.Add(child.GetComponent<GUIController>());
                ShowMenuButton(child.name, child.name);
               
                
                //Debug.Log("List: " + ge.guiControllers.Count);
            }
        }
        GUILayout.EndVertical();
    }
    #endregion

    void ShowMenuButton(string label, string name)
    {
        GameObject obj = GameObject.Find("CanvasGlobal").transform.Find(name).gameObject;
        if (obj == null)
            return;

        GUILayout.BeginHorizontal();
            GUIController gc = obj.GetComponent<GUIController>();
            if (gc == null)
                return;
            if (GUILayout.Button(gc.isSpread ? "-" : "+", new GUILayoutOption[] { GUILayout.Width(20) }))
            {
                gc.isSpread = !gc.isSpread;
            }
            GUILayout.Label(label, EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(100) });
            var oldTypeShow = gc.typeShow;

            gc.typeShow = (TypeShow)EditorGUILayout.EnumPopup(gc.typeShow, new GUILayoutOption[] { GUILayout.Width(100) });
            if (gc.typeShow != oldTypeShow)
            {
                gc.DestroyShowComponent(oldTypeShow);
                gc.AddShowComponent(gc.typeShow);
            }

            if (GUILayout.Button(obj.activeSelf ? "Hide" : "Show", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                EditorGUIUtility.PingObject(obj);
                Selection.activeGameObject = obj;
                obj.SetActive(!obj.activeSelf);
            }
            if (GUILayout.Button("Delete", new GUILayoutOption[] { GUILayout.Width(100) }))
            {
                DestroyImmediate(obj);
            }

            
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        if (obj == null)
            return;
        var img = obj.transform.Find("Image").gameObject;
        if (img == null)
            return;
        if (!gc.isSpread)
        {
            switch (gc.typeShow)
            {
                #region TweenPosition
                case TypeShow.TweenPosition:

                    var pos = img.GetComponent<TweenPosition>();
                    if (pos == null)
                        return;
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Start Value", new GUILayoutOption[] { (GUILayout.Width(100)) }))
                    {
                        pos.SetStartToCurrentValue();
                    }

                    if (GUILayout.Button("Assume Start Value", new GUILayoutOption[] { (GUILayout.Width(150)) }))
                    {
                        pos.SetCurrentValueToStart();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("X: " + pos.from.x.ToString() + "          " + "Y: " + pos.from.y.ToString() + "          " + "Z: " + pos.from.z.ToString());

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("End Value", new GUILayoutOption[] { (GUILayout.Width(100)) }))
                    {
                        pos.SetEndToCurrentValue();
                    }
                    if (GUILayout.Button("Assume End Value", new GUILayoutOption[] { (GUILayout.Width(150)) }))
                    {
                        pos.SetCurrentValueToEnd();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("X: " + pos.to.x.ToString() + "          " + "Y: " + pos.to.y.ToString() + "          " + "Z: " + pos.to.z.ToString());

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button("Reset To Default", new GUILayoutOption[] { GUILayout.Width(300) }))
                    {
                        pos.from = Vector3.zero;
                        pos.to = Vector3.zero;
                    }
                    break;
                #endregion
                #region TweenScale
                case TypeShow.TweenScale:
                    var scale = img.GetComponent<TweenScale>();
                    if (scale == null)
                        return;
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(50);
                    GUILayout.BeginVertical();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Start Value", new GUILayoutOption[] { (GUILayout.Width(100)) }))
                    {
                        scale.SetStartToCurrentValue();
                    }

                    if (GUILayout.Button("Assume Start Value", new GUILayoutOption[] { (GUILayout.Width(150)) }))
                    {
                        scale.SetCurrentValueToStart();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("X: " + scale.from.x.ToString() + "          " + "Y: " + scale.from.y.ToString() + "          " + "Z: " + scale.from.z.ToString());

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("End Value", new GUILayoutOption[] { (GUILayout.Width(100)) }))
                    {
                        scale.SetEndToCurrentValue();
                    }
                    if (GUILayout.Button("Assume End Value", new GUILayoutOption[] { (GUILayout.Width(150)) }))
                    {
                        scale.SetCurrentValueToEnd();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Label("X: " + scale.to.x.ToString() + "          " + "Y: " + scale.to.y.ToString() + "          " + "Z: " + scale.to.z.ToString());

                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button("Reset To Default", new GUILayoutOption[] { GUILayout.Width(300) }))
                    {
                        scale.from = Vector3.one;
                        scale.to = Vector3.one;
                    }
                    break;
                #endregion
                #region FadeInOut
                case TypeShow.FadeInOut:
                    var cg = img.GetComponent<CanvasGroup>();
                    if (cg == null)
                        return;
                    var fade = img.GetComponent<FadeInOut>();
                    if (cg == null)
                        return;
                    GUILayout.BeginHorizontal();
                    fade.typeFade = (TypeFade)EditorGUILayout.EnumPopup(fade.typeFade, new GUILayoutOption[] { GUILayout.Width(150) });
                    if (GUILayout.Button("Default", new GUILayoutOption[] { GUILayout.Width(100) }))
                    {
                        cg.alpha = 1;
                        cg.blocksRaycasts = true;
                        cg.interactable = true;
                        cg.ignoreParentGroups = false;
                    }
                    GUILayout.EndHorizontal();
                    cg.alpha = (TypeFade.In == fade.typeFade) ? 1 : 0;
                    obj.SetActive((TypeFade.In == fade.typeFade) ? true : false);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Alpha", new GUILayoutOption[] { GUILayout.MaxWidth(200) });
                    GUILayout.Label(cg.alpha.ToString());
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Interactable", new GUILayoutOption[] { GUILayout.MaxWidth(200) });
                    cg.interactable = EditorGUILayout.Toggle("", cg.interactable, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.MaxWidth(200) });
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Blocks Raycast", new GUILayoutOption[] { GUILayout.MaxWidth(200) });
                    cg.blocksRaycasts = EditorGUILayout.Toggle("", cg.blocksRaycasts, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.MaxWidth(200) });
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Ignore Parent Groups", new GUILayoutOption[] { GUILayout.MaxWidth(200) });
                    cg.ignoreParentGroups = EditorGUILayout.Toggle("", cg.ignoreParentGroups, new GUILayoutOption[] { GUILayout.Width(50), GUILayout.MaxWidth(200) });
                    GUILayout.EndHorizontal();

                    
                    break;
                #endregion
            }
        }
    }

    #region GUIShowRate
    void GUIShowRate()
    {
        //Debug.LogWarning("Show Gui Rate");
        // write something here
        //InitScript initscript = Camera.main.GetComponent<InitScript>();

        GUILayout.Label("Rate settings:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(150) });
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        //initscript.ShowRateEvery = EditorGUILayout.IntField("Show Rate every ", initscript.ShowRateEvery, new GUILayoutOption[] {
        int ShowRateEvery = 0;
        ShowRateEvery = EditorGUILayout.IntField("Show Rate every ", ShowRateEvery, new GUILayoutOption[] {
            GUILayout.Width (220),
            GUILayout.MaxWidth (220)
        });
        GUILayout.Label(" level (0 = disable)", EditorStyles.label, new GUILayoutOption[] { GUILayout.Width(150) });
        GUILayout.EndHorizontal();
        string RateURL = "";
        RateURL = EditorGUILayout.TextField("URL", RateURL, new GUILayoutOption[] {
            GUILayout.Width (220),
            GUILayout.MaxWidth (220)
        });
    }
    #endregion

    #region GUIShowHelp
    void GUIShowHelp()
    {
        //Debug.LogWarning("Show Gui Help");
        // write something here
        GUILayout.Label("Please read our documentation:", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(200) });
        if (GUILayout.Button("DOCUMENTATION", new GUILayoutOption[] { GUILayout.Width(150) }))
        {
            Application.OpenURL("https://docs.google.com/document/d/17QwNYwZysylZUvRcjLWZU-IaJPNynaAJ3Ds-JafhtMA/edit");
        }
        GUILayout.Space(10);
        GUILayout.Label("To get support you should provide \n ORDER NUMBER (asset store) \n or NICKNAME and DATE of purchase (other stores):", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(350) });
        GUILayout.Space(10);
        GUILayout.TextArea("best2dgames@icloud.com", EditorStyles.boldLabel, new GUILayoutOption[] { GUILayout.Width(350) });
    }
    #endregion

    
}
