using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Slider barraProgresso;

    public GameObject loadScreen;

    public GameObject engrenagem;

    [SerializeField]
    private string mapaAtual;

    public void StartLoad(string levelToLoad)
    {
        StartCoroutine(Load(levelToLoad));
    }

    private IEnumerator Load(string levelToLoad)
    {

        loadScreen.SetActive(true);

        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        

        while (!asyncOperation.isDone)
        {
            engrenagem.transform.Rotate(0f, 0f, -90f * Time.deltaTime);

            float loadProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            barraProgresso.value = loadProgress;

            yield return null;
        }

        barraProgresso.value = 1f;

        yield return new WaitForSeconds(0.3f);

        
    }

    public void Continue()
    {
        mapaAtual = SceneManager.GetActiveScene().name;

        StartLoad(name);
    }
}
