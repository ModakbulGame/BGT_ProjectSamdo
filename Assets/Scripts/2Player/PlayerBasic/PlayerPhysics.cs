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
    public bool IsGrounded { get; private set; }
    private float GroundCheckerThrashold = 0.1f;                        // IsGrounded Ȯ�� ����
    private void CheckGrounded()
    {
        PrevGrounded = IsGrounded;
        IsGrounded = Physics.CheckSphere(Position, GroundCheckerThrashold, ValueDefine.GROUND_LAYER);
    }


    // 벽
    private bool IsTouchingWall { get; set; }
    private Vector3 WallNormal { get; set; }
    private float WallCheckerDistance = 0.5f;
    public float WallCheckerThrashold = 0.8f;
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

    public AnimationCurve SpeedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float CanSlideMultiplierCurve = 0.061f;
    public float CantSlideMultiplierCurve = 0.039f;

    private Vector3 GroundNormal { get; set; }
    private Vector3 PrevGroundNormal { get; set; }
    private bool LockOnSlope { get; set; }
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
                CurLockOnSlope = LockOnSlope;

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
                    CurLockOnSlope = LockOnSlope;
                }
                else
                {
                    PlayerForward = tmpForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);
                    GlobalForward = tmpGlobalForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);
                    ReactionForward = tmpReactionForward * ((SpeedMultiplierOnAngle.Evaluate(CurSurfaceAngle / 90f) * CantSlideMultiplierCurve) + 1f);

                    FunctionDefine.SetFriction(m_collider, 0f, true);
                    CurLockOnSlope = LockOnSlope;
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
            CurLockOnSlope = LockOnSlope;
        }
        Debug.Log(PlayerForward.x + ", " + PlayerForward.z);
    }


    // 중력 관련
    private readonly float GravityMultiplier = 3f;
    private readonly float GravityMultiplyerOnSlideChange = 1.5f;
    private readonly float GravityMultiplierIfUnclimbableSlope = 15f;
    private void ApplyGravity()                           // 중력 적용
    {
        Vector3 gravity;

        if (CurLockOnSlope) gravity = GravityMultiplier*-Physics.gravity.y*PlayerDown;
        else gravity = GravityMultiplier*-Physics.gravity.y*GlobalDown;

        if (GroundNormal.y != 1 && GroundNormal.y != 0 && IsOnSlope && PrevGroundNormal != GroundNormal)
        {
            gravity *= GravityMultiplyerOnSlideChange;
        }

        if (GroundNormal.y != 1 && GroundNormal.y != 0 && (CurSurfaceAngle > m_maxSlopeAngle))
        {
            if (CurSurfaceAngle > 0f && CurSurfaceAngle <= 30f) gravity = GlobalDown * GravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
            else if (CurSurfaceAngle > 30f && CurSurfaceAngle <= 89f) gravity = GlobalDown * GravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
        }

        m_rigid.AddForce(gravity);
    }


    private void PrePhysicsUpdate()
    {
        CheckGrounded();
        CheckWall();
        CheckSlopeAndDirections();
    }

    private void LatePhysicsUpdate()
    {
        ApplyGravity();
    }
}
