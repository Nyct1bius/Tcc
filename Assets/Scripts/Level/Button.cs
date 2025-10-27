using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Button : MonoBehaviour,IHealth
{
    [SerializeField] private bool isActivated;
    [SerializeField] private bool isPressed;
    [SerializeField] Door doorToOpen;
    [SerializeField] private Door extraDoorToOpen;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private HorizontalMovePlatform horizontalMovePlatform;
    public SteppingTrigger steppingTrigger;

    private float activatedButton = -110f;
    public float activatedButtonYAdjust;
    public bool isRotated = false;
    [SerializeField] GameObject buttonLever;
    [SerializeField] private GameObject leverGameObject;
    private Material meshMaterial;
    [SerializeField] PlatformRaiser platformRaiser;

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

    [ContextMenu("PressTheButton")]
    public void PressTheButton()
    {
        if(isRotated)
        {
            buttonLever.transform.DORotate(new Vector3(activatedButton, activatedButtonYAdjust, 0), 1.5f);
        }
        else
            buttonLever.transform.DORotate(new Vector3(activatedButton, 0, 0), 1.5f);
        meshMaterial.SetColor("_Emission_Color_Variation", Color.green);



        if(platformRaiser != null)
        {
            platformRaiser.RaisePlatforms();
        }

        doorToOpen?.CheckIfAllButtonsIsActivated();
        extraDoorToOpen?.CheckIfAllButtonsIsActivated();
        if (horizontalMovePlatform != null)
            horizontalMovePlatform.enabled = true;
    }
    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        

        if (isActivated)
        {
            isActivated = false;
            PressTheButton();
            isPressed = true;
            steppingTrigger?.ButtonPressedDeactivation();
            Debug.Log("OpenDoor");
            doorToOpen?.CheckIfAllButtonsIsActivated();
            extraDoorToOpen?.CheckIfAllButtonsIsActivated();
            if(horizontalMovePlatform != null)
                horizontalMovePlatform.enabled = true;


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
