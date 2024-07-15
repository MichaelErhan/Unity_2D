using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // Ссылка на трансформ двери
    public Vector3 openOffset; // Относительное смещение для открывания двери
    public float openSpeed = 2f; // Скорость открытия двери

    private Vector3 closedPosition; // Начальная закрытая позиция двери
    private Vector3 openPosition; // Конечная открытая позиция двери
    private bool isOpening = false; // Флаг для отслеживания состояния двери

    void Start()
    {
        closedPosition = door.position;
        openPosition = closedPosition + openOffset; // Установка конечной позиции двери
    }

    void Update()
    {
        if (isOpening && door.position != openPosition)
        {
            door.position = Vector3.MoveTowards(door.position, openPosition, openSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
        }
    }
}