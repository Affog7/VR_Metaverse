using UnityEngine;

public class CollectItems : MonoBehaviour
{
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            Debug.Log("Objet collecté !");
            scoreManager.AddScore(1);
            scoreManager.AddLife(10); // Gagne 10 PV
        }
        
        if (other.CompareTag("Heart"))
        {
            Destroy(other.gameObject);
            Debug.Log("Objet collecté !");
            scoreManager.AddLife(25); // Gagne 25 PV
        }
    }
}
