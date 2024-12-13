using UnityEngine;

namespace WinterUniverse
{
    public class DamageCollider : MonoBehaviour
    {
        private PawnController _pawn;

        public void Initialize()
        {
            _pawn = GetComponentInParent<PawnController>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PawnController pawn = collision.GetComponentInParent<PawnController>();
            if (pawn != null && pawn.Team != _pawn.Team)
            {
                pawn.PawnStats.TakeDamage();
            }
        }
    }
}