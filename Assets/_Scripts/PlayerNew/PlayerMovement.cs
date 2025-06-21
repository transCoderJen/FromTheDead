// using System;
// using Unity.VisualScripting;
// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("References")]
//     public PlayerMovementStats MoveStats;
//     [SerializeField] private Collider2D _feetColl;
//     [SerializeField] private Collider2D _bodyColl;

//     private Rigidbody2D _rb;

//     //movement
//     private Vector2 _moveVelocity;
//     private bool _isFacingRight;

//     //collsions check
//     private RaycastHit2D _groundHit;
//     private RaycastHit2D _headHit;
//     private bool _isGrounded;
//     private bool _bumpedHead;

//     //jump
//     public float VerticalVelocity { get; private set; }
//     private bool _isJumping;
//     private bool _isFastFalling;
//     private bool _isFalling;
//     private float _fastFallTime;
//     private float _fastFallReleaseSpeed;
//     private int _nummberOfJumpsUsed;

//     //apex
//     private float _apexPoint;
//     private float _timePastApexThreshold;
//     private bool _isPastApexThreshold;

//     //jump buffers
//     private float _jumpBufferTimer;
//     private bool _jumpReleasedDuringBuffer;

//     //coyote time
//     private float _coyoteTimer;

//     private void Awake()
//     {
//         _isFacingRight = true;

//         _rb = GetComponent<Rigidbody2D>();
//     }

//     private void Update()
//     {
//         CountTimers();
//         JumpChecks();
//     }

//     private void FixedUpdate()
//     {
//         CollisionChecks();
//         Jump();

//         if (_isGrounded)
//         {
//             Move(MoveStats.GroundAcceleration, MoveStats.GroundAcceleration, InputManager.Movement);
//         }
//         else
//         {
//             Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
//         }
//     }

//     #region Movement
//     private void Move(float acceleration, float deceleration, Vector2 moveInput)
//     {
//         if (moveInput != Vector2.zero)
//         {
//             TurnCheck(moveInput);

//             Vector2 targetVelocity = Vector2.zero;
//             if (InputManager.RunIsHeld)
//             {
//                 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
//             }
//             else
//             {
//                 targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;
//             }

//             _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
//             _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//         }

//         else if (moveInput == Vector2.zero)
//         {
//             _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
//             _rb.linearVelocity = new Vector2(_moveVelocity.x, _rb.linearVelocity.y);
//         }
//     }

//     private void TurnCheck(Vector2 moveInput)
//     {
//         if (_isFacingRight && moveInput.x < 0)
//         {
//             Turn(false);
//         }

//         else if (!_isFacingRight && moveInput.x > 0)
//         {
//             Turn(true);
//         }
//     }

//     private void Turn(bool turnRight)
//     {
//         if (turnRight)
//         {
//             _isFacingRight = true;
//             transform.Rotate(0f, 180f, 0f);
//         }
//         else
//         {
//             _isFacingRight = false;
//             transform.Rotate(0f, -180f, 0f);
//         }
//     }

//     #endregion

//     #region Jump

//     private void JumpChecks()
//     {
//         // WHEN WE PRESS THE JUMP BUTTON
//         if (InputManager.JumpWasPressed)
//         {
//             _jumpBufferTimer = MoveStats.JumpBufferTime;
//             _jumpReleasedDuringBuffer = false;
//         }

//         // WHEN WE RELEASE THE JUMP BUTTON
//         if (InputManager.JumpWasReleased)
//         {
//             if (_jumpBufferTimer > 0f)
//             {
//                 _jumpReleasedDuringBuffer = true;
//             }

//             if (_isJumping && VerticalVelocity > 0f)
//             {
//                 if (_isPastApexThreshold)
//                 {
//                     _isPastApexThreshold = false;
//                     _isFastFalling = true;
//                     _fastFallTime = MoveStats.TimeForUpwardsCancel;
//                     VerticalVelocity = 0f;
//                 }
//                 else
//                 {
//                     _isFastFalling = true;
//                     _fastFallReleaseSpeed = VerticalVelocity;
//                 }
//             }
//         }
//         // INITIATE JUMP WITH JUMP BUFFERING AND COYOTE TIME
//         if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
//         {
//             InitiateJump(1);

//             if (_jumpReleasedDuringBuffer)
//             {
//                 _isFastFalling = true;
//                 _fastFallReleaseSpeed = VerticalVelocity;
//             }
//         }

//         // DOUBLE JUMP
//         else if (_jumpBufferTimer > 0f && _isJumping && _nummberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
//         {
//             _isFastFalling = false;
//             InitiateJump(1);
//         }

//         // AIR JUMP AFTER COYOTE TIME LAPSED
//         else if (_jumpBufferTimer > 0f && _isFalling && _nummberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
//         {
//             InitiateJump(2);
//             _isFastFalling = false;
//         }

