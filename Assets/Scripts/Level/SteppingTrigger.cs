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

    [SerializeField] private float activationTimer;
    public GameObject UIHolder;
    public TextMeshProUGUI timerText;
    public Slider timerSlider;
    private Coroutine countdownCoroutine;

    private void Awake()
    {
        triggerMeshRenderer = GetComponent<MeshRenderer>();
        normalMat = GetComponent<Material>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

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
        UIHolder.SetActive(true);

        while(remainTime > 0)
        {
            if(timerText) timerText.text = Mathf.CeilToInt(remainTime).ToString();
            if(timerSlider) timerSlider.value = remainTime;

            yield return null;
            remainTime -= Time.deltaTime;
        }

        DeactivateTrigger();
        if (timerText) timerText.text = "0";
        if (timerSlider) timerSlider.value = 0;
        StartCoroutine(UIHolderDeactivation());

        Debug.Log("Countdown complete!");
        countdownCoroutine = null;
    }

    private IEnumerator UIHolderDeactivation()
    {
        yield return new WaitForSeconds(2f);
        UIHolder.SetActive(false);
    }
}
