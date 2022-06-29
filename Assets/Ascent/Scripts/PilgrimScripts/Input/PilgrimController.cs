using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PilgrimController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The rigidbody attached to the highest level of the pilgrim's heirarchy. Used for movement.")]
    private Rigidbody MainRB;

    [SerializeField]
    [Tooltip("Modifier to change the walkspeed of the pilgrim, default 2.5")]
    private float MoveSpeed = 2.5f;

    [SerializeField]
    [Tooltip("Modifier to change movement speed of pilgrim when crouched, default 0.6f")]
    [Range(0.1f, 0.9f)]
    private float CrouchSpeed = 0.6f;

    [SerializeField]
    [Tooltip("Modifier to multiply the height of the pilgrim by when crouched, default 0.6f")]
    [Range(0.1f, 0.9f)]
    private float CrouchHeight = 0.6f;

    [SerializeField]
    [Tooltip("The transform of the visual component of the pilgrim to be rotated to face the direction of travel")]
    private Transform VisualComponent;

    [SerializeField]
    [Tooltip("The transform of the collider component of the pilgrim; used for crouching etc.")]
    private Transform ColliderComponent;

    [SerializeField]
    [Tooltip("Modifier to change the speed the pilgrim's visual component rotates to face the direction of travel, default 360")]
    private float RotateSpeed = 360f;


    private Vector3 moveDirection;

    private bool isCrouched = false;

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
        if (isCrouched)
        {
            MainRB.position += moveDirection * MoveSpeed * CrouchSpeed * Time.deltaTime;
        }
        else
        {
            MainRB.position += moveDirection * MoveSpeed * Time.deltaTime;
        }

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

    /// <summary>
    /// Call on crouch and on uncrouch. isCrouched variable defines whether to shrink or grow character.
    /// Should be replaced by animation later.
    /// </summary>
    private void OnCrouch()
    {
        // When anims ready only shrink collider component and trigger anim instead of manipulating visual component.
        switch(isCrouched)
        {
            case true:
                VisualComponent.localScale = new Vector3(VisualComponent.localScale.x, VisualComponent.localScale.y * CrouchHeight, VisualComponent.localScale.z);
                ColliderComponent.localScale = new Vector3(ColliderComponent.localScale.x, ColliderComponent.localScale.y * CrouchHeight, ColliderComponent.localScale.z);
                // Translate pilgrim down to floor level to remove moment of floating.
                transform.Translate(new Vector3(0f, -(CrouchHeight / 2), 0f));
                break;
            case false:
                VisualComponent.localScale = new Vector3(VisualComponent.localScale.x, VisualComponent.localScale.y / CrouchHeight, VisualComponent.localScale.z);
                ColliderComponent.localScale = new Vector3(ColliderComponent.localScale.x, ColliderComponent.localScale.y / CrouchHeight, ColliderComponent.localScale.z);
                break;
        }
        
    }

    // Crouch zones.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CrouchZone"))
        {
            isCrouched = true;
            OnCrouch();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CrouchZone"))
        {
            isCrouched = false;
            OnCrouch();
        }
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