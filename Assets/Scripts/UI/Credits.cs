using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    public GameObject creditos;
    public GameObject titleScreen;

    public bool isScene = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if(isScene)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Title Screen");
            }
            else
            {
                creditos.SetActive(false);
                titleScreen.SetActive(true);
            }
        }
    }
}
