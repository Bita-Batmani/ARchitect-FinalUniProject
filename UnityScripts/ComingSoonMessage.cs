using UnityEngine;
using TMPro;

public class ComingSoonMessage : MonoBehaviour
{
    public TextMeshProUGUI comingSoonText;   
    public string message = "Feature coming soon!";

    private void Awake()
    {
        if (comingSoonText != null) 
        {
            comingSoonText.text = "";
            comingSoonText.gameObject.SetActive(false);
        }
    }

    public void ShowComingSoon()
    {
        if (comingSoonText != null)
        {
            comingSoonText.text = message;
            comingSoonText.gameObject.SetActive(true);
        }
    }

    public void HideComingSoon()
    {
        if (comingSoonText != null)
            comingSoonText.gameObject.SetActive(false);
    }
}
