using System.Collections.Generic;
using UnityEngine;

// If you have floors in your JSON:
[System.Serializable]
public class FloorInfo
{
    public int floorNumber;
    public string floorName;
    public string floorDescription;
}

[System.Serializable]
public class Building
{
    public int id;
    public string name;
    public string image;
    public string description;
    public FloorInfo[] floors; // optional array for floors
}

[System.Serializable]
public class BuildingList
{
    public Building[] buildings;
}

public class BuildingDatabase : MonoBehaviour
{
    public static BuildingDatabase Instance;

    [Tooltip("If null, will try to load 'buildings.json' from Resources folder.")]
    public TextAsset buildingsJson;

    private Dictionary<int, Building> buildingDict = new Dictionary<int, Building>();

    private void Awake()
    {
        // Simple singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBuildings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadBuildings()
    {
        if (buildingsJson == null)
        {
            // Try loading from Resources if not assigned
            buildingsJson = Resources.Load<TextAsset>("buildings");
        }

        if (buildingsJson == null)
        {
            Debug.LogError("BuildingDatabase: Could not find buildings.json in Resources or assigned in Inspector!");
            return;
        }

        BuildingList list = JsonUtility.FromJson<BuildingList>(buildingsJson.text);
        if (list != null && list.buildings != null)
        {
            foreach (Building b in list.buildings)
            {
                buildingDict[b.id] = b;
            }
            Debug.Log($"BuildingDatabase: Loaded {buildingDict.Count} buildings.");
        }
        else
        {
            Debug.LogError("BuildingDatabase: Failed to parse buildings.json or it's empty.");
        }
    }

    public Building GetBuildingById(int buildingId)
    {
        buildingDict.TryGetValue(buildingId, out Building building);
        return building;
    }
}
