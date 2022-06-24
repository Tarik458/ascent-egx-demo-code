using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PilgrimController : MonoBehaviour
{

    public float moveSpeed = 2f;

    private Vector2 moveDirection;


    private PlayerControls controls;
    private PlayerControls Controls
    {
        get
        {
            if (controls != null)
            {
                return controls;
            }
            return controls = new PlayerControls();
        }
    }


    private void Awake()
    {
        Controls.Pilgrim.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        Controls.Pilgrim.Movement.canceled += ctx => OnMovement(ctx.ReadValue<Vector2>());
    }

    private void Update()
    {
        transform.Translate(new Vector3(moveDirection.x, 0f, moveDirection.y) * moveSpeed * Time.deltaTime);
    }


    /// <summary>
    /// Call when movement performed to set direction of travel.
    /// Call again when input cancelled to stop movement.
    /// </summary>
    /// <param name="direction"></param>
    private void OnMovement(Vector2 direction)
    {
        moveDirection = direction;
    }




    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }

}
