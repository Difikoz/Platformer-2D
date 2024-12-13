using System;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnStats : MonoBehaviour
    {
        public Action<int, int> OnHealthChanged;

        public float Acceleration = 8f;
        public float Deceleration = 16f;
        public float MaxSpeed = 4f;
        public float Mass = 80f;
        public float JumpForce = 8f;
        public int JumpCount = 1;
        public int Health = 0;
        public int HealthMax = 1;

        private PawnController _pawn;

        public void Initialize()
        {
            _pawn = GetComponent<PawnController>();
            RestoreHealth(HealthMax);
        }

        public void TakeDamage(int value = 1)
        {
            if (_pawn.IsDead)
            {
                return;
            }
            Health = Mathf.Clamp(Health - value, 0, HealthMax);
            if (Health <= 0f)
            {
                _pawn.Die();
            }
            else
            {
                _pawn.PawnCombat.SpawnBlood();
            }
            OnHealthChanged?.Invoke(Health, HealthMax);
        }

        public void RestoreHealth(int value = 1)
        {
            if (_pawn.IsDead)
            {
                return;
            }
            Health = Mathf.Clamp(Health + value, 0, HealthMax);
            OnHealthChanged?.Invoke(Health, HealthMax);
        }
    }
}