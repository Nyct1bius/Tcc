using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [Header("FMOD Events")]
    public EventReference hoverSound;
    public EventReference clickSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClick();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayHover();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayClick();
    }

    private void PlayHover()
    {
        if(!hoverSound.IsNull)
        {
            RuntimeManager.PlayOneShot(hoverSound);
        }
    }

    private void PlayClick()
    {
        if (!clickSound.IsNull)
            RuntimeManager.PlayOneShot(clickSound);
    }
}
