//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenTransform))]
public class TweenTransformEditor : UITweenerEditor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Space(6f);
        EditorTools.SetLabelWidth(120f);

        TweenTransform tw = target as TweenTransform;
        GUI.changed = false;
        
        RectTransform from = EditorGUILayout.ObjectField("From", tw.from, typeof(RectTransform), true) as RectTransform;
        RectTransform to = EditorGUILayout.ObjectField("To", tw.to, typeof(RectTransform), true) as RectTransform;
        bool parentWhenFinished = EditorGUILayout.Toggle("Parent When Finished", tw.parentWhenFinished);

        if (GUI.changed)
        {
            EditorTools.RegisterUndo("Tween Change", tw);
            tw.from = from;
            tw.to = to;
            tw.parentWhenFinished = parentWhenFinished;
            EditorTools.SetDirty(tw);
        }

        DrawCommonProperties();
    }
}
