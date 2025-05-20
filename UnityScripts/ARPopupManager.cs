using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ARPopupManager : MonoBehaviour
{
    public static ARPopupManager Instance;

    [Header("Popup UI")]
    public GameObject popupPanel;
    public TMP_Text buildingNameText;
    public TMP_Text confidenceText;
    public TMP_Text buildingInfoText;
    public Button closeButton; // optional

    private int currentBuildingId = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    // Overload #1: If some scripts call (buildingId, float)
    public void ShowPopup(int buildingId, float confidence)
    {
        ShowPopup(buildingId, confidence, Vector3.zero);
    }

    // Overload #2: If some scripts call (buildingId, float, Vector3)
    public void ShowPopup(int buildingId, float confidence, Vector3 position)
    {
        currentBuildingId = buildingId;
        Building building = BuildingDatabase.Instance.GetBuildingById(buildingId);
        if (building == null)
        {
            Debug.LogError($"ARPopupManager: Building with ID {buildingId} not found.");
            return;
        }

        if (buildingNameText != null)
            buildingNameText.text = building.name;

        if (confidenceText != null)
            confidenceText.text = $"Confidence: {(confidence * 100f):0.0}%";

        if (buildingInfoText != null)
        {
            string info = building.description;
            if (building.floors != null && building.floors.Length > 0)
            {
                info += "\n\nFloors:\n";
                foreach (var floor in building.floors)
                {
                    info += $"- Floor {floor.floorNumber} ({floor.floorName}): {floor.floorDescription}\n";
                }
            }
            buildingInfoText.text = info;
        }

        popupPanel.SetActive(true);
    }

    // Overload #3: If your ARBuilding calls (buildingId, Vector3)
    public void ShowPopup(int buildingId, Vector3 position)
    {
        ShowPopup(buildingId, 1.0f, position);
    }

    public void HidePopup()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    // Optional close button
    public void OnClick_Close()
    {
        HidePopup();
    }
}
