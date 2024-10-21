using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Create/GameData")]
public class GameData : ScriptableObject
{
    public EnemySO[] enemy;
}
