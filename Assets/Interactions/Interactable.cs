using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void EnableInteractability();
    public abstract void DisableInteractability();

    private void OnDisable()
    {
        DisableInteractability();
    }
}
