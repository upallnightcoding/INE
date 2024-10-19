using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCntrl : MonoBehaviour
{
    private CharacterController charCntrl;

    private float playerSpeed = 5.0f;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        charCntrl = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();   
    }

    private void MoveCharacter()
    {
        if (moveInput.magnitude > 0.3f)
        {
            moveDirection.x = moveInput.x;
            moveDirection.y = 0.0f;
            moveDirection.z = moveInput.y;

            charCntrl.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
