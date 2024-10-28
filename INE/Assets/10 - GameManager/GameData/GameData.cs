using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Create/GameData")]
public class GameData : ScriptableObject
{
    [Header("Camera Controls ...")]
    public float cameraDamping = 2.0f;

    [Header("List of enemies ...")]
    public EnemySO[] enemy;

    [Space]
    [Header("Weapons Attributes ...")]
    public float weaponsSpinning = 30.0f;
}
