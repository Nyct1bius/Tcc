using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject _fase1, _fase2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fase1.SetActive(true);
        _fase2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        SceneManager.LoadSceneAsync("Programmers_TestScene");
    }

    public void NewGame()
    {
        SceneManager.LoadSceneAsync("Programmers_TestScene");
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync("Programmers_TestScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
