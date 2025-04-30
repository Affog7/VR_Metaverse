using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;      // Le joueur à suivre
    public Vector3 offset;        // Décalage de position (ex: derrière ou au-dessus)

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
