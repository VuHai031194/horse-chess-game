//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenFOV))]
public class TweenFOVEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
        EditorTools.SetLabelWidth(120f);

		TweenFOV tw = target as TweenFOV;
		GUI.changed = false;

		float from = EditorGUILayout.Slider("From", tw.from, 1f, 180f);
		float to = EditorGUILayout.Slider("To", tw.to, 1f, 180f);

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
