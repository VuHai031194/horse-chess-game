//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenHeight))]
public class TweenHeightEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
        EditorTools.SetLabelWidth(120f);

		TweenHeight tw = target as TweenHeight;
		GUI.changed = false;

        float from = EditorGUILayout.FloatField("From", tw.from);
        float to = EditorGUILayout.FloatField("To", tw.to);

		if (from < 0) from = 0;
		if (to < 0) to = 0;

		if (GUI.changed)
		{
            EditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
            EditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
