using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class SentimentClient : MonoBehaviour
{
    public TMP_InputField inputField;   // Champ où on tape le texte
    public TMP_Text resultText;         // Où on affiche le sentiment
    public string apiUrl = "http://127.0.0.1:5000/predict"; // URL de ton API Flask

    // Appelé quand on clique sur le bouton
    public void OnSendClicked()
    {
        string userMessage = inputField.text;
        if (!string.IsNullOrEmpty(userMessage))
        {
            StartCoroutine(SendToApi(userMessage));
        }
        else
        {
            resultText.text = "⚠️ Entrez un message d'abord.";
        }
    }

    IEnumerator SendToApi(string message)
    {
        // Prépare les données en JSON
        string json = "{\"text\":\"" + message + "\"}";
        byte[] body = System.Text.Encoding.UTF8.GetBytes(json);

        // Prépare la requête HTTP POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Envoie la requête et attend la réponse
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse la réponse JSON
            SentimentResponse response = JsonUtility.FromJson<SentimentResponse>(request.downloadHandler.text);
            Debug.Log(response);
            resultText.text = "🧠 Emotion : " + response.sentiment;
        }
        else
        {
            resultText.text = "❌ Erreur : " + request.error;

        }
    }

    [System.Serializable]
    public class SentimentResponse
    {
        public string sentiment;
    }
}
