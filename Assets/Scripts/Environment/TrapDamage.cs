using UnityEngine;

namespace WinterUniverse
{
    public class TrapDamage : TrapBase
    {
        [SerializeField] private int _damageOnEnter = 1;
        [SerializeField] private int _damageOnStay = 1;
        [SerializeField] private int _damageOnExit = 1;

        protected override void OnEnterAction(PawnController pawn)
        {
            base.OnEnterAction(pawn);
            pawn.PawnStats.TakeDamage(_damageOnEnter);
        }

        protected override void OnStayAction(PawnController pawn)
        {
            base.OnStayAction(pawn);
            pawn.PawnStats.TakeDamage(_damageOnStay);
        }

        protected override void OnExitAction(PawnController pawn)
        {
            base.OnExitAction(pawn);
            pawn.PawnStats.TakeDamage(_damageOnExit);
        }
    }
}