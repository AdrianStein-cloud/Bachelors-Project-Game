using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public float delay;

    private void Awake()
    {
        InputManager.ReloadInputs();
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Procedural Generation");
    }
}
