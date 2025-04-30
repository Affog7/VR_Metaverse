using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 5f;

    private Vector3 startPosition;
    private bool goingForward = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (goingForward)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        else
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= distance)
            goingForward = !goingForward;
    }
}
