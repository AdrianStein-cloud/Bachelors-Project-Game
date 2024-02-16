using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuDoorHover : MonoBehaviour
{
    private Animator anim;
    public bool hasClicked;
    private Animator cameraAnim;
    public Animator fadeAnim;
    private MainMenu menu;
    private void Start()
    {
        menu = FindObjectOfType<MainMenu>();
        cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
        anim = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if (!hasClicked)
        {
            hasClicked = true;
            cameraAnim.SetTrigger("Toggle");
            fadeAnim.SetTrigger("Toggle");
            StartCoroutine(StartGame());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        menu.StartGame();
    }

    private void OnMouseEnter()
    {
        if (!hasClicked) anim.SetTrigger("Toggle");
    }

    private void OnMouseExit()
    {
        if (!hasClicked) anim.SetTrigger("Toggle");
    }
}
