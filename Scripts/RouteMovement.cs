using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float height;
    [SerializeField] public float movementSpeed;
    public GameObject rout;
    public Transform currentTarget;
    public float distanceToNext;
    public float switchToNextTargetDistance;
    private int index = 0;
    public Transform[] allPoints;
    public float originalMovementSpeed;
    void Start()
    {
        originalMovementSpeed = movementSpeed;
        allPoints = rout.gameObject.GetComponentsInChildren<Transform>();
        currentTarget = allPoints[index];
        transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckTarget();
        Move();
    }

    void Move()
    {
        distanceToNext = (currentTarget.position - transform.position).magnitude;
        if (transform.position.x < currentTarget.position.x) transform.position += Vector3.right * movementSpeed;
        if (transform.position.x > currentTarget.position.x) transform.position += Vector3.left * movementSpeed;
        if (transform.position.z < currentTarget.position.z) transform.position += Vector3.forward * movementSpeed;
        if (transform.position.z > currentTarget.position.z) transform.position += Vector3.back * movementSpeed;
    }

    private void CheckTarget()
    {
        if (distanceToNext < switchToNextTargetDistance)
        {
            transform.position = currentTarget.position;
            var temp = transform.position;
            temp.y = height;
            transform.position = temp;
            if (index + 1 == allPoints.Length)
            {
                GameController.RemoveHealth();
                Destroy(gameObject);
                return;
            }
            index++;
            currentTarget = allPoints[index];
        }
    }
    
}