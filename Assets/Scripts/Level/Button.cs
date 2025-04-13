using UnityEngine;

public class Button : MonoBehaviour,IHealth
{
    [SerializeField] private bool isActivated;
    [SerializeField] Door doorToOpen;
    [SerializeField] private Material newMat;
    private MeshRenderer meshRenderer;

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

    public void Damage(float damage)
    {
        if (isActivated)
        {
            Debug.Log("OpenDoor");
            doorToOpen.CheckIfAllButtonsIsActivated();
            meshRenderer.material = newMat;
        }
    }

  
    public void ActivatedButton()
    {
        isActivated = true;
    }


}
