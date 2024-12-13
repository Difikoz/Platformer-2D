using System;
using UnityEngine;

namespace WinterUniverse
{
    public abstract class PawnController : MonoBehaviour
    {
        public Action OnDied;

        public PawnTeam Team;
        [HideInInspector] public Vector2 MoveDirection;
        [HideInInspector] public bool IsGrounded;
        [HideInInspector] public bool IsKnockbacked;
        [HideInInspector] public bool IsMoving;
        [HideInInspector] public bool IsFacingRight;
        [HideInInspector] public bool IsPerfomingAction;
        [HideInInspector] public bool CanMove;
        [HideInInspector] public bool CanJump;
        [HideInInspector] public bool IsDead;

        protected PawnAnimator _pawnAnimator;
        protected PawnCombat _pawnCombat;
        protected PawnLocomotion _pawnLocomotion;
        protected PawnSound _pawnSound;
        protected PawnStats _pawnStats;

        public PawnAnimator PawnAnimator => _pawnAnimator;
        public PawnCombat PawnCombat => _pawnCombat;
        public PawnLocomotion PawnLocomotion => _pawnLocomotion;
        public PawnSound PawnSound => _pawnSound;
        public PawnStats PawnStats => _pawnStats;

        public virtual void Initialize()
        {
            GetComponents();
            InitializeComponents();
        }

        protected virtual void GetComponents()
        {
            _pawnAnimator = GetComponent<PawnAnimator>();
            _pawnCombat = GetComponent<PawnCombat>();
            _pawnLocomotion = GetComponent<PawnLocomotion>();
            _pawnSound = GetComponent<PawnSound>();
            _pawnStats = GetComponent<PawnStats>();
        }

        protected virtual void InitializeComponents()
        {
            IsGrounded = true;
            IsKnockbacked = false;
            IsMoving = false;
            IsPerfomingAction = false;
            CanMove = true;
            CanJump = true;
            IsDead = false;
            _pawnAnimator.Initialize();
            _pawnCombat.Initialize();
            _pawnLocomotion.Initialize();
            _pawnSound.Initialize();
            _pawnStats.Initialize();
        }

        public virtual void Despawn()
        {

        }

        public virtual void OnFixedUpdate()
        {
            _pawnLocomotion.OnFixedUpdate();
            _pawnAnimator.OnFixedUpdate();
        }

        public void FlipRight()
        {
            IsFacingRight = true;
            transform.localScale = new(1f, 1f, 1f);
        }

        public void FlipLeft()
        {
            IsFacingRight = false;
            transform.localScale = new(-1f, 1f, 1f);
        }

        public void Die(bool spawnBlood = true)
        {
            if (IsDead)
            {
                return;
            }
            if (spawnBlood)
            {
                _pawnCombat.SpawnBlood();
            }
            IsDead = true;
            _pawnSound.PlayDeathClip();
            _pawnAnimator.PlayAction("Death");
            OnDied?.Invoke();
            PerformDeath();
        }

        protected virtual void PerformDeath()
        {

        }
    }
}