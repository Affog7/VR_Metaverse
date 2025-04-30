using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Perdu ! Tu as touché un ennemi.");
            scoreManager.RemoveLife(25); // Perd 25 PV
        }
    }
}
