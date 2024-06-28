using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;  // Массив точек пути
    [SerializeField] private float _duration = 3f;    // Время движения к следующей точке
    [SerializeField] private float _idleTime = 1.5f;    // Время ожидания на точке
    [SerializeField] private float _rotationSpeed = 80f; // Скорость поворота персонажа
    [SerializeField] private bool _loop = true;      // Флаг для зацикливания движения
    private int _currentWaypointIndex = 0; // Текущий индекс точки пути
    [SerializeField] private Animator _animator; // Компонент Animator
    [SerializeField] private ParticleSystem _dustEffect; // Эффект пыли
    [SerializeField] private Material _playerMaterial; // Материал игрока

    public static readonly int IsWalking = Animator.StringToHash("isWalking"); // Кэшируем анимационный параметр

    public float IdleTime => _idleTime; // Открытое свойство для доступа к idleTime

    private void Start()
    {
        _animator = GetComponent<Animator>(); // Получение компонента Animator
        _playerMaterial.color = new Color(0.219f, 0.219f, 0.219f, 1); // Устанавливаем цвет в начальный
        _animator.SetBool(IsWalking, true); // Запуск с состояния Walk
    }

    public void MoveToNextWaypoint()
    {
        if (_currentWaypointIndex < _waypoints.Length)
        {
            Vector3 direction = (_waypoints[_currentWaypointIndex].position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                StartCoroutine(RotateTo(targetRotation)); // Запуск корутины на плавный поворот
            }

            _animator.SetBool(IsWalking, true); // Устанавливаем состояние Walk

            if (_dustEffect != null && !_dustEffect.isPlaying)
            {
                _dustEffect.Play(); // Включаем эффект пыли
            }

            // Создаем последовательность анимаций с использованием DOTween
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), _duration / 10).SetEase(Ease.InOutQuad)) // Увеличение масштаба
                      .Join(transform.DOMove(_waypoints[_currentWaypointIndex].position, _duration).SetEase(Ease.Linear)) // Движение к точке
                      .Append(transform.DOScale(Vector3.one, _duration / 10).SetEase(Ease.InOutQuad)) // Возвращение масштаба
                      .OnComplete(OnReachedWaypoint); // Вызов функции по завершении анимации
        }
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnReachedWaypoint()
    {
        _animator.SetBool(IsWalking, false); // Устанавливаем состояние Idle

        if (_dustEffect != null && _dustEffect.isPlaying)
        {
            _dustEffect.Stop(); // Останавливаем эффект пыли
            _dustEffect.Clear(); // Полная остановка и очистка эффекта пыли
        }

        float newColorR = Mathf.Clamp01(_playerMaterial.color.r + 0.03f);
        float newColorB = Mathf.Clamp01(_playerMaterial.color.b + 0.03f);
        Color newColorPL = new Color(newColorR, _playerMaterial.color.g, newColorB, _playerMaterial.color.a);

        _playerMaterial.DOColor(newColorPL, _duration / 10);

        _currentWaypointIndex++; // Переход к следующей точке

        if (_currentWaypointIndex >= _waypoints.Length)
        {
            if (_loop)
            {
                _currentWaypointIndex = 0; // Вернуться к первой точке, если зацикливание включено
            }
            else
            {
                return; // Выйти из метода, если зацикливание выключено и достигнута последняя точка
            }
        }
    }

    public void OnFootstep()
    {
    }
}