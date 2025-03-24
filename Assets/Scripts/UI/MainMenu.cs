using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool paused = false;

    public GameObject _testes;

    [SerializeField]
    private GameObject _mainMenuScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p") && !paused)
        {
            _mainMenuScreen.SetActive(true);
            paused = true;
            //Debug.Log("pausado");
        }
        else if(Input.GetKeyDown("p") && paused)
        {
            _mainMenuScreen.SetActive(false);
            paused = false;
            //Debug.Log("Não pausado");
        }

        if (Input.GetKeyDown("l"))
        {
            _testes.SetActive(true);
        }


    }

    public void Title()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }
}
