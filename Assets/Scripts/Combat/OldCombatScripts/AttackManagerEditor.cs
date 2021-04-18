using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(AttackManager))]
public class AttackManagerEditor : Editor
{
    //override public void OnInspectorGUI()
    //{
    //    AttackManager attackScript = (AttackManager)target;

    //    attackScript.areaOfEffect = GUILayout.Toggle(attackScript.areaOfEffect, "AreaOfEffect");

    //    if (attackScript.areaOfEffect)
    //    {
    //        attackScript.areaOfEffectRadius = EditorGUILayout.FloatField("I field:", attackScript.areaOfEffectRadius);
    //    }


    //    attackScript.cleve = GUILayout.Toggle(attackScript.areaOfEffect, "Flag");

    //    if (attackScript.cleve)
    //    {
    //        attackScript.cleveRange = EditorGUILayout.FloatField("I field:", attackScript.cleveRange);
    //    }

    //    attackScript.bounce = GUILayout.Toggle(attackScript.areaOfEffect, "Flag");

    //    if (attackScript.bounce)
    //    {
    //        attackScript.bounceRange = EditorGUILayout.FloatField("I field:", attackScript.bounceRange);
    //    }
    //}
}
#endif
