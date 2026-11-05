using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    public PlayerInput.OnFootActions OnFoot => onFoot;
    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump(); // kdykolik je onfoot.jump performovano  pouzivame call back kontext(ctx) a volame motor.jump funkci
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rekne motoru aby se pohnul podle inputu z input manageru
        if (motor != null)
        {
            motor.SetCrouch(onFoot.Crouch.ReadValue<float>() > 0.5f);
            motor.SetSprint(onFoot.Sprint.ReadValue<float>() > 0.5f);
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }
    void LateUpdate()
    {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>()); //rekne looku aby se pohnul podle inputu z input manageru
        
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
