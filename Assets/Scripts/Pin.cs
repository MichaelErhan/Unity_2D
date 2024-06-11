using TMPro;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private bool _done;

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.collider.CompareTag("Ball") || collision.collider.CompareTag("Pin")) && !_done)
        {
            float velocity = GetComponent<Rigidbody>().velocity.magnitude;

            if (velocity < 10)
            {
                var point = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>().Point;
                point += 1;
                GameObject.FindGameObjectWithTag("Poing").GetComponent<TextMeshProUGUI>().text = $"Сбито кеглей: {point}";
                GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>().Point = point;
                _done = true;

                var score = 0;

                if (point == 10)
                {
                    score = point + 10;
                    GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = $"Очков: {score}";
                }
                else
                {
                    score += point;
                    GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = $"Очков: {score}";
                }
            }
        }
    }
}