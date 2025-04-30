using NativeWebSocket;
using UnityEngine;

public class DirectionSocket : MonoBehaviour
{
    WebSocket websocket;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8768");

        websocket.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message du serveur : " + message);

            // Ici tu peux déclencher l’action correspondante dans Unity
        };

        await websocket.Connect();
    }

    public async void SendCommand(string command)
    {
        await websocket.SendText(command);
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
