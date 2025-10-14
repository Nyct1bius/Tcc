using UnityEngine;

public class OceanManager : MonoBehaviour
{
    public static OceanManager Instance;
    [SerializeField] private float wavesHeight = 0.07f;
    [SerializeField] private float wavesFrequency = 0.1f;
    [SerializeField] private float wavesSpeed = 0.04f;


    [SerializeField] private Transform oceanTransform;
     private Material oceanMaterial;
    Texture2D wavesDisplacement;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);    
    }

    private void Start()
    {
        SetupVarables();
    }
    private void SetupVarables()
    {
        oceanMaterial = oceanTransform.GetComponent<Renderer>().sharedMaterial;

        wavesDisplacement = (Texture2D)oceanMaterial.GetTexture("_WavesDisplacement");
    }

    public float GetWavesHeight(Vector3 targetPosition)
    {
        return oceanTransform.position.y * wavesDisplacement.GetPixelBilinear(targetPosition.x * wavesFrequency, targetPosition.z  *wavesFrequency + Time.time * wavesSpeed).g * wavesHeight * oceanTransform.localScale.x;
    }

    private void OnValidate()
    {
        if(!oceanMaterial)
            SetupVarables();

        UpdateMaterial();

    }

    private void UpdateMaterial()
    {

        oceanMaterial.SetFloat("_WavesHeight", wavesHeight);
        oceanMaterial.SetFloat("_WavesFrequency", wavesFrequency);
        oceanMaterial.SetFloat("_WavesSpeed", wavesSpeed);
    }
}
