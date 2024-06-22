using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private float _currentHealth;
    private Ragdoll _ragdoll;

    private void Start()
    {
        _ragdoll = GetComponent<Ragdoll>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        _currentHealth -= amount;

        if(_currentHealth <= 0) 
        {
            Die();
        }
    }

    private void Die() 
    {
        _ragdoll.ActivateRagdoll();
    }
}
