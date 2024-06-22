using UnityEngine;
using UnityEngine.AI;

public class SearchState : StateMachineBehaviour
{
    public float searchRadius = 7f;
    private NavMeshAgent agent;
    private GameObject target;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        MoveToRandomLocation();
    }

    private void MoveToRandomLocation()
    {
        Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
        randomDirection += agent.transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToRandomLocation();
        }

        // Проверка на наличие предметов для сбора в радиусе
        Collider[] hitColliders = Physics.OverlapSphere(agent.transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Collectible"))
            {
                target = hitCollider.gameObject;
                agent.SetDestination(target.transform.position);
                break; // Прекращаем проверку, так как нашли цель
            }
        }

        // Проверяем, достиг ли агент цели
        if (target != null && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            animator.SetTrigger("Collect");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.isStopped = false; // Включаем агент снова при выходе из состояния
    }
}
