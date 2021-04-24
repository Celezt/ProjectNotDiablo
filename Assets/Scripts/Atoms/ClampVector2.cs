using System;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityAtoms;

/// <summary>
/// An `AtomFunction&lt;Vector2, Vector2&gt;` that clamps the value using a min and a max value and returns it.
/// </summary>
[EditorIcon("atom-icon-sand")]
[CreateAssetMenu(menuName = "Unity Atoms/Functions/Custom/Clamp Vector2 (Vector2 => Vector2)", fileName = "ClampVector2")]
public class ClampVector2 : Vector2Vector2Function, IIsValid
{
    /// <summary>
    /// The minimum value.
    /// </summary>
    public Vector2Reference Min;

    /// <summary>
    /// The maximum value.
    /// </summary>
    public Vector2Reference Max;

    public override Vector2 Call(Vector2 value)
    {
        if (!IsValid()) { throw new Exception("Min value must be less than or equal to Max value."); }
        return new Vector2(Mathf.Clamp(value.x, Min.Value.x, Max.Value.x), Mathf.Clamp(value.y, Min.Value.y, Max.Value.y));
    }

    public bool IsValid()
    {
        return Min.Value.x <= Max.Value.x && Min.Value.y <= Min.Value.y;
    }
}
