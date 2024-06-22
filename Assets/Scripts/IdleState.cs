using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    private float idleTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer = 0f; // Обнуление таймера при входе в состояние Idle
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer += Time.deltaTime;
        animator.SetFloat("IdleTime", idleTimer); // Обновление параметра IdleTime
    }
}
