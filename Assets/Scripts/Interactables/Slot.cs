using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : Interactable
{
    [SerializeField]
    private GameObject lever;
    private GameObject player;

    // amount of balance to gain or lose
    public int maxReward = 100;
    public int maxLoss = 50;

    // reference to PlayerStats or another player stats script
    private PlayerStats PlayerStats;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        lever.GetComponent<Animator>().SetTrigger("PullLever");
        // simulate slot machine result
        int result = Random.Range(-maxLoss, maxReward + 1);

        // update player's balance based on result
        PlayerStats.UpdateBalance(result);

        // optional: play animation or sound for the slot machine
        Debug.Log("Slot machine result: " + result + " balance updated to: " + PlayerStats.GetBalance());
    }
}
