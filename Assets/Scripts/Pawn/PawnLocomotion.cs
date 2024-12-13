using UnityEngine;

namespace WinterUniverse
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PawnLocomotion : MonoBehaviour
    {
        [SerializeField] private float _timeToJump = 0.25f;
        [SerializeField] private float _timeToFall = 0.25f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _maxFallSpeed = 25f;
        [SerializeField] private float _jumpGravityMultiplier = 1f;
        [SerializeField] private float _fallGravityMultiplier = 2f;
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private Transform _wallCheckPoint;
        [SerializeField] private Vector2 _wallCheckSize;
        [SerializeField] private Transform _roofCheckPoint;
        [SerializeField] private Vector2 _roofCheckSize;

        private PawnController _pawn;
        private Rigidbody2D _rb;
        private float _movementVelocity;
        private float _fallVelocity;
        private float _jumpTime;
        private float _groundedTime;
        private int _jumpCount;
        private Vector2 _knockbackVelocity;
        private Vector2 _linearVelocity;

        public void Initialize()
        {
            _pawn = GetComponent<PawnController>();
            _rb = GetComponent<Rigidbody2D>();
            _movementVelocity = 0f;
            _fallVelocity = 0f;
            _jumpTime = 0f;
            _groundedTime = 0f;
            _jumpCount = 0;
            _linearVelocity = Vector2.zero;
            _rb.linearVelocity = Vector2.zero;
            _rb.mass = _pawn.PawnStats.Mass;
        }

        public void OnFixedUpdate()
        {
            HandleKnockback();
            HandleGravity();
            HandleMovement();
            HandleVelocity();
        }

        private void HandleKnockback()
        {
            if (_knockbackVelocity != Vector2.zero)
            {
                _knockbackVelocity = Vector2.MoveTowards(_knockbackVelocity, Vector2.zero, _rb.mass * Time.fixedDeltaTime);
                _pawn.IsKnockbacked = true;
            }
            else
            {
                _pawn.IsKnockbacked = false;
            }
        }

        private void HandleGravity()
        {
            if (_pawn.CanJump && !_pawn.IsKnockbacked && _jumpTime > 0f && _groundedTime > 0f)
            {
                ApplyJumpForce();
            }
            _pawn.IsGrounded = _fallVelocity <= 0f && Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, WorldManager.StaticInstance.LayerManager.ObstacleMask);
            if (_pawn.IsGrounded)
            {
                _groundedTime = _timeToFall;
                _fallVelocity = 0f;
                _jumpCount = 0;
            }
            else
            {
                if (UnderRoof())
                {
                    _fallVelocity = 0f;
                    _knockbackVelocity.y = 0f;
                }
                _groundedTime -= Time.fixedDeltaTime;
                if (_fallVelocity >= 0f)
                {
                    _fallVelocity += _gravity * _jumpGravityMultiplier * Time.fixedDeltaTime;
                }
                else if (_fallVelocity > -_maxFallSpeed)
                {
                    _fallVelocity += _gravity * _fallGravityMultiplier * Time.fixedDeltaTime;
                }
            }
            _jumpTime -= Time.fixedDeltaTime;
        }

        private void HandleMovement()
        {
            if (_pawn.CanMove && _pawn.MoveDirection.x != 0f)
            {
                _movementVelocity = Mathf.MoveTowards(_movementVelocity, _pawn.MoveDirection.x * _pawn.PawnStats.MaxSpeed, _pawn.PawnStats.Acceleration * Time.fixedDeltaTime);
                if (_pawn.IsFacingRight && _pawn.MoveDirection.x < 0f)
                {
                    _pawn.FlipLeft();
                }
                else if (!_pawn.IsFacingRight && _pawn.MoveDirection.x > 0f)
                {
                    _pawn.FlipRight();
                }
            }
            else
            {
                _movementVelocity = Mathf.MoveTowards(_movementVelocity, 0f, _pawn.PawnStats.Deceleration * Time.fixedDeltaTime);
            }
            if (FacedToWall())
            {
                _movementVelocity = 0f;
            }
            _pawn.IsMoving = _movementVelocity != 0f;
        }

        private void HandleVelocity()
        {
            _linearVelocity.x = _movementVelocity + _knockbackVelocity.x;
            _linearVelocity.y = _fallVelocity + _knockbackVelocity.y;
            _rb.linearVelocityX = _linearVelocity.x;
            _rb.linearVelocityY = _linearVelocity.y;
        }

        private bool UnderRoof()
        {
            return _linearVelocity.y > 0f && Physics2D.OverlapBox(_roofCheckPoint.position, _roofCheckSize, 0f, WorldManager.StaticInstance.LayerManager.ObstacleMask);
        }

        private bool FacedToWall()
        {
            if (_pawn.IsFacingRight && _linearVelocity.x > 0f)
            {
                return Physics2D.OverlapBox(_wallCheckPoint.position, _wallCheckSize, 0f, WorldManager.StaticInstance.LayerManager.ObstacleMask);
            }
            else if (!_pawn.IsFacingRight && _linearVelocity.x < 0f)
            {
                return Physics2D.OverlapBox(_wallCheckPoint.position, _wallCheckSize, 0f, WorldManager.StaticInstance.LayerManager.ObstacleMask);
            }
            return false;
        }

        public void StartJumping()
        {
            if (!_pawn.CanJump || _pawn.IsKnockbacked || _jumpCount >= _pawn.PawnStats.JumpCount)
            {
                return;
            }
            if (_jumpCount == 0)
            {
                _jumpTime = _timeToJump;
            }
            else
            {
                ApplyJumpForce();
            }
        }

        public void StopJumping()
        {
            if (_fallVelocity > 0f)
            {
                _fallVelocity /= 2f;
            }
        }

        public void AddKnockbackForce(Vector2 direction, float force)
        {
            _knockbackVelocity += direction.normalized * force;
            _fallVelocity = 0f;
        }

        private void ApplyJumpForce()
        {
            _jumpCount++;
            _jumpTime = 0f;
            _groundedTime = 0f;
            _fallVelocity = _pawn.PawnStats.JumpForce;
        }

        private void OnDrawGizmos()
        {
            if (_groundCheckPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
            }
            if (_wallCheckPoint != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(_wallCheckPoint.position, _wallCheckSize);
            }
            if (_roofCheckPoint != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(_roofCheckPoint.position, _roofCheckSize);
            }
        }
    }
}