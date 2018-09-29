//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's position.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("Tween/Tween Position")]
public class TweenPosition : UITweener
{
	public Vector3 from;
	public Vector3 to;

    RectTransform mTrans;

    public RectTransform cachedTransform { get { if (mTrans == null) mTrans = (RectTransform)transform; return mTrans; } }

	[System.Obsolete("Use 'value' instead")]
	public Vector3 position { get { return this.value; } set { this.value = value; } }

	/// <summary>
	/// Tween's current value.
	/// </summary>

	public Vector2 value
	{
		get
		{
            return cachedTransform.anchoredPosition;
		}
		set
		{
            cachedTransform.anchoredPosition = value;
		}
	}

	/// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) 
    {
        value = from * (1f - factor) + to * factor;
        if (isFinished && direction == Direction.Reverse && gameObject.name.Equals("Image"))
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenPosition Begin (GameObject go, float duration, Vector3 pos)
	{
		TweenPosition comp = UITweener.Begin<TweenPosition>(go, duration);
		comp.from = comp.value;
		comp.to = pos;

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
	public void SetCurrentValueToStart () { value = from; }

	[ContextMenu("Assume value of 'To'")]
	public void SetCurrentValueToEnd () { value = to; }
}
