using DG.Tweening;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private float scaleAmount;
    [SerializeField] private ParticleSystem fogVFX;
    [SerializeField] private string levelToLoad;
    [SerializeField] private LoadScene LoadScene;   
    private float normalScale;
    private bool isEneabled = false;

    private void Start()
    {
        normalScale = transform.localScale.x;   
    }


    public void EneableIsland()
    {
        isEneabled = true;
        GetComponent<Collider>().isTrigger = true;

        var main = fogVFX.main;
        main.loop = false;
        main.startLifetime = 1f;
    }

    public void InstaEneableIsland()
    {
        isEneabled = true;
        GetComponent<Collider>().isTrigger = true;
        Destroy(fogVFX);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ScaleIsland(scaleAmount, other,1f);
    }
    private void OnTriggerExit(Collider other)
    {
        ScaleIsland(normalScale, other,0.2f);
    }

    private void ScaleIsland(float scaleAmount, Collider other, float time)
    {
        if (!isEneabled) return;

        ShipController shipController = other.gameObject.GetComponent<ShipController>();
        if (shipController != null)
        {
            transform.DOScale(scaleAmount, time);
        }
    }
    
    public void LoadLevel()
    {
        if (!isEneabled) return;
        LoadScene.StartLoad(levelToLoad);
    }
}
