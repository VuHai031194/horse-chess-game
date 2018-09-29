//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UITweener), true)]
[CanEditMultipleObjects]
public class UITweenerEditor : Editor
{
    SerializedProperty m_OnFinishedProperty;
    protected virtual void OnEnable()
    {
        m_OnFinishedProperty = serializedObject.FindProperty("m_OnFinished");
    }

	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
        EditorTools.SetLabelWidth(110f);
		base.OnInspectorGUI();
		DrawCommonProperties();
	}

	protected void DrawCommonProperties ()
	{
		UITweener tw = target as UITweener;

        if (EditorTools.DrawHeader("Tweener"))
		{
            EditorTools.BeginContents();
            EditorTools.SetLabelWidth(110f);

			GUI.changed = false;

			UITweener.Style style = (UITweener.Style)EditorGUILayout.EnumPopup("Play Style", tw.style);
			AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.animationCurve, GUILayout.Width(170f), GUILayout.Height(62f));
            UITweener.Method method = (UITweener.Method)EditorGUILayout.EnumPopup("Play Method", tw.method);

			GUILayout.BeginHorizontal();
			float dur = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			float del = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(170f));
			GUILayout.Label("seconds");
			GUILayout.EndHorizontal();

			int tg = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(170f));
			bool ts = EditorGUILayout.Toggle("Ignore TimeScale", tw.ignoreTimeScale);

			if (GUI.changed)
			{
                EditorTools.RegisterUndo("Tween Change", tw);
				tw.animationCurve = curve;
                tw.method = method;
				tw.style = style;
				tw.ignoreTimeScale = ts;
				tw.tweenGroup = tg;
				tw.duration = dur;
				tw.delay = del;
                UnityEditor.EditorUtility.SetDirty(tw);
			}
            EditorTools.EndContents();
		}

        EditorTools.SetLabelWidth(80f);

        EditorGUILayout.PropertyField(m_OnFinishedProperty);
	}
}
