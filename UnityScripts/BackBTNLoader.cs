using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackBTNLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("starter page");
    }
}
