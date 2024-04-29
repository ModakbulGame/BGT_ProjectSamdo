using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController
{
    private const float FLOOR_FRICTION = 0.3f;


    private Vector3 PlayerForward { get; set; }
    private Vector3 GlobalForward { get; set; }
    private Vector3 ReactionForward { get; set; }
    private Vector3 PlayerDown { get; set; }
    private Vector3 GlobalDown { get; set; }
    private Vector3 ReactionDown { get; set; }
    private float TargetAngle { get; set; }
    private float HalfHeight { get { return ObjectHeight / 2; } }


    // 바닥
    private bool PrevGrounded { get; set; }
    private float MaxFallPower { get; set; } = 0;
    public bool IsGrounded { get; private set; }
    private float GroundCheckerThrashold = 0.1f;                        // IsGrounded 확인 범위
    private void CheckGrounded()
    {
        PrevGrounded = IsGrounded;
        MaxFallPower = m_rigid.velocity.y < 0 ? m_rigid.velocity.y : MaxFallPower;
        IsGrounded = Physics.CheckSphere(Position, GroundCheckerThrashold, ValueDefine.GROUND_LAYER);
        if (IsGrounded && !PrevGrounded)
        {
            PlayerLand(MaxFallPower);
        }
    }


    // 계단
    private bool IsTouchingStep = false;
    private float MaxStepHeight = 0.5f;
    private float StepCheckerThrashold = 0.6f;
    private void CheckStep()
    {
        bool tmpStep = false;
        Vector3 bottomStepPos = transform.position + new Vector3(0f, 0.05f, 0f);

        RaycastHit stepLowerHit;
        if (Physics.Raycast(bottomStepPos, GlobalForward, out stepLowerHit, StepCheckerThrashold, ValueDefine.GROUND_LAYER))
        {
            RaycastHit stepUpperHit;
            if (RoundValue(stepLowerHit.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), GlobalForward, out stepUpperHit, StepCheckerThrashold + 0.05f, ValueDefine.GROUND_LAYER))
            {
                //m_rigid.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHit45;
        if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(45, transform.up) * GlobalForward, out stepLowerHit45, StepCheckerThrashold, ValueDefine.GROUND_LAYER))
        {
            RaycastHit stepUpperHit45;
            if (RoundValue(stepLowerHit45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), Quaternion.AngleAxis(45, Vector3.up) * GlobalForward, out stepUpperHit45, StepCheckerThrashold + 0.05f, ValueDefine.GROUND_LAYER))
            {
                //m_rigid.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHitMinus45;
        if (Physics.Raycast(bottomStepPos, Quaternion.AngleAxis(-45, transform.up) * GlobalForward, out stepLowerHitMinus45, StepCheckerThrashold, ValueDefine.GROUND_LAYER))
        {
            RaycastHit stepUpperHitMinus45;
            if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && !Physics.Raycast(bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), Quaternion.AngleAxis(-45, Vector3.up) * GlobalForward, out stepUpperHitMinus45, StepCheckerThrashold + 0.05f, ValueDefine.GROUND_LAYER))
            {
                //m_rigid.position -= new Vector3(0f, -stepSmooth, 0f);
                tmpStep = true;
            }
        }

        IsTouchingStep = tmpStep;
    }
    private float RoundValue(float _value)
    {
        float unit = (float)Mathf.Round(_value);

        if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
        else return _value;
    }

    // 벽
    private bool IsTouchingWall { get; set; }
    private Vector3 WallNormal { get; set; }
    private float WallCheckerDistance = 0.5f;
    private float WallCheckerThrashold = 0.8f;
    private void CheckWall()
    {
        bool tmpWall = false;
        Vector3 tmpWallNormal = Vector3.zero;
        Vector3 topWallPos = Position + Vector3.up * (HalfHeight + WallCheckerDistance);

        RaycastHit wallHit;
        for (int i = 0; i < 8; i++)
        {
            if (Physics.Raycast(topWallPos, Quaternion.AngleAxis(i * 45, transform.up) * GlobalForward, out wallHit, WallCheckerThrashold, ValueDefine.GROUND_LAYER))
            {
                tmpWallNormal = wallHit.normal;
                tmpWall = true;
                break;
            }
        }

        IsTouchingWall = tmpWall;
        WallNormal = tmpWallNormal;
    }


    // 경사 / 방향
    private const float SLOPE_RAY_DIST = 0.1f;                          // 경사 탐지 거리
    [SerializeField]
    private float m_maxSlopeAngle = 56;                                 // 최대 오를 수 있는 각도

    private AnimationCurve SpeedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private float CanSlideMultiplierCurve = 0.061f;
    private float CantSlideMultiplierCurve = 0.039f;
    private float ClimbingStairsMultiplierCurve = 0.637f;

    private Vector3 GroundNormal { get; set; }
    private Vector3 PrevGroundNormal { get; set; }
    private bool m_lockOnSlope = true;
    private bool CurLockOnSlope { get; set; }
    private float CurSurfaceAngle { get; set; }
    public bool IsOnSlope { get; private set; }                         // 경사 위

    private void CheckSlopeAndDirections()
    {
        PrevGroundNormal = GroundNormal;

        RaycastHit slopeHit;
        if (Physics.SphereCast(transform.position + Vector3.up * HalfHeight, SLOPE_RAY_DIST, Vector3.down, out slopeHit, HalfHeight + 0.5f, ValueDefine.GROUND_LAYER))
        {
            GroundNormal = slopeHit.normal;

            if (slopeHit.normal.y == 1)
            {

                PlayerForward = Quaternion.Euler(0f, TargetAngle, 0f) * Vector3.forward;
                GlobalForward = PlayerForward;
                ReactionForward = PlayerForward;

                FunctionDefine.SetFriction(m_collider, FLOOR_FRICTION, true);
                CurLockOnSlope = m_lockOnSlope;

                CurSurfaceAngle = 0f;
                IsOnSlope = false;

            }
            else
            {
                Vector3 tmpGlobalForward = transform.forward.normalized;
                Vector3 tmpForward = new Vector3(tmpGlobalForward.x, Vector3.ProjectOnPlane(transform.forward.normalized, slopeHit.normal).normalized.y, tmpGlobalForward.z);
                Vector3 tmpReactionForward = new Vector3(tmpForward.x, tmpGlobalForward.y - tmpForward.y, tmpForward.z);

                if (CurSurfaceAngle <= m_maxSlopeAngle)
                {
                    PlayerForward = tmpForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CanSlideMultiplierCurve) + 1f);
                    GlobalForward = tmpGlobalForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CanSlideMultiplierCurve) + 1f);
                    ReactionForward = tmpReactionForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CanSlideMultiplierCurve) + 1f);

                    FunctionDefine.SetFriction(m_collider, FLOOR_FRICTION, true);
                    CurLockOnSlope = m_lockOnSlope;
                }
                else if (IsTouchingStep)
                {
                    //set forward
                    PlayerForward = tmpForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * ClimbingStairsMultiplierCurve) + 1f);
                    GlobalForward = tmpGlobalForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * ClimbingStairsMultiplierCurve) + 1f);
                    ReactionForward = tmpReactionForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * ClimbingStairsMultiplierCurve) + 1f);

                    FunctionDefine.SetFriction(m_collider, FLOOR_FRICTION, true);
                    CurLockOnSlope = true;
                }
                else
                {
                    PlayerForward = tmpForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);
                    GlobalForward = tmpGlobalForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);
                    ReactionForward = tmpReactionForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);

                    FunctionDefine.SetFriction(m_collider, 0f, true);
                    CurLockOnSlope = m_lockOnSlope;
                }

                CurSurfaceAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                IsOnSlope = true;
            }

            PlayerDown = Vector3.Project(Vector3.down, slopeHit.normal);
            GlobalDown = Vector3.down.normalized;
            ReactionDown = Vector3.up.normalized;
        }
        else
        {
            GroundNormal = Vector3.zero;

            PlayerForward = Vector3.ProjectOnPlane(transform.forward, slopeHit.normal).normalized;
            GlobalForward = PlayerForward;
            ReactionForward = PlayerForward;

            PlayerDown = Vector3.down.normalized;
            GlobalDown = Vector3.down.normalized;
            ReactionDown = Vector3.up.normalized;

            FunctionDefine.SetFriction(m_collider, FLOOR_FRICTION, true);
            CurLockOnSlope = m_lockOnSlope;
        }
    }


    // 중력 관련
    private readonly float GravityMultiplier = 3f;
    private readonly float GravityMultiplyerOnSlideChange = 1.5f;
    private readonly float GravityMultiplierIfUnclimbableSlope = 15f;
    private float CoyoteJumpMultiplier = 1f;
    private void ApplyGravity()                           // 중력 적용
    {
        Vector3 gravity; 
        
        if (m_rigid.velocity.y < 0 && !IsGrounded) CoyoteJumpMultiplier = 1.7f;
        else CoyoteJumpMultiplier = 1f;

        if (CurLockOnSlope || IsTouchingStep) gravity = GravityMultiplier*-Physics.gravity.y*PlayerDown * CoyoteJumpMultiplier;
        else gravity = GravityMultiplier*-Physics.gravity.y*GlobalDown * CoyoteJumpMultiplier;

        if (GroundNormal.y != 1 && GroundNormal.y != 0 && IsOnSlope && PrevGroundNormal != GroundNormal)
        {
            gravity *= GravityMultiplyerOnSlideChange;
        }

        if (GroundNormal.y != 1 && GroundNormal.y != 0 && (CurSurfaceAngle > m_maxSlopeAngle) && !IsTouchingStep)
        {
            if (CurSurfaceAngle > 0f && CurSurfaceAngle <= 30f) gravity = GlobalDown * GravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
            else if (CurSurfaceAngle > 30f && CurSurfaceAngle <= 89f) gravity = GlobalDown * GravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
        }


        m_rigid.AddForce(gravity);
    }


    private void PrePhysicsUpdate()
    {
        CheckGrounded();
        CheckStep();
        CheckWall();
        CheckSlopeAndDirections();
    }

    private void LatePhysicsUpdate()
    {
        ApplyGravity();
    }
}