//         // LANDED
//         if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
//         {
//             _isJumping = false;
//             _isFalling = false;
//             _isFastFalling = false;
//             _fastFallTime = 0f;
//             _isPastApexThreshold = false;
//             _nummberOfJumpsUsed = 0;

//             VerticalVelocity = Physics2D.gravity.y;
//         }
//     }

//     private void InitiateJump(int nummberOfJumpsUsed)
//     {
//         if (!_isJumping)
//         {
//             _isJumping = true;
//         }

//         _jumpBufferTimer = 0f;
//         _nummberOfJumpsUsed += nummberOfJumpsUsed;
//         VerticalVelocity = MoveStats.InitialJumpVelocity;
//     }

//     private void Jump()
//     {
//         // APPLY GRAVITY WHILE JUMPING
//         if (_isJumping)
//         {
//             // CHECK FOR HEAD BUMP
//             if (_bumpedHead)
//             {
//                 _isFastFalling = true;
//             }

//             // GRAVITY ON ASCENDING
//             if (VerticalVelocity >= 0f)
//             {
//                 // APEX CONTROLS
//                 _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);

//                 if (_apexPoint > MoveStats.ApexThreshold)
//                 {
//                     if (!_isPastApexThreshold)
//                     {
//                         _isPastApexThreshold = true;
//                         _timePastApexThreshold = 0f;
//                     }

//                     if (_isPastApexThreshold)
//                     {
//                         _timePastApexThreshold += Time.fixedDeltaTime;
//                         if (_timePastApexThreshold < MoveStats.ApexHangeTime)
//                         {
//                             VerticalVelocity = 0f;
//                         }
//                         else { VerticalVelocity = -0.01f; }
//                     }
//                 }

//                 // GRAVITY ON ASCENDING BUT NOT PAST APEX THRESHOLD
//                 else
//                 {
//                     VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//                     if (_isPastApexThreshold)
//                     {
//                         _isPastApexThreshold = false;
//                     }

//                 }
//             }

//             // GRAVITY ON DESCENDING
//             else if (_isFastFalling)
//             {
//                 VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//             }

//             else if (VerticalVelocity < 0f)
//             {
//                 if (!_isFalling)
//                 {
//                     _isFalling = true;
//                 }
//             }

//         }

//         // JUMP CUT
//         if (_isFastFalling)
//         {
//             if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
//             {
//                 VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
//             }
//             else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
//             {
//                 VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
//             }

//             _fastFallTime += Time.fixedDeltaTime;
//         }

//         // NORMAL GRAVITY WHILE FALLING
//         if (!_isGrounded && !_isJumping)
//         {
//             if (!_isFalling)
//             {
//                 _isFalling = true;
//             }

//             VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
//         }

//         // CLAMPY FALL SPEED
//         VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);

//         _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);
//     }

//     #endregion

//     #region Collisions

//     private void IsGrounded()
//     {
//         Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
//         Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLength);

//         _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
//         if (_groundHit.collider != null)
//         {
//             _isGrounded = true;
//         }
//         else { _isGrounded = false; }

//         #region Debug Visualization
//         if (MoveStats.DebugShowIsGroundedBox)
//         {
//             Color rayColor;
//             if (_isGrounded)
//             {
//                 rayColor = Color.green;
//             }
//             else { rayColor = Color.red; }

//             Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector3.down * MoveStats.GroundDetectionRayLength, rayColor);
//             Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2, boxCastOrigin.y), Vector3.down * MoveStats.GroundDetectionRayLength, rayColor);
//             Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector3.right * boxCastSize.x, rayColor);
//         }
//         #endregion
//     }

//     private void BumpedHead()
//     {
//         Vector2 boxCastOrigin = new Vector2(_bodyColl.bounds.center.x, _bodyColl.bounds.max.y);
//         Vector2 boxCastSize = new Vector2(_bodyColl.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

//         _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
//         if (_headHit.collider != null)
//         {
//             _bumpedHead = true;
//         }
//         else { _bumpedHead = false; }

//         #region Debug Visualization
//         if (MoveStats.DebugShowHeadBumpBox)
//         {
//             float headWidth = MoveStats.HeadWidth;

//             Color rayColor;
//             if (_bumpedHead)
//             {
//                 rayColor = Color.green;
//             }
//             else { rayColor = Color.red; }

//             Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//             Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
//             Debug.DrawRay(new Vector2(boxCastOrigin.x + boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);

//         }
//         #endregion
//     }

//     private void CollisionChecks()
//     {
//         IsGrounded();
//         BumpedHead();
//     }

//     #endregion

//     #region Timers

//     private void CountTimers()
//     {
//         _jumpBufferTimer -= Time.deltaTime;

//         if (!_isGrounded)
//         {
//             _coyoteTimer -= Time.deltaTime;
//         }
//         else { _coyoteTimer = MoveStats.JumpCoyoteTime;  }
//     }

//     #endregion
// }