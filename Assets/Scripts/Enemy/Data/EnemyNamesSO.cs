using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyNames", menuName = "ScriptableObjects/EnemyNames")]
public class EnemyNamesSO : ScriptableObject
{
    public List<string> names;
}
