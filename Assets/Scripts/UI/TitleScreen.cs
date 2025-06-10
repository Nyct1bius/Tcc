using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject _fase1, _fase2;
    private LoadScene loadscene;

    private void Awake()
    {
        loadscene = GetComponent<LoadScene>();
    }

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
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void NewGame()
    {
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void LoadGame()
    {
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
