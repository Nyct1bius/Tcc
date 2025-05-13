using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private Slider barraProgresso;

    public void StartLoad(string levelToLoad)
    {
        StartCoroutine(Load(levelToLoad));
    }

    private IEnumerator Load(string levelToLoad)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while(!asyncOperation.isDone)
        {
            this.barraProgresso.value = asyncOperation.progress;
            yield return null;
        }
    }
}
