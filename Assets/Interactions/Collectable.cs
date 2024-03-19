using System;
using UnityEngine;

public class Collectable : Interactable
{
    [SerializeField] Vector2 pitchRange;

    public Action onCollect;

    AudioSource source;
    bool inFocus;

    private void Awake()
    {
        source = transform.parent.GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (inFocus && InputManager.Player.Interact.triggered)
        {
            onCollect();
            source.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
            source.Play();
            Destroy(gameObject.transform.parent.gameObject, source.clip.length);
            gameObject.SetActive(false);
        }
    }

    public override void EnableInteractability()
    {
        inFocus = true;
        InteractionUIText.Instance.SetText("Press E to pickup heart");

    }

    public override void DisableInteractability()
    {
        inFocus = false;
        InteractionUIText.Instance.SetText("");
    }

}
