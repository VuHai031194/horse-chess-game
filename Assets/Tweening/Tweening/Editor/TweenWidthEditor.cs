//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenWidth))]
public class TweenWidthEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
        EditorTools.SetLabelWidth(120f);

		TweenWidth tw = target as TweenWidth;
		GUI.changed = false;

        float from = EditorGUILayout.FloatField("From", tw.from);
        float to = EditorGUILayout.FloatField("To", tw.to);
        //bool table = EditorGUILayout.Toggle("Update Table", tw.updateTable);

		if (from < 0) from = 0;
		if (to < 0) to = 0;

		if (GUI.changed)
		{
            EditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
            //tw.updateTable = table;
            EditorTools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
