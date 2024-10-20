using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponSO weapon;

    private CharacterController charCntrl;
    private Animator animator;

    private float playerSpeed = 5.0f;
    private Vector3 direction;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        animator.SetFloat("speed", 0.0f);

        direction = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        ClickAndMove(Time.deltaTime);
        FireKey();
    }

    private void ClickAndMove(float dt)
    {
        float throttle = 1.0f;
        float speed = 5.0f;

        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target = new Vector3(hit.point.x, 0.0f, hit.point.z);

                if (Vector3.Distance(transform.position, target) > 0.3f)
                {
                    direction = (target - transform.position).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    Quaternion playerRotation = targetRotation;
                    //Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 25.0f * dt);
                    transform.localRotation = playerRotation;

                    transform.Translate(transform.forward * speed * throttle * dt, Space.World);

                    animator.SetFloat("speed", 0.9f);
                } else
                {
                    animator.SetFloat("speed", 0.0f);
                }
            }
        } else
        {
            if (Vector3.Distance(transform.position, target) > 0.3f)
            {
                transform.Translate(transform.forward * speed * throttle * dt, Space.World);
            } else
            {
                animator.SetFloat("speed", 0.0f);
            }
        }
    }

    private void FireKey()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            GameObject go = Instantiate(weapon.prefab, firePoint.transform.position, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(direction * 90.0f, ForceMode.Impulse);
            Debug.DrawLine(firePoint.transform.position, firePoint.transform.position + direction * 10.0f, Color.red) ;
            Destroy(go, 2.0f);
        }

        if (Keyboard.current.digit1Key.wasReleasedThisFrame)
        {
           
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            //Debug.Log("Fire 2 ... Start");
        }

        if (Keyboard.current.digit2Key.wasReleasedThisFrame)
        {
            //Debug.Log("Fire 2 ... Canceled");
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            //Debug.Log("Fire 3 ... Start");
        }

        if (Keyboard.current.digit3Key.wasReleasedThisFrame)
        {
            //Debug.Log("Fire 3 ... Canceled");
        }
    }
}
