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
    [Tooltip("The Main camera with the PilgrimFollowCam script attached.")]
    private PilgrimFollowCam FollowCam;

    [SerializeField]
    [Tooltip("Modifier to change the walkspeed of the pilgrim, default 2.5")]
    private float MoveSpeed = 2.5f;

    [SerializeField]
    [Tooltip("Modifier to change the initial force applied to a jump, default 5")]
    private float JumpPower = 5f;

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
    private Transform VisualComponentTransform;

    [SerializeField]
    [Tooltip("The transform of the collider component of the pilgrim; used for crouching etc.")]
    private Transform ColliderTransform;

    [SerializeField]
    [Tooltip("Modifier to change the speed the pilgrim's visual component rotates to face the direction of travel, default 360")]
    private float RotateSpeed = 360f;


    [SerializeField]
    [Tooltip("The other script also attached to pilgrim, helps keep this script clean.")]
    private TriggerZoneInfo triggerZoneInfo;

    [SerializeField]
    [Tooltip("Temporary script for controlling Mixamo anims.")]
    private MixamoController mixamoController;

    [SerializeField]
    private AudioClip[] Footsteps;

    private Vector3 moveDirection;
    private float camFacingDirection;

    private bool isCrouched = false;

    private bool isJumping = false;
    // Distance to raycast downwards from pilgrim for groundcheck, should be half height + small margin.
    private float findFloorRaycastDist;
    private float findWallRaycastDist;
    private Vector3 wallCastPos;

    private float jumpCheckTimeDelay = 0.5f;
    private float timeSinceJump = 0f;

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
        if(FollowCam != null)
        {
            FollowCam.SetTarget(this.transform);
        }

        // Set up input event triggers for movement.
        Controls.Pilgrim.Movement.performed += ctx => OnMovement(ctx.ReadValue<Vector2>());
        Controls.Pilgrim.Movement.canceled += ctx => OnMovement(ctx.ReadValue<Vector2>());
        Controls.Pilgrim.Jump.performed += ctx => OnJump();
        Controls.Pilgrim.Interact.performed += ctx => OnInteractPressed();
    }

    private void Start()
    {
        findFloorRaycastDist = (ColliderTransform.gameObject.GetComponent<CapsuleCollider>().height / 2) + 0.2f;
        findWallRaycastDist = ColliderTransform.gameObject.GetComponent<CapsuleCollider>().radius + 0.1f;
        camFacingDirection = FollowCam.transform.eulerAngles.y;
    }

    private void Update()
    {
        camFacingDirection = FollowCam.GetCurrentCamRot().eulerAngles.y;
        Vector3 camRelativeMoveDir = Quaternion.Euler(0, camFacingDirection, 0) * moveDirection;

        // Used to stop character from sticking to walls.
         wallCastPos = new(transform.position.x, transform.position.y - 0.4f, transform.position.z);

        // Move the pilgrim smoothly in correct direction.
        if (moveDirection == Vector3.zero || Physics.Raycast(wallCastPos, camRelativeMoveDir, findWallRaycastDist))
        {
            MainRB.velocity = new Vector3(0f, MainRB.velocity.y, 0f);
            mixamoController.StopWalking();
            FollowCam.CamTwistDirection(camRelativeMoveDir, true);
        }
        else
        {
            if (isCrouched)
            {
                MainRB.position += CrouchSpeed * MoveSpeed * Time.deltaTime * camRelativeMoveDir;
                FollowCam.CamTwistDirection(moveDirection);
            }
            else
            {
                mixamoController.StartWalking();
                MainRB.position += MoveSpeed * Time.deltaTime * camRelativeMoveDir;
                FollowCam.CamTwistDirection(moveDirection);
            }

            if(!isJumping && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(Footsteps[Random.Range(0, Footsteps.Length - 1)]);
            }

        }

        if (moveDirection != Vector3.zero)
        {
            // While moving rotate the pilgrim to face direction of travel.
            Quaternion rotateTo = Quaternion.LookRotation(camRelativeMoveDir, Vector3.up);
            VisualComponentTransform.rotation = Quaternion.RotateTowards(VisualComponentTransform.rotation, rotateTo.normalized, RotateSpeed * Time.deltaTime);
        }

        // Check if player is on the ground.
        if (isJumping)
        {
            if (Physics.Raycast(transform.position, Vector3.down, findFloorRaycastDist) && timeSinceJump > jumpCheckTimeDelay)
            {
                isJumping = false;
            }
            timeSinceJump += Time.deltaTime;
        }
    }


    /// <summary>
    /// Call when movement performed to set direction of travel.
    /// Call again when input cancelled to stop movement.
    /// </summary>
    /// <param name="_direction"></param>
    private void OnMovement(Vector2 _direction)
    {
        moveDirection = new Vector3(_direction.x, 0f, _direction.y);
    }

    /// <summary>
    /// Call when jump button pressed to boost character upwards, only works if not crouched.
    /// </summary>
    private void OnJump()
    {
        if (!isCrouched && !isJumping && Physics.Raycast(transform.position, Vector3.down, findFloorRaycastDist))
        {
            isJumping = true;
            mixamoController.Jump();
            timeSinceJump = 0f;
            MainRB.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Perform a specific interaction depending on the current surroundings of the character.
    /// </summary>
    private void OnInteractPressed()
    {
        triggerZoneInfo.TestInterations(mixamoController);
    }


    /// <summary>
    /// Call on crouch and on uncrouch. isCrouched variable defines whether to shrink or grow character.
    /// Should be replaced by animation later.
    /// </summary>
    private void OnCrouch(bool _isCrouched)
    {
        isCrouched = _isCrouched;
        // When anims ready only shrink collider component and trigger anim instead of manipulating visual component.
        switch(isCrouched)
        {
            case true:
                VisualComponentTransform.localScale = new Vector3(VisualComponentTransform.localScale.x, VisualComponentTransform.localScale.y * CrouchHeight, VisualComponentTransform.localScale.z);
                ColliderTransform.localScale = new Vector3(ColliderTransform.localScale.x, ColliderTransform.localScale.y * CrouchHeight, ColliderTransform.localScale.z);
                // Translate pilgrim down to floor level to remove moment of floating.
                transform.Translate(new Vector3(0f, -(CrouchHeight / 2), 0f));
                break;
            case false:
                // Translate pilgrim upwards to negate occasional bouncing caused by funky physics.
                transform.Translate(new Vector3(0f, (CrouchHeight / 2), 0f));
                VisualComponentTransform.localScale = new Vector3(VisualComponentTransform.localScale.x, VisualComponentTransform.localScale.y / CrouchHeight, VisualComponentTransform.localScale.z);
                ColliderTransform.localScale = new Vector3(ColliderTransform.localScale.x, ColliderTransform.localScale.y / CrouchHeight, ColliderTransform.localScale.z);
                break;
        }
        
    }

    /// <summary>
    /// Handle passing camera adjustments to the camera script.
    /// </summary>
    /// <param name="_triggerZoneScript"></param>
    private void OnCamAdjust(CamAdjustVals _triggerZoneScript)
    {
        Vector4 offset = _triggerZoneScript.GetAdditionToOffset();
        Vector4 directionToFace = _triggerZoneScript.GetDesiredCamRotation();

        // Checks if vector is empty without W value as W defaults to 1.
        Vector3 timeExclusionChecker;
   
        timeExclusionChecker = offset;
        if (timeExclusionChecker != Vector3.zero)
        {
            // Apply offset.
            FollowCam.AddOffset(offset);
        }

        Vector3 curCamRot = FollowCam.GetPrevCamRot();
        _triggerZoneScript.SetPreviousRotation(curCamRot);
        // Apply rotation.
        FollowCam.SetAngleToFace(directionToFace);
        // Set current cam rotation for backtracking.

        
        _triggerZoneScript.ToggleEnabledZone();
    }


    private void OnTriggerEnter(Collider _trigger)
    {
        switch(_trigger.gameObject.tag)
        {
            case "CrouchZone":
                OnCrouch(true);
                break;
            case "FireZone":
                triggerZoneInfo.EnterFireZone(_trigger.gameObject);
                mixamoController.EnterInteractionZone();
                break;
            case "BeeSwarm":
                triggerZoneInfo.EnterBeeZone(_trigger.gameObject);
                mixamoController.EnterInteractionZone();
                break;
            case "HiveZone":
                if (!triggerZoneInfo.EnterHiveZone(_trigger.gameObject))
                {
                    mixamoController.EnterInteractionZone();
                }
                break;
            case "GhostBeesZone":
                if (triggerZoneInfo.beesFollowing)
                {
                    triggerZoneInfo.ScareBees();
                }
                break;
            case "EndGameZone":
                triggerZoneInfo.EndGame(_trigger.gameObject);
                break;
            case "CamAdjustZone":
                OnCamAdjust(_trigger.gameObject.GetComponentInParent<CamAdjustVals>());
                break;
            default:
                break;
        }
    }


    private void OnTriggerExit(Collider _trigger)
    {
        switch (_trigger.gameObject.tag)
        {
            case "CrouchZone":
                OnCrouch(false);
                break;
            case "FireZone":
                triggerZoneInfo.ExitFireZone();
                break;
            case "BeeSwarm":
                triggerZoneInfo.ExitBeeZone();
                break;
            case "HiveZone":
                triggerZoneInfo.ExitHiveZone();
                break;
            default:
                break;
        }
        mixamoController.ExitInteractionZone();
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
