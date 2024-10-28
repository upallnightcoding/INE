using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTileCntrl : MonoBehaviour
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;

    public bool IsRuneTile { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3 MakeRuneTile()
    {
        IsRuneTile = true;
        gameObject.SetActive(false);
        return (transform.position);
    }
}
