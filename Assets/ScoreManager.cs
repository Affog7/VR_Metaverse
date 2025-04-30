using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text lifeText;

    private int score = 0;
    private int life = 100;

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    public void AddLife(int value)
    {
        life += value;
        if (life > 100) life = 100; // max 100
        UpdateUI();
    }

    public void RemoveLife(int value)
    {
        life -= value;
        if (life < 0) life = 0; // min 0
        UpdateUI();

        if (life <= 0)
        {
            Debug.Log("Tu es mort ! Game Over.");
            // TODO : Afficher un écran de fin ou relancer la scène
        }
    }

    void UpdateUI()
    {
        scoreText.text = "Score : " + score;
        lifeText.text = "Vie : " + life;
    }
}
