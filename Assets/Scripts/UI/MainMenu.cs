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

        if(Input.GetKeyDown("1"))
        {
            // pega item
            Debug.Log("Pegou roda");
            Roda();
        }

        if (Input.GetKeyDown("2"))
        {
            // pega item
            Debug.Log("Pegou casco");
            Casco();
        }

        if (Input.GetKeyDown("3"))
        {
            // pega item
            Debug.Log("Pegou arma");
            Arma();
        }


    }

    public void Title()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }

    // testes
    public InventoryManager inventoryManager;
    public void Roda()
    {
        inventoryManager.GetIten("Roda");
    }

    public void Casco()
    {
        inventoryManager.GetIten("Casco");
    }

    public void Arma()
    {
        inventoryManager.GetIten("Arma");
    }
}
