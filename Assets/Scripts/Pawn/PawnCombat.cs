using Lean.Pool;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnCombat : MonoBehaviour
    {
        [SerializeField] private Transform _bloodSpawnPoint;
        [SerializeField] private GameObject _bloodEffect;

        private PawnController _pawn;
        private DamageCollider _damageCollider;

        public void Initialize()
        {
            _pawn = GetComponent<PawnController>();
            _damageCollider = GetComponentInChildren<DamageCollider>();
            _damageCollider.Initialize();
        }

        public void SpawnBlood()
        {
            GameObject effect = LeanPool.Spawn(_bloodEffect, _bloodSpawnPoint.position, Quaternion.identity);
            LeanPool.Despawn(effect, 10f);
        }
    }
}