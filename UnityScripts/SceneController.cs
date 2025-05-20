using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Options");
    }
}
