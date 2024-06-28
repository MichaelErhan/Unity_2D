using System.Collections;
using UnityEngine;
using DG.Tweening;

public class IdleState : StateMachineBehaviour
{
    private CharacterMovement _characterMovement;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterMovement == null)
        {
            _characterMovement = animator.GetComponent<CharacterMovement>();
        }

        _characterMovement.StartCoroutine(IdleBeforeNextMove(animator));
    }

    private IEnumerator IdleBeforeNextMove(Animator animator)
    {
        yield return new WaitForSeconds(_characterMovement.IdleTime); // Задержка на точке
        animator.SetBool(CharacterMovement.IsWalking, true); // Переход в состояние Walk
    }
}