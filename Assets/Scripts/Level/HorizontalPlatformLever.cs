using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HorizontalPlatformLever : MonoBehaviour, IHealth
{
    public HorizontalMovePlatform platform;
    public int indexToCheck = 0;
    public int indexToSet = 0;
    public bool isMoving = false;
    public float timeToWait;

    private float activatedButton = -110f;
    public float activatedButtonYAdjust;
    public bool isRotated = false;
    [SerializeField] GameObject buttonLever;
    [SerializeField] private GameObject leverGameObject;


    [SerializeField] private MeshRenderer meshRenderer;
    private Material meshMaterial;


    private void Start()
    {
        meshMaterial = meshRenderer.material;
        meshMaterial.SetColor("_Emission_Color_Variation", Color.green);
    }
    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CallPlatform();
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void HealHealth(float healing)
    {
        throw new System.NotImplementedException();
    }

    [ContextMenu("CallPlatform")]
    public void CallPlatform()
    {
        if (platform.currentIndex != indexToCheck && isMoving == false)
        {
            isMoving = true;
            PressTheButton();
            platform.currentIndex = indexToSet;
            platform.MovePlatform();
        }
        else
            return;
        
    }

    public void PressTheButton()
    {
        if (isRotated)
        {
            buttonLever.transform.DORotate(new Vector3(activatedButton, activatedButtonYAdjust, 0), 1.5f);
        }
        else
            buttonLever.transform.DORotate(new Vector3(activatedButton, 0, 0), 1.5f);

        StartCoroutine(ReturnToOriginalPos(isRotated));
    }

    IEnumerator ReturnToOriginalPos(bool isRot)
    {
        yield return new WaitForSeconds(timeToWait);
        if (isRot)
        {
            buttonLever.transform.DORotate(new Vector3(0, activatedButtonYAdjust, 0), 1.5f);
        }
        else
            buttonLever.transform.DORotate(new Vector3(0, 0, 0), 1.5f);

        isMoving = false;
    }
}
