using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float _maxTime = 120f;
    public float _currentTime;

    public TextMeshProUGUI _timerText;
    public Slider _timerSlider;

    private void Start()
    {
        _currentTime = _maxTime;
        _timerSlider.maxValue = _maxTime;
        _timerSlider.value = _maxTime;
        UpdateTimerUI();
    }

    private void Update()
    {
        if(_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            if(_currentTime < 0)
            {
                _currentTime = 0;
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60f);
        int seconds = Mathf.FloorToInt(_currentTime % 60f);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        _timerSlider.value = _currentTime;
    }
}
