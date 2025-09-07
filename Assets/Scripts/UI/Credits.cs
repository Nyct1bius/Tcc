using UnityEngine;

public class Credits : MonoBehaviour
{

    public GameObject creditos;
    public GameObject titleScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            creditos.SetActive(false);
            titleScreen.SetActive(true);
        }
    }
}
