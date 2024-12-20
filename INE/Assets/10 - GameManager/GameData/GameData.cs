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

    [Header("Environment Tiles ...")]
    public GameObject[] runeTiles;
    public GameObject[] rails;
    public GameObject basicFloorTile;
    public GameObject[] grassTiles;
    public GameObject[] mushRoomTiles;
    public GameObject[] statueTiles;

    public int nGrassTiles = 125;
    public int nMushRooms = 10;
    public int nStatueTiles = 5;
}
