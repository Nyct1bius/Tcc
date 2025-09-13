using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour,IHealth
{
    [SerializeField] private bool isActivated;
    [SerializeField] private bool isPressed;
    [SerializeField] Door doorToOpen;
    [SerializeField] private MeshRenderer meshRenderer;
    public SteppingTrigger steppingTrigger;

    private float activatedButton = -110f;
    [SerializeField] GameObject buttonLever;
    [SerializeField] private GameObject leverGameObject;
    private Material meshMaterial;

    private void Start()
    {
        meshMaterial = meshRenderer.material;
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
            isActivated = false;
            isPressed = true;
            steppingTrigger?.ButtonPressedDeactivation();
            Debug.Log("OpenDoor");
            doorToOpen.CheckIfAllButtonsIsActivated();

            buttonLever.transform.DORotate(new Vector3(activatedButton, 0, 0), 1.5f);
            meshMaterial.SetColor("_Emission_Color_Variation", Color.green);
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
