using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    public static Notifier instance;
    public GameObject notificationText;
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void NotifyGreen(string message)
    {
        GameObject newNotification = Instantiate(notificationText);
        newNotification.GetComponent<RectTransform>().SetParent(instance.transform);
        newNotification.GetComponent<RectTransform>().localScale = Vector3.one;
        newNotification.GetComponent<TextMeshProUGUI>().text = message;
        newNotification.GetComponent<TextMeshProUGUI>().color = Color.green;
    }
    
}
