//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the widget's size.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("Tween/Tween Size")]
public class TweenSize : UITweener
{
	public Vector2 from = new Vector2(100, 100);
    public Vector2 to = new Vector2(100, 100);

    RectTransform mTrans;

    public RectTransform cachedWidget { get { if (mTrans == null) mTrans = (RectTransform)transform; return mTrans; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

    public Vector2 value { get { return cachedWidget.sizeDelta; } set { cachedWidget.sizeDelta = value; } }

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = from * (1f - factor) + to * factor;
	}

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

    static public TweenSize Begin(RectTransform widget, float duration, Vector2 size)
	{
        TweenSize comp = UITweener.Begin<TweenSize>(widget.gameObject, duration);
        comp.from = widget.sizeDelta;
        comp.to = size;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = to; }
}
