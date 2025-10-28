using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private string mapaAtual;

    [SerializeField]
    private LoadScene loadScene;

    private void Start()
    {
        mapaAtual = SceneManager.GetActiveScene().name;
    }

    public void Continue()
    {
        loadScene.StartLoad(mapaAtual);
    }

    public void Quit()
    {
        SceneManager.LoadSceneAsync("Title Screen");
    }
}
