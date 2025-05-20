using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Prediction
{
    public List<float> bbox; // Bounding Box [x1, y1, x2, y2]
    public string class_name;
    public float confidence;
}

[System.Serializable]
public class PredictionResponse
{
    public List<Prediction> predictions;
}

public class FlaskAPIClient : MonoBehaviour
{
    private string serverURL = "http://176.97.218.195:5000/predict"; // Server URL using HTTP

    public Camera captureCamera; // Renamed from "camera" to avoid hiding the inherited member.
    public RenderTexture renderTexture;
    public GameObject buildingInfoPanel;
    public TMPro.TextMeshProUGUI infoText;

    void Start()
    {
        buildingInfoPanel.SetActive(false); // Hide panel initially
    }

    public void CaptureAndSend()
    {
        Debug.Log("Starting image capture...");
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        captureCamera.Render();
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        Debug.Log("Image captured successfully.");

        byte[] imageData = texture.EncodeToJPG();
        Debug.Log("Image encoded to JPG.");

        StartCoroutine(SendImageToServer(imageData));
    }

    public IEnumerator SendImageToServer(byte[] imageData)
    {
        Debug.Log("Sending image data to server...");
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", imageData, "image.jpg", "image/jpeg");

        using (UnityWebRequest request = UnityWebRequest.Post(serverURL, form))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error sending image: " + request.error);
            }
            else
            {
                Debug.Log("Image sent successfully. Server response received:");
                Debug.Log("Response: " + request.downloadHandler.text);
                ProcessServerResponse(request.downloadHandler.text);
            }
        }
    }

    private void ProcessServerResponse(string jsonResponse)
    {
        Debug.Log("Processing server response...");
        PredictionResponse response = JsonUtility.FromJson<PredictionResponse>(jsonResponse);

        foreach (Prediction pred in response.predictions)
        {
            Debug.Log($"Detected: {pred.class_name} - Confidence: {pred.confidence}");
            DrawBoundingBox(pred.bbox, pred.class_name);
        }
    }

    private void DrawBoundingBox(List<float> bbox, string label)
    {
        float x1 = bbox[0], y1 = bbox[1], x2 = bbox[2], y2 = bbox[3];
        Debug.Log($"Bounding Box: {x1}, {y1}, {x2}, {y2}");

        // Show panel and building info
        buildingInfoPanel.SetActive(true);
        infoText.text = $"Building: {label}";
    }
}
