using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCntrl : MonoBehaviour
{
    private Animator animator;

    float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("speed", 0.0f);
        speed = 0.0f;

        Invoke("StartEnemyMoving", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void StartEnemyMoving()
    {
        animator.SetFloat("speed", 1.0f);
        speed = 0.7f;
    }
}
