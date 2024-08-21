using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : Interactable
{
    public AudioSource audioSource;
    public AudioClip beep;
    private bool alreadyPressed;
    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        if (alreadyPressed)
        {
            return;
        }

        audioSource.PlayOneShot(beep);
        alreadyPressed = true;
    }
}
