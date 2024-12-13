using UnityEngine;

namespace WinterUniverse
{
    public class RemoveActionFlag : StateMachineBehaviour
    {
        private PawnController _pawn;

        [SerializeField] private bool _removeIsPerfoming = true;
        [SerializeField] private bool _restoreCanMove = true;
        [SerializeField] private bool _restoreCanJump = true;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _pawn = animator.GetComponent<PawnController>();
            if (_removeIsPerfoming)
            {
                _pawn.IsPerfomingAction = false;
            }
            if (_restoreCanMove)
            {
                _pawn.CanMove = true;
            }
            if (_restoreCanJump)
            {
                _pawn.CanJump = true;
            }
        }
    }
}