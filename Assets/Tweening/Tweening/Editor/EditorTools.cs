using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

public static class EditorTools
{
    /// <summary>
    /// Unity 4.3 changed the way LookLikeControls works.
    /// </summary>

    static public void SetLabelWidth(float width)
    {
        EditorGUIUtility.labelWidth = width;
    }
    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    static public void BeginContents() { BeginContents(minimalisticLook); }

    static bool mEndHorizontal = false;

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>
    static public void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>
    static public void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }


    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text) { return DrawHeader(text, text, false, minimalisticLook); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, minimalisticLook); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    static public bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    /// <summary>
    /// Draw 18 pixel padding on the right-hand side. Used to align fields.
    /// </summary>

    static public void DrawPadding()
    {
        if (!minimalisticLook)
            GUILayout.Space(18f);
    }

    static public bool minimalisticLook
    {
        get { return GetBool("NGUI Minimalistic", false); }
        set { SetBool("NGUI Minimalistic", value); }
    }

    /// <summary>
    /// Create an undo point for the specified objects.
    /// </summary>

    static public void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }
    /// <summary>
    /// Get the previously saved boolean value.
    /// </summary>
    static public bool GetBool(string name, bool defaultValue) { return EditorPrefs.GetBool(name, defaultValue); }
    /// <summary>
    /// Save the specified boolean value in settings.
    /// </summary>
    static public void SetBool(string name, bool val) { EditorPrefs.SetBool(name, val); }
    /// <summary>
    /// Convenience function that marks the specified object as dirty in the Unity Editor.
    /// </summary>

    static public void SetDirty(UnityEngine.Object obj)
    {
		if (obj)
		{
			//if (obj is Component) Debug.Log(NGUITools.GetHierarchy((obj as Component).gameObject), obj);
			//else if (obj is GameObject) Debug.Log(NGUITools.GetHierarchy(obj as GameObject), obj);
			//else Debug.Log("Hmm... " + obj.GetType(), obj);
			UnityEditor.EditorUtility.SetDirty(obj);
		}
    }

    static public void CreateHandles(MonoBehaviour _target, Vector3[] arr, GUIStyle style, string prefix)
    {
        if (arr.Length <= 0)
            return;

        for (int i = 0; i < arr.Length; i++)
        {
            Vector3 v = _target.transform.TransformPoint(arr[i]);
            Handles.Label(v, string.Format("{0} {1}", prefix, i), style);
            v = Handles.PositionHandle(v, Quaternion.identity);
            arr[i] = _target.transform.InverseTransformPoint(v);
        }
    }

    static public void CreateHandles(MonoBehaviour _target, ref Vector3 vec, GUIStyle style, string name)
    {
        Handles.Label(_target.transform.TransformPoint(vec), name, style);
        CreateHandles(_target, ref vec);
    }

    static public void CreateHandles(MonoBehaviour _target, ref Vector3 vec)
    {
        Vector3 v = _target.transform.TransformPoint(vec);
        v = Handles.PositionHandle(v, Quaternion.identity);
        vec = _target.transform.InverseTransformPoint(v);
    }
}
