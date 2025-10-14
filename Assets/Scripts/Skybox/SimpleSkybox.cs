using UnityEngine;

public class SimpleSkybox : MonoBehaviour
{
    [Header("Skybox Config")]
    public Material skyboxMaterial;       
    public Cubemap skyboxCubemap;         
    public float rotationSpeed = .1f;      
    public Vector3 rotationDirection = new Vector3(4.35f, 0.83f, -5.47f); 

    void Start()
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;

            if (skyboxCubemap != null)
                skyboxMaterial.SetTexture("_Cube", skyboxCubemap);
        }
        else
        {
            Debug.LogWarning("Skybox material não definido!");
        }
    }

    void Update()
    {
        if (skyboxMaterial == null) return;

        skyboxMaterial.SetFloat("_RotationSpeed", rotationSpeed);
        skyboxMaterial.SetFloat("_RotationX", rotationDirection.x);
        skyboxMaterial.SetFloat("_RotationY", rotationDirection.y);
        skyboxMaterial.SetFloat("_RotationZ", rotationDirection.z);
        if (skyboxCubemap != null)
            skyboxMaterial.SetTexture("_SkyboxTex", skyboxCubemap);
    }
    public void SetSkyboxCubemap(Cubemap newCubemap)
    {
        skyboxCubemap = newCubemap;
        if (skyboxMaterial != null)
            skyboxMaterial.SetTexture("_SkyboxTex", skyboxCubemap);
    }
}
