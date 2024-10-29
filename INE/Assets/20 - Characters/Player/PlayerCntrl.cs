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

    private bool runGamePlay = false;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private Vector3 target;

    private WeaponSO holdWeapon;

    private PlayerState playerState = PlayerState.FIRING;

    public void StartGamePlay() => runGamePlay = true;

    

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
        if (runGamePlay)
        {
            GamePlay();
        }
    }

    private void GamePlay()
    {
        ClickCommands();
        ClickAndMove(Time.deltaTime);

        switch (playerState)
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

    private void ClickCommands()
    {
        if(Keyboard.current.qKey.wasPressedThisFrame)
        {
            EndGamePlay();
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            EventManager.Instance.InvokeOnHintDisplay();
        }
    }

    private void EndGamePlay()
    {
        runGamePlay = false;

        EventManager.Instance.InvokeOnEndGame();
        EventManager.Instance.InvokeOnDisplayMainMenu();
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

    /**
     * DropWeapon() - When a weapon has been taken from a rune, it must be
     * bound to a "1", "2" or "3" key.  After binding the key can then be
     * used as a trigger for shooting.
     */
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

    /**
     * SetNewWeapon() - 
     */
    private void SetNewWeapon(int slot)
    {
        weaponSlot[slot].Set(holdWeapon);
        EventManager.Instance.InvokeOnSetWeapon(slot, holdWeapon.sprite, holdWeapon.maxRounds);
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
        if (Keyboard.current.digit1Key.wasPressedThisFrame) StartCoroutine(StartFiring(0, weaponSlot[0]));
        if (Keyboard.current.digit1Key.wasReleasedThisFrame) StopFiring(weaponSlot[0]);

        if (Keyboard.current.digit2Key.wasPressedThisFrame) StartCoroutine(StartFiring(1, weaponSlot[1]));
        if (Keyboard.current.digit2Key.wasReleasedThisFrame) StopFiring(weaponSlot[1]);

        if (Keyboard.current.digit3Key.wasPressedThisFrame) StartCoroutine(StartFiring(2, weaponSlot[2]));
        if (Keyboard.current.digit3Key.wasReleasedThisFrame) StopFiring(weaponSlot[2]);
    }

    private IEnumerator StartFiring(int slot, WeaponSlot weaponSlot)
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

                EventManager.Instance.InvokeOnWeaponUpdate(slot, weaponSlot.Rounds, weaponSlot.MaxRounds);

                firstShot = false;
            }

            if (weaponSlot.IsWeaponEmpty())
            {
                float timing = 0.0f;
                float reloadTime = weaponSlot.GetReloadTime();

                while (timing < reloadTime)
                {
                    timing += Time.deltaTime;
                    EventManager.Instance.InvokeOnWeaponReload(slot, timing / reloadTime);
                    yield return null;
                }

                weaponSlot.Reload();
                int maxRounds = weaponSlot.MaxRounds;
                EventManager.Instance.InvokeOnWeaponUpdate(slot, maxRounds, maxRounds);
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

        public int Rounds { get; set; }
        public int MaxRounds { get; set; }

        private int projectilePerClick;

        public WeaponSlot()
        {

        }

        public void Set(WeaponSO weapon)
        {
            this.weapon = weapon;
            this.Rounds = weapon.maxRounds;
            this.MaxRounds = weapon.maxRounds;
            this.projectilePerClick = weapon.projectilesPerClick;

            this.DoesSlotHaveWeapon = true;
        }

        public void InitializeFiring()
        {
            IsFiring = true;
            EndAutomatic = false;
        }

        public void EndFiring()
        {
            EndAutomatic = true;
            IsFiring = false;
        }

        public bool CanFire(bool firstShot)
        {
            int currentRounds = Rounds;
            return (((weapon.manual && firstShot) || (!weapon.manual && !EndAutomatic)) && (Rounds-- > 0));
        }

        public void FireWeapon(Vector3 muzzlePoint, Vector3 direction)
        {
            switch(projectilePerClick)
            {
                case 1:
                    RenderProjectile(muzzlePoint, direction, 0.0f);
                    break;
                case 2:
                    Fire2Projectiles(muzzlePoint, direction);
                    break;
                case 3:
                    Fire3Projectiles(muzzlePoint, direction);
                    break;
                case 4:
                    Fire4Projectiles(muzzlePoint, direction);
                    break;
                case 5:
                    Fire5Projectiles(muzzlePoint, direction);
                    break;
            }
        }

        /**
         * Fire2Projectiles() - Fires projectiles in two directions based on 
         * the muzzle point and the direction.
         */
        private void Fire2Projectiles(Vector3 muzzlePoint, Vector3 direction)
        {
            Vector3 d1 = (Vector3.Cross(direction, Vector3.up) + direction).normalized;
            Vector3 d2 = (-Vector3.Cross(direction, Vector3.up) + direction).normalized;

            RenderProjectile(muzzlePoint, d1, 0.5f);
            RenderProjectile(muzzlePoint, d2, 0.5f);
        }

        /**
        * Fire3Projectiles() - Fires projectiles in three directions based on 
        * the muzzle point and the direction.
        */
        private void Fire3Projectiles(Vector3 muzzlePoint, Vector3 direction)
        {
            RenderProjectile(muzzlePoint, direction, 0.0f);

            Vector3 d1 = (Vector3.Cross(direction, Vector3.up) + direction).normalized;
            Vector3 d2 = (-Vector3.Cross(direction, Vector3.up) + direction).normalized;

            RenderProjectile(muzzlePoint, d1, 0.5f);
            RenderProjectile(muzzlePoint, d2, 0.5f);
        }

        /**
        * Fire4Projectiles() - Fires projectiles in four directions based on 
        * the muzzle point and the direction.
        */
        private void Fire4Projectiles(Vector3 muzzlePoint, Vector3 direction)
        {
            Vector3 u = Vector3.Cross(direction, Vector3.up);

            Vector3 d1 = (u + direction);
            Vector3 d2 = (-u + direction);

            RenderProjectile(muzzlePoint, (d1 +  u).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (d1 + direction).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (d2 + -u).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (d2 + direction).normalized, 0.5f);
        }

        /**
       * Fire4Projectiles() - Fires projectiles in four directions based on 
       * the muzzle point and the direction.
       */
        private void Fire5Projectiles(Vector3 muzzlePoint, Vector3 direction)
        {
            RenderProjectile(muzzlePoint, direction, 0.0f);

            Vector3 u = Vector3.Cross(direction, Vector3.up);

            Vector3 d1 = (u + direction);
            Vector3 d2 = (-u + direction);

            RenderProjectile(muzzlePoint, (u + d1).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (d1 + direction).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (direction + d2).normalized, 0.5f);
            RenderProjectile(muzzlePoint, (d2 + -u).normalized, 0.5f);
        }

        private void RenderProjectile(Vector3 startPoint, Vector3 direction, float offset)
        {
            GameObject go = Instantiate(weapon.prefab, startPoint + direction*offset, Quaternion.identity);
            go.GetComponentInChildren<Rigidbody>().AddForce(direction * weapon.force, ForceMode.Impulse);
            go.GetComponent<ProjectileCntrl>().SetDamage(weapon.damage, weapon.destroyPrefab);
            Destroy(go, weapon.destroyTiming);
        }

        public bool IsWeaponEmpty()
        {
            return (Rounds <= 0);
        }

        public void Reload()
        {
            Rounds = weapon.maxRounds;
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

