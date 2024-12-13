using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public abstract class TrapBase : MonoBehaviour
    {
        [SerializeField] private bool _triggerOnceOnEnter;
        [SerializeField] private bool _triggerOnceOnExit;
        [SerializeField] private bool _activateOnEnter;
        [SerializeField] private bool _activateOnStay;
        [SerializeField] private bool _activateOnExit;
        [SerializeField] private bool _hasCooldown;
        [SerializeField] private float _cooldownTime = 2f;
        [SerializeField] private float _triggerStayDelay = 0.5f;

        private bool _triggeredOnEnter;
        private bool _triggeredOnExit;
        private Coroutine _cooldownCoroutine;
        private Coroutine _triggerCoroutine;
        private List<PawnController> _enteredTargets = new();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_triggerOnceOnEnter)
            {
                if (_triggeredOnEnter)
                {
                    return;
                }
                _triggeredOnEnter = true;
            }
            if (_hasCooldown && _cooldownCoroutine != null)
            {
                return;
            }
            if (collision.TryGetComponent(out PawnController pawn) && !_enteredTargets.Contains(pawn))
            {
                _enteredTargets.Add(pawn);
                if (_activateOnEnter)
                {
                    _cooldownCoroutine = StartCoroutine(CooldownTimer());
                    OnEnterAction(pawn);
                }
                if (_activateOnStay && _triggerCoroutine == null)
                {
                    _triggerCoroutine = StartCoroutine(TriggerTimer());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_triggerOnceOnExit)
            {
                if (_triggeredOnExit)
                {
                    return;
                }
                _triggeredOnExit = true;
            }
            if (_hasCooldown && _cooldownCoroutine != null)
            {
                return;
            }
            if (collision.TryGetComponent(out PawnController pawn) && _enteredTargets.Contains(pawn))
            {
                if (_activateOnExit)
                {
                    _cooldownCoroutine = StartCoroutine(CooldownTimer());
                    OnExitAction(pawn);
                }
                _enteredTargets.Remove(pawn);
            }
        }

        protected virtual void OnEnterAction(PawnController pawn)
        {

        }

        protected virtual void OnStayAction(PawnController pawn)
        {

        }

        protected virtual void OnExitAction(PawnController pawn)
        {

        }

        protected IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(_cooldownTime);
            _cooldownCoroutine = null;
        }

        protected IEnumerator TriggerTimer()
        {
            WaitForSeconds delay = new(_triggerStayDelay);
            while (_enteredTargets.Count > 0)
            {
                foreach (PawnController pawn in _enteredTargets)
                {
                    OnStayAction(pawn);
                }
                yield return delay;
            }
            _triggerCoroutine = null;
        }
    }
}