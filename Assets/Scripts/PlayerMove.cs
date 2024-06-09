using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerMove : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 mouseWorldPosition;
    private Vector3 targetPosition;

    void Update()
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        targetPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
