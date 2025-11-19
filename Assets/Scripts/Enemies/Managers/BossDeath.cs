using UnityEngine;

public class BossDeath : MonoBehaviour
{
    [SerializeField] private string creditsScene;
    [SerializeField] private LoadScene loadScene;

    public void LoadCreditsScene()
    {
        loadScene.StartLoad(creditsScene);
    }
}
