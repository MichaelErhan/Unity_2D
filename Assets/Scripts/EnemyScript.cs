using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private Transform _transform;

    // Переменная для хранения радиуса обзора противника
    public float viewRadius = 10.0f;

    // Вектор для хранения центра обзора противника
    public Vector3 viewCenter;

    void Start()
    {
        viewRadius = 2.0f;
        viewCenter = _transform.position + _transform.forward * viewRadius;
    }

    void Update()
    {
        // Обновление центра обзора
        viewCenter = _transform.position + _transform.forward * viewRadius;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Рисуем сферу с центром в позиции противника и заданного радиуса
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
