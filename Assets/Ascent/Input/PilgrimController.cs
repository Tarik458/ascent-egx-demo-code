using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PilgrimController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Modifier to change the walkspeed of the pilgrim")]
    private float MoveSpeed = 2.5f;
    [SerializeField]
    [Tooltip("The visual component of the pilgrim to be rotated to face the direction of travel")]
    private GameObject VisualComponent;

    private Vector2 moveDirection;

    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private PlayerControls _controls;
    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private PlayerControls Controls
    {
        get
        {
            if (_controls != null)
            {
                return _controls;
            }
            return _controls = new PlayerControls();
        }
    }


    private void Awake()
    {
        // Set up event triggers for movement.
        Controls.Pilgrim.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        Controls.Pilgrim.Movement.canceled += ctx => OnMovement(ctx.ReadValue<Vector2>());
    }

    private void Update()
    {
        // Move the pilgrim smoothly in correct direction.
        transform.Translate(new Vector3(moveDirection.x, 0f, moveDirection.y) * MoveSpeed * Time.deltaTime);
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
