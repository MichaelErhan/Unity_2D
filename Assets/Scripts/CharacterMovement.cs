using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
    public Transform[] waypoints; // Массив контрольных точек
    public float duration = 5f; // Продолжительность анимации
    public Ease easeType = Ease.InOutQuad; // Тип кривой изменения
    public bool loop = true; // Зациклить анимацию
    public ParticleSystem dustEffect; // Эффект пыли
    public Animator animator; // Аниматор персонажа
    public Material playerMaterial;

    private NavMeshAgent navAgent;
    private int currentWaypointIndex = 0;
    private bool isWalking = false;
    private SkinnedMeshRenderer characterRenderer; // Рендерер персонажа
    private Vector3 initialScale; // Начальный масштаб персонажа

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>(); // Находим рендерер в дочерних объектах
        initialScale = transform.localScale; // Сохранение начального масштаба
        playerMaterial.color = new Color(0.219f, 0.219f, 0.219f, 1);

        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        navAgent.SetDestination(waypoints[currentWaypointIndex].position);
        animator.SetBool("isWalking", true); // Включение анимации ходьбы

        navAgent.isStopped = false;
        isWalking = true;
    }

    void Update()
    {
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex < waypoints.Length)
            {
                if (currentWaypointIndex == 0)
                {
                    // Вернуть масштаб персонажа к исходному в нулевой точке
                    transform.DOScale(initialScale, duration).SetEase(easeType);
                }
                else
                {
                    float newColorR = Mathf.Clamp01(playerMaterial.color.r + 0.03f);
                    float newColorB = Mathf.Clamp01(playerMaterial.color.b + 0.03f);
                    Color newColorPL = new Color(newColorR, playerMaterial.color.g, newColorB, playerMaterial.color.a);

                    // Увеличить масштаб персонажа на +0.1 на каждой точке
                    transform.DOScale(initialScale * (1 + 0.1f * currentWaypointIndex), duration).SetEase(easeType);
                    playerMaterial.DOColor(newColorPL, duration);

                }

                MoveToNextWaypoint();
            }
            else
            {
                if (loop)
                {
                    currentWaypointIndex = 0;
                    MoveToNextWaypoint();
                }
                else
                {
                    animator.SetBool("isWalking", false); // Отключение анимации ходьбы
                    navAgent.isStopped = true;
                    isWalking = false;

                    // Вернуть масштаб персонажа при остановке
                    transform.DOScale(initialScale, duration).SetEase(easeType);
                }
            }
        }

        // Управление эффектом пыли в зависимости от состояния движения
        if (isWalking)
        {
            if (dustEffect != null && !dustEffect.isPlaying)
            {
                dustEffect.Play(); // Включение эффекта пыли
            }
        }
        else
        {
            if (dustEffect != null && dustEffect.isPlaying)
            {
                dustEffect.Stop(); // Остановка эффекта пыли
                dustEffect.Clear(); // Полная остановка и очистка эффекта пыли
            }
        }
    }
    public void OnFootstep()
    {
    }
}
