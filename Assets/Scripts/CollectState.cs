using UnityEngine;
using UnityEngine.AI;

public class CollectState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private GameObject target;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Collectible");
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            Debug.Log("Путь установлен к объекту: " + target.name);
        }
        else
        {
            Debug.Log("Объект с тегом 'Collectible' не найден.");
            agent.transform.position = new Vector3(-9.02f, 0.44f, 20.13f);
            agent.isStopped = true;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target != null && Vector3.Distance(agent.transform.position, target.transform.position) <= agent.stoppingDistance)
        {
            // Остановка агента перед сбором
            agent.isStopped = true;

            // Удаление объекта
            Destroy(target);
            Debug.Log($"Удален объект: {target}");

            // Убедитесь, что объект удален, чтобы не пытаться снова к нему подойти
            target = null;

            // Переход в Idle состояние после сбора
            animator.SetTrigger("Idle");
        }
        else if (target != null)
        {
            Debug.Log("Подход к объекту еще не завершен: расстояние - " + Vector3.Distance(agent.transform.position, target.transform.position));
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false; // Включаем агента снова при выходе из состояния
        animator.ResetTrigger("Idle"); // Сброс параметра Idle при выходе из состояния
    }
}