using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Destructible : MonoBehaviour
{
    public float health = 100f; // Здоровье объекта

    public void TakeDamage(float amount)
    {
        health -= amount; // Уменьшаем здоровье объекта

        if (health <= 0f)
        {
            Destroy(gameObject);       // Объект разрушен
        }
    }
}
