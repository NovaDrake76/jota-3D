using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : Interactable
{
    [SerializeField]
    private GameObject lever;
    private GameObject player;

    public int MaxHealthToRestore = 120;

    // reference to PlayerStats or another player stats script
    private PlayerStats PlayerStats;
    public AudioSource audioSource;
    public AudioClip healSound;
    public AudioClip noHealSound;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerStats = player.GetComponent<PlayerStats>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        if (MaxHealthToRestore <= 0)
        {
            audioSource.PlayOneShot(noHealSound);
            return;
        }

        lever.GetComponent<Animator>().SetTrigger("PullLever");
        PlayerStats.RestoreHealth(20);
        audioSource.PlayOneShot(healSound);

        MaxHealthToRestore -= 20;

    }
}
