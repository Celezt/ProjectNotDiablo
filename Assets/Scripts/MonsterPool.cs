using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterPool", menuName = "ScriptableObjects/Monster Pool")]
public class MonsterPool : ScriptableObject
{
    public List<GameObject> monsters;
}
