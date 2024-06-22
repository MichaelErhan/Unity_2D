using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _raycastLenth;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _damage;
    [SerializeField] private float _pushForce;

    private RaycastHit _hitInfo;

    public float Damage => _damage;

    private void Update()
    {
        Ray ray = new Ray(_shootPoint.position, transform.position);

        Debug.DrawRay(ray.origin, ray.direction * 10);

        if(Physics.Raycast(ray, out _hitInfo))
        {
            Rigidbody rb = _hitInfo.collider.GetComponent<Rigidbody>();

            if(rb) 
            {
                rb.AddForce(_hitInfo.normal * _pushForce, ForceMode.Impulse);
            }
        }
    }
}
