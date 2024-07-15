using UnityEngine;
public class TrapScript : MonoBehaviour
{
    public int damageAmount = 100; // Количество урона, который ловушка наносит
    private Rigidbody _rb;
    private AudioSource _audioSource;

     void Start()
     {
         _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
     }
     private void OnTriggerEnter(Collider collision)
     {
         if (collision.gameObject.name.Equals("CharacterThirdPerson"))
         {
             _rb.isKinematic = false;
         }
     }
     private void OnCollisionEnter(Collision collision)
     {
         if (collision.gameObject.name.Equals("CharacterThirdPerson"))
         {
             Debug.Log("Trap!!!");
            _audioSource.Play();
             PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
             if (playerHealth != null)
             {
                 playerHealth.TakeDamage(damageAmount, Vector3.zero); 
             }

         }

     }
}
