using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public float speed = 2.5f;
    public float raycastDistance = 0.6f;
    public LayerMask obstacleLayer;
    
    public float pickupRadius = 1.0f;
    public LayerMask trashLayer;

    private Vector3 direction;
    private bool foundObstacle;
    private float changeDirectionTimer;

    void Start()
    {
        direction = RandomDirection();
        changeDirectionTimer = Random.Range(2.0f, 7.0f);
    }

    void Update()
    {
        Move(); 
        CheckForObstacles(); 
        PickUpTrash(); 
        
        changeDirectionTimer -= Time.deltaTime;
        if (changeDirectionTimer <= 0)
        {
            direction = RandomDirection();
            changeDirectionTimer = Random.Range(2.0f, 7.0f);
        }
    }

    void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void CheckForObstacles()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red, 0.1f);
            foundObstacle = true;
            ChangeDirection(); 
        }

        if (Physics.Raycast(transform.position, transform.right, out hit, raycastDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, transform.right * raycastDistance, Color.green, 0.1f);
            foundObstacle = true;
            ChangeDirection();
        }

        if (Physics.Raycast(transform.position, -transform.right, out hit, raycastDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, -transform.right * raycastDistance, Color.blue, 0.1f);
            foundObstacle = true;
            ChangeDirection();
        }
        

        if (Physics.Raycast(transform.position, -transform.forward, out hit, raycastDistance, obstacleLayer))
        {
            Debug.DrawRay(transform.position, -transform.forward * raycastDistance, Color.yellow, 0.1f);
            foundObstacle = true;
            ChangeDirection();
        }

        if (!foundObstacle)
        {
            foundObstacle = false;    
        }
    }
    void ChangeDirection()
    {
        if (foundObstacle)
        {
            direction = RandomDirection();
        }
    }

    Vector3 RandomDirection()
    {
        int randomIndex = Random.Range(0, 4);
        switch (randomIndex)
        {
            case 0:
                return Vector3.right;
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.forward;
            case 3:
                return Vector3.back;
            default:
                return Vector3.forward;
        }
    }

    void PickUpTrash()
    {
        Collider[] trashColliders = Physics.OverlapSphere(transform.position, pickupRadius, trashLayer);
        if (trashColliders.Length > 0)
        {
            foreach (Collider trashCollider in trashColliders)
            {
                Destroy(trashCollider.gameObject);
            }
        }
    }
}
