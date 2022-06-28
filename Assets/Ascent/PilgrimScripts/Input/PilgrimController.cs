using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PilgrimController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Modifier to change the walkspeed of the pilgrim, default 2.5")]
    private float MoveSpeed = 2.5f;

    [SerializeField]
    [Tooltip("Modifier to change the speed the pilgrim rotates to face the direction of travel, default 360")]
    private float RotateSpeed = 360f;

    [SerializeField]
    [Tooltip("The transform of the visual component of the pilgrim to be rotated to face the direction of travel")]
    private Transform VisualComponent;

    [SerializeField]
    [Tooltip("The rigidbody attached to the highest level of the pilgrim's heirarchy. Used for movement.")]
    private Rigidbody MainRB;

    private Vector3 moveDirection;

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
        // Set up input event triggers for movement.
        Controls.Pilgrim.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        Controls.Pilgrim.Movement.canceled += ctx => OnMovement(ctx.ReadValue<Vector2>());
    }

    private void Update()
    {
        // Move the pilgrim smoothly in correct direction.
        MainRB.position += moveDirection * MoveSpeed * Time.deltaTime;

        // While moving rotate the pilgrim to face direction of travel.
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotateTo = Quaternion.LookRotation(moveDirection, Vector3.up);

            VisualComponent.rotation = Quaternion.RotateTowards(VisualComponent.rotation, rotateTo.normalized, RotateSpeed * Time.deltaTime);
        }
    }


    /// <summary>
    /// Call when movement performed to set direction of travel.
    /// Call again when input cancelled to stop movement.
    /// </summary>
    /// <param name="direction"></param>
    private void OnMovement(Vector2 direction)
    {
        moveDirection = new Vector3(direction.x, 0f, direction.y);
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
