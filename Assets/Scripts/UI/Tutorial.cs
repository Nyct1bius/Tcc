using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public bool _attackTutorial = false;
    public bool _jumpTutorial = false;
    public bool _dashTutorial = false;

    [SerializeField] GameObject _attackScreen;

    [SerializeField] GameObject _jumpScreen;

    [SerializeField] GameObject _dashScreen;

    public bool _finish = false;
    public bool _isShowing = false;

    private void Update()
    {
        if (_isShowing && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTutorial();
            _finish = true;
            _isShowing = false;
        }
    }

    private void ShowTutorial()
    {
        if(_attackTutorial)
        {
            _attackScreen.SetActive(true);
        }
        else if(_jumpTutorial)
        {
            _jumpScreen.SetActive(true);
        }
        else if(_dashTutorial)
        {
            _dashScreen.SetActive(true);
        }

        _isShowing = true;
    }

    private void CloseTutorial()
    {
        if (_attackTutorial)
        {
            _attackScreen.SetActive(false);

        }
        else if (_jumpTutorial)
        {
            _jumpScreen.SetActive(false);
            
        }
        else if (_dashTutorial)
        {
            _dashScreen.SetActive(false);
            
        }

        PauseGameManager.ResumeGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_finish)
        {
            ShowTutorial();
            PauseGameManager.PauseGame();
        }
    }

  /*  private void OnTriggerExit(Collider other) // temp ate add pause e botoes
    {
        CloseTutorial();
        _finish = true;
    }*/
}
