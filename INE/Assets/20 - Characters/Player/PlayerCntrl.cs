using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private WeaponSO weapon;

    private WeaponSlot[] weaponSlot = new WeaponSlot[3];

    private CharacterController charCntrl;
    private Animator animator;

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

        for (int i = 0; i < weaponSlot.Length; i++)
        {
            weaponSlot[i] = new WeaponSlot();
        }
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
                SetNewWeapon(0);
                break;
            case PlayerState.KEY2:
                SetNewWeapon(1);
                break;
            case PlayerState.KEY3:
                SetNewWeapon(2);
                break;
        }

        return (PlayerState.FIRING);
    }

    private void SetNewWeapon(int slot)
    {
        weaponSlot[slot].Set(holdWeapon);
        Debug.Log($"Drop Weapon Slot: {slot}");
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
        /*if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            GameObject go = Instantiate(weapon.prefab, firePoint.transform.position, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(direction * 90.0f, ForceMode.Impulse);
            Destroy(go, 2.0f);
        }*/

        if (Keyboard.current.digit1Key.wasPressedThisFrame) StartCoroutine(StartFiring(weaponSlot[0]));
        if (Keyboard.current.digit1Key.wasReleasedThisFrame) StopFiring(weaponSlot[0]);

        if (Keyboard.current.digit2Key.wasPressedThisFrame) StartCoroutine(StartFiring(weaponSlot[1]));
        if (Keyboard.current.digit2Key.wasReleasedThisFrame) StopFiring(weaponSlot[1]);

        if (Keyboard.current.digit3Key.wasPressedThisFrame) StartCoroutine(StartFiring(weaponSlot[2]));
        if (Keyboard.current.digit3Key.wasReleasedThisFrame) StopFiring(weaponSlot[2]);
    }

    private IEnumerator StartFiring(WeaponSlot weaponSlot)
    {
        if (weaponSlot != null && weaponSlot.DoesSlotHaveWeapon && !weaponSlot.IsFiring)
        {
            weaponSlot.InitializeFiring();

            bool firstShot = true;
            float timeBetweenRounds = 1.0f / weapon.roundsPerSec;

            while (weaponSlot.CanFire(firstShot))
            {
                weaponSlot.FireWeapon(muzzlePoint.position, transform.forward);

                yield return new WaitForSeconds(timeBetweenRounds);

                firstShot = false;
            }

            if (weaponSlot.IsWeaponEmpty())
            {
                yield return new WaitForSeconds(weaponSlot.GetReloadTime());
                weaponSlot.Reload();
            }

            weaponSlot.EndFiring();
        }
    }

    private void StopFiring(WeaponSlot weaponSlot)
    {
        if (weaponSlot != null && weaponSlot.DoesSlotHaveWeapon)
        {
            weaponSlot.StopAutomaticFiring();
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.OnRuneTrigger += Pickup;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnRuneTrigger -= Pickup;
    }

    /************************/
    /*** External Classes ***/
    /************************/

    public class WeaponSlot
    {
        private WeaponSO weapon;

        public bool DoesSlotHaveWeapon { get; set; } = false;
        public bool IsFiring { get; set; } = false;
        public bool EndAutomatic { get; set; } = true;

        private int rounds;

        public WeaponSlot()
        {

        }

        public void Set(WeaponSO weapon)
        {
            this.weapon = weapon;
            this.rounds = weapon.maxRounds;

            this.DoesSlotHaveWeapon = true;
        }

        public void InitializeFiring()
        {
            IsFiring = true;
            EndAutomatic = false;
        }

        public void EndFiring()
        {
            IsFiring = false;
        }

        public bool CanFire(bool firstShot)
        {
            return (((weapon.manual && firstShot) || (!weapon.manual && !EndAutomatic)) && (rounds-- > 0));
        }

        public void FireWeapon(Vector3 muzzlePoint, Vector3 direction)
        {
            GameObject go = Instantiate(weapon.prefab, muzzlePoint, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(direction * weapon.force, ForceMode.Impulse);
            Destroy(go, weapon.destroyTiming);
        }

        public bool IsWeaponEmpty()
        {
            return (rounds == 0);
        }

        public void Reload()
        {
            rounds = weapon.maxRounds;
        }

        public float GetReloadTime()
        {
            return (weapon.reloadSec);
        }

        public void StopAutomaticFiring()
        {
            EndAutomatic = true;
        }
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

