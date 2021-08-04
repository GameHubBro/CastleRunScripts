using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private Animator goodByeTextAnimator;
    [SerializeField] private Animator startNewGameTextAnimator;

    public bool isStartingNewGame = false;
    public bool isQuiting = false;

    public static TextChanger instance;

    private void Awake()
    {
        if (TextChanger.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        TextChanger.instance = this;
    }

    private void OnMouseDown()
    {
        if (isQuiting)
        {
            goodByeTextAnimator.SetTrigger("Clicked");
        }
        else if (isStartingNewGame)
        {

        }
    }
}
