using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCntrl : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameData gameData;

    private Vector3 delta;
    private Vector3 velocity;

    private float damping;

    // Start is called before the first frame update
    void Start()
    {
        damping = gameData.cameraDamping;

        delta = player.position - transform.position;   
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 movePosition = player.position - delta;

        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
}
