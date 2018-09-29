//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenSize))]
public class TweenSizeEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
        EditorTools.SetLabelWidth(120f);

        TweenSize tw = target as TweenSize;
		GUI.changed = false;

        Vector2 from = EditorGUILayout.Vector2Field("From", tw.from);
        Vector2 to = EditorGUILayout.Vector2Field("To", tw.to);

        if (from.x < 0) from.x = 0;
        if (from.y < 0) from.y = 0;
        if (to.x < 0) to.x = 0;
        if (to.y < 0) to.y = 0;

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
