using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteppingTrigger : MonoBehaviour
{
    [SerializeField] private Material pressedMat;
    [SerializeField] private Material normalMat;
    [SerializeField] private Button[] ButtonsToActivate;
    private MeshRenderer triggerMeshRenderer;
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] private float activationTimer;
    public GameObject UIHolder;
    public TextMeshProUGUI timerText;
    public Slider timerSlider;
    private Coroutine countdownCoroutine;

    private int buttonPressedCount;
    public int buttonsCount;

    public bool activatesEnemies = false;
    [SerializeField] GameObject roomsToActivate;

    private void Awake()
    {
        triggerMeshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        if(PlayerUIManager.Instance)
        {
            UIHolder = PlayerUIManager.Instance._uiHolder;
            timerSlider = PlayerUIManager.Instance._timeSlider;
            timerText = PlayerUIManager.Instance._timeText;
        }
        StartCoroutine(WaitToFindPlayer());
        for (int i = 0; i < ButtonsToActivate.Length; i++)
        {
            ButtonsToActivate[i].steppingTrigger = this;
        }
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStateMachine player = other.GetComponent<PlayerStateMachine>();

        if(player != null )
        {
            ActivateTrigger();
        }
    }

    private void ActivateTrigger()
    {
        triggerMeshRenderer.material = pressedMat;
        if(countdownCoroutine != null ) StopCoroutine(CountdownTimerRoutine(activationTimer));
        for( int i = 0; i < ButtonsToActivate.Length; i++ )
        {
            ButtonsToActivate[i].ActivatedButton();   
        }
        countdownCoroutine = StartCoroutine(CountdownTimerRoutine(activationTimer));
        if(activatesEnemies && !roomsToActivate.activeSelf)
            roomsToActivate.SetActive(true);

        
    }
    private void DeactivateTrigger()
    {
        triggerMeshRenderer.material = normalMat;
        for (int i = 0; i < ButtonsToActivate.Length; i++)
        {
            ButtonsToActivate[i].DeactivatedButton();
        }
    }

    private IEnumerator CountdownTimerRoutine(float time)
    {
        float remainTime = time;
        if (timerSlider) timerSlider.maxValue = time;
        if (UIHolder) UIHolder.SetActive(true);

        while(remainTime > 0)
        {
            if(timerText) timerText.text = Mathf.CeilToInt(remainTime).ToString();
            if(timerSlider) timerSlider.value = remainTime;
            print(remainTime);
            yield return null;
            remainTime -= Time.deltaTime;
        }

        DeactivateTrigger();
        if (timerText) timerText.text = "0";
        if (timerSlider) timerSlider.value = 0;
        if (UIHolder) StartCoroutine(UIHolderDeactivation());

        Debug.Log("Countdown complete!");
        countdownCoroutine = null;
    }

    public void ButtonPressedDeactivation()
    {
        buttonPressedCount++;
        if (buttonsCount == buttonPressedCount) 
        {
            StopCoroutine(countdownCoroutine);
            StartCoroutine(UIHolderDeactivation());
            boxCollider.enabled = false;
        }
    }

    private IEnumerator UIHolderDeactivation()
    {
        yield return new WaitForSeconds(2f);
        UIHolder.SetActive(false);
    }

    IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if (PlayerUIManager.Instance != null)
        {
            while (UIHolder == null)
            {
                UIHolder = PlayerUIManager.Instance._uiHolder;
                timerSlider = PlayerUIManager.Instance._timeSlider;
                timerText = PlayerUIManager.Instance._timeText;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("GameManager Instance not found");
        }
    }
}
