using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabTube : Interactable
{
    private bool inFocus;
    private bool pressed;
    private bool canBeClaimed;
    private AudioSource buttonSound;
    private AudioSource humSound;
    [SerializeField] private GameObject tubeContent;
    private int worth;
    private TextMeshProUGUI countdownText;
    private float countdownTimer = 80f;

    private void Update()
    {
        if (InputManager.Player.Interact.triggered && inFocus && !pressed)
        {
            PressButton();
        }
        if (InputManager.Player.Interact.triggered && inFocus && pressed && canBeClaimed)
        {
            Claim();
        }
        if(pressed && countdownTimer >= 0)
        {
            countdownTimer -= Time.deltaTime;
            if(countdownTimer <= 0)
            {
                countdownText.text = $"00:00";
            }
            else
            {
                countdownText.text = $"{Mathf.Floor(countdownTimer / 60).ToString("00")}:{((int)(countdownTimer % 60)).ToString("00")}";
            }
        }
    }

    private void Start()
    {
        worth = UnityEngine.Random.Range(60, 100);
        buttonSound = GetComponent<AudioSource>();
        humSound = tubeContent.GetComponent<AudioSource>();
        countdownText = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        countdownText.text = $"{Mathf.Floor(countdownTimer / 60).ToString("00")}:{((int)(countdownTimer % 60)).ToString("00")}";
    }

    private void Claim()
    {
        canBeClaimed = false;
        InteractionUIText.Instance.SetText("");
        buttonSound.Play();
        transform.position -= new Vector3(0, 0.09f, 0);
        UnitySingleton<CurrencyManager>.Instance.AddCurrency(worth);
        Destroy(tubeContent);
        StartCoroutine(ButtonWait());
    }

    private void PressButton()
    {
        InteractionUIText.Instance.SetText("");
        buttonSound.Play();
        humSound.Play();
        transform.position -= new Vector3(0, 0.09f, 0);
        pressed = true;
        StartCoroutine(ButtonWait());
        StartCoroutine(LerpTube());
    }

    IEnumerator ButtonWait()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position += new Vector3(0, 0.09f, 0);
    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        if (!pressed)
        {
            InteractionUIText.Instance.SetText("Start Blood Extraction");
        }
        if (canBeClaimed)
        {
            InteractionUIText.Instance.SetText($"Claim Blood ({worth}$)");
        }
    }

    IEnumerator LerpTube()
    {
        float elapsedTime = 0;
        var startSizeY = tubeContent.transform.localScale.y;
        float smoothTime = countdownTimer;
        while (elapsedTime < smoothTime)
        {
            tubeContent.transform.localScale = new Vector3(tubeContent.transform.localScale.x, Mathf.Lerp(startSizeY, 3.13f, elapsedTime / smoothTime), tubeContent.transform.localScale.z);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canBeClaimed = true;
    }
}
