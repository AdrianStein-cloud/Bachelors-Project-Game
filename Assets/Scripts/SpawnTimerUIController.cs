using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnTimerUIController : MonoBehaviour
{
    bool started = false;
    float duration;
    float startTime;

    [SerializeField] Image timer;


    private void Awake()
    {
        UnitySingleton<SpawnTimerUIController>.BecomeSingleton(this);
        SetActive(false);
    }

    private void Update()
    {
        if (started)
        {
            float fill = (Time.time - startTime) / duration;
            timer.fillAmount = fill;
        }
    }

    public void StartTimer(float duration)
    {
        this.duration = duration;
        timer.fillAmount = 0;
        startTime = Time.time;
        started = true;
        SetActive(true);
    }

    void SetActive(bool enable)
    {
        GetComponentsInChildren<Image>(true)
            .ToList()
            .ForEach(x => x.gameObject.SetActive(enable));
    }
}
