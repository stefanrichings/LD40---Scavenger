using TMPro;
using UnityEngine;

public class HoldMessage : MonoBehaviour
{
    public TextMeshProUGUI message;
    public TextMeshProUGUI key;

    public string Message
    {
        get
        {
            return currentMessage;
        }
    }
    string currentMessage;

    public void Show(string msg, string k)
    {
        currentMessage = msg;
        gameObject.SetActive(true);
        message.text = "Hold to " + msg.ToLower();
        key.text = k;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
