using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private float health;
    private int balance = 100;
    private float lerpTimer;
    [Header("Health Bar")]

    public float maxHealth = 100;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    [Header("Damage Overlay")]
    public Image Overlay;
    public float duration;
    public float fadeSpeed;
    public float durationTimer;
    public AudioSource audioSource;
    public GameObject gameOverCanvas;

    public AudioClip takeDamage;
    private CursorLockMode cursorLock;



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b, 0);
        gameOverCanvas.SetActive(false);
        cursorLock = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (Overlay.color.a > 0)
        {
            // if (health < 30)
            // {
            //     return;
            // }


            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = Overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b, tempAlpha);
            }
        }

    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;

            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;

            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;
        audioSource.PlayOneShot(takeDamage);
        Overlay.color = new Color(Overlay.color.r, Overlay.color.g, Overlay.color.b, 1);

        if (health <= 0)
        {
            Die();
        }

    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void UpdateBalance(int amount)
    {
        balance += amount;
        Debug.Log("Player's new balance: " + balance);
    }

    public int GetBalance()
    {
        return balance;
    }

    public void Die()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        //liberate mouse to allow player to click on buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
