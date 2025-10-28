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

    private void OnTriggerEnter(Collider other)
    {
        ScaleIsland(scaleAmount, other);
    }
    private void OnTriggerExit(Collider other)
    {
        ScaleIsland(normalScale, other);
    }

    private void ScaleIsland(float scaleAmount, Collider other)
    {
        if (!isEneabled) return;

        ShipController shipController = other.gameObject.GetComponent<ShipController>();
        if (shipController != null)
        {
            transform.DOScale(scaleAmount, 1f);
        }
    }
    
    public void LoadLevel()
    {
        if (!isEneabled) return;
        LoadScene.StartLoad(levelToLoad);
    }
}
