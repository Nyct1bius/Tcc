using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("FMOD Events")]
    public EventReference hoverSound;
    public EventReference clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hoverSound.IsNull)
            RuntimeManager.PlayOneShot(hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickSound.IsNull)
            RuntimeManager.PlayOneShot(clickSound);
    }
}
