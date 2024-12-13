using UnityEngine;

namespace WinterUniverse
{
    public class TrapKnockback : TrapBase
    {
        [SerializeField] private float _force = 10f;
        [SerializeField] private Vector2 _knockbackDirection;
        [SerializeField] private bool _usePointInsteadDirection;
        [SerializeField] private Transform _knockbackPoint;
        [SerializeField] private bool _reversePointDirection;

        protected override void OnEnterAction(PawnController pawn)
        {
            base.OnEnterAction(pawn);
            if (_usePointInsteadDirection)
            {
                if (_reversePointDirection)
                {
                    pawn.PawnLocomotion.AddKnockbackForce(_knockbackPoint.position - pawn.transform.position, _force);
                }
                else
                {
                    pawn.PawnLocomotion.AddKnockbackForce(pawn.transform.position - _knockbackPoint.position, _force);
                }
            }
            else
            {
                pawn.PawnLocomotion.AddKnockbackForce(_knockbackDirection, _force);
            }
        }
    }
}