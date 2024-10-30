using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrailFollow : MonoBehaviour
{
    [SerializeField] private float distance = 10.0f;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();

            Vector3 position = cam.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, distance));

            transform.position = position;
        }
    }
}
