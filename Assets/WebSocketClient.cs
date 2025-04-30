using UnityEngine;
using NativeWebSocket;
using System.Collections.Generic;

public class WebSocketClient : MonoBehaviour
{
    WebSocket websocket;

    [System.Serializable]
    public class MouseData
    {
        public float x;
    }

    [System.Serializable]
    public class ShapeResponse
    {
        public string shape;
    }

    private string currentShape = ""; // forme actuelle
    private string lastReceivedShape = ""; // forme déjà affichée
    private float lastMouseX = -1f; // position x précédente
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        websocket = new WebSocket("ws://localhost:8765");

        websocket.OnOpen += () => {
            Debug.Log("Connexion WebSocket ouverte.");
        };

        websocket.OnMessage += (bytes) => {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message reçu : " + message);

            string shape = JsonUtility.FromJson<ShapeResponse>(message).shape;

            // Ne rien faire si la forme est la même que la précédente
            if (shape == lastReceivedShape)
                return;

            // Sinon : supprimer les anciens objets et en créer un nouveau
            ClearObjects();
            lastReceivedShape = shape;
            CreateShape(shape);
        };

        websocket.Connect();
    }

    void Update()
    {
        if (websocket != null)
            websocket.DispatchMessageQueue();

        float mouseX = Input.mousePosition.x / Screen.width;

        // Seuil de changement significatif (évite les micro-mouvements)
        if (Mathf.Abs(mouseX - lastMouseX) > 0.01f)
        {
            lastMouseX = mouseX;

            MouseData data = new MouseData();
            data.x = mouseX;
            string json = JsonUtility.ToJson(data);
            websocket.SendText(json);
        }
    }

    void CreateShape(string shape)
    {
        GameObject obj = null;
        switch (shape)
        {
            case "cube":
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "sphere":
                obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case "cylinder":
                obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
        }

        if (obj != null)
        {
            obj.transform.position = new Vector3(Random.Range(-3f, 3f), 1f, 0);
            spawnedObjects.Add(obj);
        }
    }

    void ClearObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
