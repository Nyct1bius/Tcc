using UnityEngine;

public class Button : MonoBehaviour,IHealth
{
    [SerializeField] private bool isActivated;
    [SerializeField] private bool isPressed;
    [SerializeField] Door doorToOpen;
    [SerializeField] private Material newMat;
    private MeshRenderer meshRenderer;
    public SteppingTrigger steppingTrigger;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void HealHealth(float healing)
    {
        throw new System.NotImplementedException();
    }

    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        if (isActivated)
        {
            isPressed = true;
            steppingTrigger?.ButtonPressedDeactivation();
            Debug.Log("OpenDoor");
            doorToOpen.CheckIfAllButtonsIsActivated();
            meshRenderer.material = newMat;
        }
    }

  
    public void ActivatedButton()
    {
        isActivated = true;
    }

    public void DeactivatedButton()
    {
        if(!isPressed)
            isActivated = false;
    }


}
