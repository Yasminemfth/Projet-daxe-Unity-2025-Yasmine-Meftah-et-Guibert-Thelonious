using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    private int currentHealth;

//permet de gerer la santé du joueur : limité entre 0 et santé max
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public float invincibilityTimeAfterHit = 2f;
    public float invincibilityFlashDelay = 0.2f;
    public bool isInvincible = false;

    public SpriteRenderer graphics;
    public HealthBar healthBar;

    public static PlayerHealth instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerHealth dans la scène");
            return;
        }

        instance = this;
    }

    void Start()
    {
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(60);
        }
    }

    public void HealPlayer(int amount)
    {
        CurrentHealth += amount;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            CurrentHealth -= damage;

            if (CurrentHealth <= 0)
            {
                Die();
                return;
            }

            isInvincible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvincibilityDelay());
        }
    }

//je n'ai pas mis l'ecran game over pour l'instant ni le respawn
    public void Die()
    {
        PlayerController.instance.enabled = false;
        PlayerController.instance.rb.bodyType = RigidbodyType2D.Kinematic;
        PlayerController.instance.rb.linearVelocity = Vector2.zero;
        PlayerController.instance.playerCollider.enabled = false;

        Debug.Log("Le joueur est mort .");
    }

    public void Respawn()
    {
        PlayerController.instance.enabled = true;
        PlayerController.instance.rb.bodyType = RigidbodyType2D.Dynamic;
        PlayerController.instance.playerCollider.enabled = true;
        CurrentHealth = maxHealth;
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isInvincible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvincible = false;
    }
}
