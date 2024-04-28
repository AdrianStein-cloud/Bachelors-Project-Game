using Unity.Netcode;

public abstract class Interactable : NetworkBehaviour
{
    public abstract void EnableInteractability();
    public abstract void DisableInteractability();

    private void OnDisable()
    {
        DisableInteractability();
    }
}
