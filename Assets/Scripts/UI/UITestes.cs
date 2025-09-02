using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITestes : MonoBehaviour
{
    public GameObject telaInicio;
    public GameObject telaOver;
    public GameObject telaMenu;
    public GameObject telaMapa;

    public int money; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            telaInicio.SetActive(true);
        }

        if (Input.GetKeyDown("s"))
        {
            telaOver.SetActive(true);
        }

        if (Input.GetKeyDown("d"))
        {
            telaMenu.SetActive(true);
        }

        if (Input.GetKeyDown("f"))
        {
            telaInicio.SetActive(false);
            telaOver.SetActive(false);
            telaMenu.SetActive(false);
            telaMapa.SetActive(true);
        }
    }

    public void CarregarScene(string NewScene)
    {
        SceneManager.LoadScene("LoadScene", LoadSceneMode.Additive);
        LoadScene.FindAnyObjectByType<LoadScene>();

    }

    public void Aumenta()
    {
        money += 1;
        Debug.Log("money: " + money);
    }

   /* public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame(); // não pode salvar sem inicializar o game data primeiro
    }

    public void LoadGame()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void NewGame()
    {
        DataPersistenceManager.instance.NewGame();
    }*/

  
}
