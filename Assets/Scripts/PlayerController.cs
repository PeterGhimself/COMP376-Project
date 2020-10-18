using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_walkSpeed = 1f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;
    private Keyboard keyboard;
    private Mouse mouse;
    private CurrentGamepad currentGamepad;

    private const string k_attackAnim = "Attack";

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        currentGamepad = new CurrentGamepad();
    }

    void Update()
    {
        keyboard = Keyboard.current;
        mouse = Mouse.current;
        UpdateCharacterStates();
    }

    private void UpdateCharacterStates()
    {
        Walk();
        Attack();
    }

    private void Walk()
    {
        Vector2 wasd = new Vector2(-Convert.ToInt16(keyboard.aKey.isPressed) + Convert.ToInt16(keyboard.dKey.isPressed),
            Convert.ToInt16(keyboard.wKey.isPressed) - Convert.ToInt16(keyboard.sKey.isPressed));

        Vector2 walk = (currentGamepad.LeftStick().magnitude > 0 ? currentGamepad.LeftStick() : wasd).normalized * m_walkSpeed;

        transform.Translate(walk * Time.deltaTime);
    }

    private void Attack()
    {
        if(mouse.leftButton.IsPressed() || currentGamepad.IsPressed(GamepadButton.West))
        {
            m_animator.SetTrigger(k_attackAnim);
        }
    }

    private class CurrentGamepad
    {
        private Gamepad gamepad = default;

        public CurrentGamepad()
        {
            InputSystem.onDeviceChange +=
                (device, change) =>
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added:
                            gamepad = Gamepad.current;
                            UnityEngine.Debug.Log($"Device {device} was added");
                            break;
                        case InputDeviceChange.Removed:
                            gamepad = null;
                            UnityEngine.Debug.Log($"Device {device} was removed");
                            break;
                    }
                };

            gamepad = Gamepad.current;
        }

        public Vector2 LeftStick()
        {
            if (gamepad != null)
            {
                return gamepad.leftStick.ReadValue();
            }
            return Vector2.zero;
        }

        public bool IsPressed(GamepadButton button)
        {
            if (gamepad != null)
            {
                return gamepad[button].isPressed;
            }

            return false;
        }

        public bool WasReleased(GamepadButton button)
        {
            if (gamepad != null)
            {
                return gamepad[button].wasReleasedThisFrame;
            }

            return false;
        }
    }

}
