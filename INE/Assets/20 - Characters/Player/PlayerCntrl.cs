using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponSO weapon;

    private WeaponStatus[] weaponStatus = new WeaponStatus[3];

    private CharacterController charCntrl;
    private Animator animator;

    private float playerSpeed = 5.0f;
    private Vector3 direction;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 target;

    private WeaponSO holdWeapon;

    private PlayerState playerState = PlayerState.FIRING;

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

        switch(playerState)
        {
            case PlayerState.FIRING:
                CheckFireWeapon();
                break;
            case PlayerState.PICK_WEAPON_SLOT:
                playerState = PickWeaponSlot();
                break;
            case PlayerState.KEY1:
            case PlayerState.KEY2:
            case PlayerState.KEY3:
                playerState = DropWeapon(playerState);
                break;
        }
    }

    public void Pickup(WeaponSO weapon)
    {
        holdWeapon = weapon;
        playerState = PlayerState.PICK_WEAPON_SLOT;
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

    /*********************/
    /*** Firing Weapon ***/
    /*********************/

    private PlayerState DropWeapon(PlayerState state)
    {
        switch(state)
        {
            case PlayerState.KEY1:
                break;
            case PlayerState.KEY2:
                break;
            case PlayerState.KEY3:
                break;
        }

        return (PlayerState.FIRING);
    }

    private PlayerState PickWeaponSlot()
    {
        PlayerState state = PlayerState.PICK_WEAPON_SLOT;

        if (Keyboard.current.digit1Key.wasPressedThisFrame) state = PlayerState.KEY1;
        if (Keyboard.current.digit2Key.wasPressedThisFrame) state = PlayerState.KEY2;
        if (Keyboard.current.digit3Key.wasPressedThisFrame) state = PlayerState.KEY3;

        return (state);
    }

    private void CheckFireWeapon()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            GameObject go = Instantiate(weapon.prefab, firePoint.transform.position, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(direction * 90.0f, ForceMode.Impulse);
            Destroy(go, 2.0f);
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame) StartFiring(0);
        if (Keyboard.current.digit1Key.wasReleasedThisFrame) StopFiring(0);

        if (Keyboard.current.digit2Key.wasPressedThisFrame) StartFiring(1);
        if (Keyboard.current.digit2Key.wasReleasedThisFrame) StopFiring(1);

        if (Keyboard.current.digit3Key.wasPressedThisFrame) StartFiring(2);
        if (Keyboard.current.digit3Key.wasReleasedThisFrame) StopFiring(2);
    }

    private void StartFiring(int weaponIndex)
    {

    }

    private void StopFiring(int weaponIndex)
    {

    }

    public class WeaponStatus
    {
        private bool weaponSet = false;
        private bool isFiring;
        private bool endAutomatic;
        private int maxRounds;
        private int rounds;
    }

    public enum PlayerState
    {
        FIRING,
        PICK_WEAPON_SLOT,
        KEY1,
        KEY2,
        KEY3
    }
}

