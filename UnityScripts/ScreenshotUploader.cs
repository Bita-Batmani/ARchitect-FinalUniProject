using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

namespace MyProject
{
    [System.Serializable]
    public class Floor
    {
        public int floorNumber;
        public string floorName;
        public string floorDescription;
    }

    [System.Serializable]
    public class PredictionResult
    {
        public float[] bbox;
        public string class_name;
        public float confidence;
        public int building_id;
        public string display_name;
        public string description;
        public Floor[] floors;
    }

    [System.Serializable]
    public class PredictionsWrapper
    {
        public PredictionResult[] predictions;
    }

    public class ScreenshotUploader : MonoBehaviour
    {
        [Header("Server Settings")]
        [SerializeField] private string serverUrl = "http://176.97.218.195:5000/predict";

        [Header("UI References")]
        [Tooltip("TextMeshPro field to show the current status (Loading, Processing, etc.)")]
        public TMP_Text statusText;

        public void CaptureAndUpload()
        {
            SetStatus("Capturing screenshot...");
            StartCoroutine(TakeScreenshotAndUpload());
        }

        private IEnumerator TakeScreenshotAndUpload()
        {
            yield return new WaitForEndOfFrame();

            SetStatus("Processing screenshot...");

            // Capture screenshot
            Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenshot.Apply();

            byte[] imageBytes = screenshot.EncodeToPNG();
            Destroy(screenshot);

            // Prepare form data
            WWWForm form = new WWWForm();
            form.AddBinaryData("image", imageBytes, "screenshot.png", "image/png");

            SetStatus("Uploading to server...");

            // Send request
            using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log("Image uploaded successfully!");
                    Debug.Log("Response: " + request.downloadHandler.text);
                    SetStatus("Server response received.");
                    ProcessServerResponse(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Failed to upload image: " + request.error);
                    SetStatus($"Upload error: {request.error}");
                }
            }
        }

        private void ProcessServerResponse(string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse))
            {
                SetStatus("No response from server.");
                return;
            }

            PredictionsWrapper predictions = JsonUtility.FromJson<PredictionsWrapper>(jsonResponse);
            if (predictions == null || predictions.predictions == null || predictions.predictions.Length == 0)
            {
                SetStatus("No predictions in server response.");
                return;
            }

            PredictionResult first = predictions.predictions[0];

            // Check for confidence threshold
            if (first.confidence < 0.2f)
            {
                SetStatus($"Confidence too low ({first.confidence * 100f:0.0}%). No match found.");
                return;
            }

            // Use ARPopupManager with building details (if you want to use the extra fields)
            ARPopupManager.Instance.ShowPopup(first.building_id, first.confidence);

            // Alternatively, if you want to use additional details directly:
            Debug.Log($"Detected {first.class_name} - {first.display_name}: {first.description}");
            SetStatus($"Detected {first.class_name} ({first.confidence * 100f:0.0}%).");
        }

        private void SetStatus(string msg)
        {
            Debug.Log("Status: " + msg);
            if (statusText != null)
            {
                statusText.text = msg;
            }
        }
    }
}
