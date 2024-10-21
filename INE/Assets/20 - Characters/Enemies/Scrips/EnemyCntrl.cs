using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCntrl : MonoBehaviour
{
    private GameObject player;

    private Animator animator;

    float speed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("speed", 0.0f);
        speed = 0.0f;

        Invoke("StartEnemyMoving", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 playerPos = player.transform.position;

            Vector3 target = new Vector3(playerPos.x, 0.0f, playerPos.z);
            Vector3 direction = (target - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //Quaternion playerRotation = targetRotation;
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 25.0f * Time.deltaTime);

            transform.localRotation = playerRotation;
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
    }

    private void StartEnemyMoving()
    {
        animator.SetFloat("speed", 1.0f);
        speed = 0.7f;
    }
}
