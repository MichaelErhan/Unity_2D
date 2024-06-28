using System.Collections;
using UnityEngine;
using DG.Tweening;

public class WalkState : StateMachineBehaviour
{
    private CharacterMovement _characterMovement;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_characterMovement == null)
        {
            _characterMovement = animator.GetComponent<CharacterMovement>();
        }

        _characterMovement.MoveToNextWaypoint(); // Начало движения к следующей точке
    }
}