using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Slider barraProgresso;

    public GameObject loadScreen;

    public void StartLoad(string levelToLoad)
    {
        loadScreen.SetActive(true);
        StartCoroutine(Load(levelToLoad));
    }

    private IEnumerator Load(string levelToLoad)
    {
        TransitionLayer layer = TransitionManager.Instance.layer;
        layer.Show(0.5f, 0.0f);

        while(!layer.isDone)
        {
            yield return null;
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        asyncOperation.allowSceneActivation = false;


        while(asyncOperation.progress < 0.9f)
        {
            //this.barraProgresso.value = asyncOperation.progress;
            yield return null;
        }

        asyncOperation.allowSceneActivation = true;

        while(!layer.isDone)
        {
            yield return null;
        }
        layer.Hide(0.5f, 1.25f);

    }
}
