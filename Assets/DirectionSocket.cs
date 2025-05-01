using NativeWebSocket;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using TMPro;

public class DirectionSocket : MonoBehaviour
{
    public TMP_InputField champTexte;
    private WebSocket websocket;
    private GameObject joueur;

    [Serializable]
    public class ActionResponse
    {
        public string action;
    }

    async void Start()
    {
        joueur = GameObject.FindGameObjectWithTag("Player");

        websocket = new WebSocket("ws://localhost:8768");

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("Message du serveur : " + message);

            ActionResponse response = JsonUtility.FromJson<ActionResponse>(message);

            if (joueur != null)
            {
                var controleur = joueur.GetComponent<PlayerController>();
                if (controleur != null)
                {
                    switch (response.action)
                    {
                        case "forward":
                            controleur.Avancer();
                            break;
                        case "rotate_right":
                            controleur.TournerDroite();
                            break;
                        case "rotate_left":
                            controleur.TournerGauche();
                            break;
                        default:
                            Debug.LogWarning("Action inconnue reçue : " + response.action);
                            break;
                    }
                }
            }
        };

        await websocket.Connect();
    }

    public async void OnSendClicked()
    {
        string texte = champTexte.text;
        if (websocket.State == WebSocketState.Open && !string.IsNullOrWhiteSpace(texte))
        {
            await websocket.SendText(texte);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }
}
