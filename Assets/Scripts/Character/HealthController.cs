using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using myStateMachine;
using Shooting_range;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Header("Health main parameters")]
    public float playerHealth = 100.0f;
    [SerializeField]
    private float maxHealth = 100f;


    [Header("Health Regen")]
    [Range(0, 50)] [SerializeField] private float HealthRegen = 0.5f;
    [SerializeField] private float RecoveryTime = 5;
    [Header("Stamina UI")]
    [SerializeField] private Slider healthUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;
    private Character character;
    private float timeUntillRecovery;
    private bool damaged = false;
    void Start()
    {
        character = GetComponent<Character>();
        timeUntillRecovery = RecoveryTime;
    }


    void Update()
    {
        if (damaged == true && timeUntillRecovery > 0.01)
        {
            timeUntillRecovery -= Time.deltaTime;
        }
        else 
        { 
            damaged = false; 
            timeUntillRecovery = RecoveryTime;
        }
            if (!damaged && playerHealth <= maxHealth - 0.001f)
            {
                playerHealth += HealthRegen * Time.deltaTime;
                UpdateHealth(1);
            }
            if(playerHealth >= maxHealth - 0.001f)
            {
                UpdateHealth(0);
            }       
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.TryGetComponent<Bullet>(out var bullet)) return;
        {
            GetDamage(bullet.Damage);
            //Debug.Log("hit");
        }
    }
    
    void GetDamage(float damage)
    {
        damaged = true;
        playerHealth -= damage;
        timeUntillRecovery = RecoveryTime;
        UpdateHealth(1);
        if (playerHealth <= 0)
        {
            Die();
            playerHealth = -10f;
        }
    }

    void Die()
    {
        Debug.Log("Death");

        SceneManager.LoadScene(0);
    }

    void UpdateHealth(int value)
    {
        healthUI.value = playerHealth;
        if (value == 0)
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
