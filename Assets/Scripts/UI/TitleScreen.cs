using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject _fase1, _fase2;
    private LoadScene loadscene;
    private bool fase2 = false;

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
        if(!fase2)
        {
            if(Input.anyKeyDown)
            {
                _fase1.SetActive(false);
                _fase2.SetActive(true);
            }
        }
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
