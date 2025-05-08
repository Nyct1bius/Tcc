using UnityEngine;

public enum TutorialType
{
    Attack,
    Jump,
    Dash,
    Puzzle,
    Navigation
}

public class Tutorial : MonoBehaviour
{
    public TutorialType tutorialType;
    [SerializeField] private GameObject[] tutorialParts;

    private int currentPartIndex = 0;
    private bool isFinished = false;
    private bool isShowing = false;

    private void Update()
    {
        if(isShowing && Input.GetKeyDown(KeyCode.Space))
        {
            ShowNextPart();
        }

        if (isShowing && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTutorial();
        }
    }

    private void ShowTutorial()
    {
        if (tutorialParts.Length == 0 || isFinished) return;

        currentPartIndex = 0;
        tutorialParts[currentPartIndex].SetActive(true);
        isShowing = true;

        PauseGameManager.PauseGame();
    }

    private void ShowNextPart()
    {
        // Desativa a parte atual
        tutorialParts[currentPartIndex].SetActive(false);

        currentPartIndex++;

        if(currentPartIndex < tutorialParts.Length)
        {
            // Ativa a próxima parte
            tutorialParts[currentPartIndex].SetActive(true);
        }
        else
        {
            // Se não houver mais partes, fecha o tutorial
            CloseTutorial();
        }
    }

    private void CloseTutorial()
    {
        if(currentPartIndex < tutorialParts.Length)
        {
            tutorialParts[currentPartIndex].SetActive(false);
        }

        isFinished = true;
        isShowing = false;
        PauseGameManager.ResumeGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isFinished)
        {
            ShowTutorial();
        }
    }
}
