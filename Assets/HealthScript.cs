using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public Slider HealthSlider;
    public Slider RegenSlider;
    public Text PotionsCounterDisplay; 

    public int MaxHealth = 100;
    public int RegenMaxHealth;
    public int CurrentHealth;

    public float RegenTimerMax = 1f;
    private float RegenTimer;

    public int PotionAmmount = 3;
    public int PotionHealing = 25;

    

    // Start is called before the first frame update
    void Awake()
    {
        PotionsCounterDisplay.text = "Potions: " + PotionAmmount;
        CurrentHealth = MaxHealth;
        RegenMaxHealth = MaxHealth;
        RegenTimer = RegenTimerMax;
        SetMaxHealth(MaxHealth);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(25);
        }

        if (RegenTimer <= 0 && CurrentHealth < RegenMaxHealth )
        {
            Regenerate();
            RegenTimer = RegenTimerMax;
        }
        else
        {
            RegenTimer -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AddHealth(PotionHealing);
        }
    }

    public void Regenerate()
    {
        HealthSlider.value += 1;
        CurrentHealth += 1;
    }


    public void SetHealth(int health)
    {
        HealthSlider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        HealthSlider.maxValue = health;
        HealthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        
        int regenDamage = (int)(damage / 2);

        RegenSlider.value = HealthSlider.value;

        HealthSlider.value -= damage;
        RegenSlider.value -= regenDamage;
        CurrentHealth = (int) HealthSlider.value;
        RegenMaxHealth = (int) RegenSlider.value;
    }

    public void AddHealth(int health)
    {

        if (PotionAmmount > 0)
        {
            CurrentHealth += health;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            RegenMaxHealth = CurrentHealth;
            HealthSlider.value = CurrentHealth;
            RegenSlider.value = RegenMaxHealth;
            PotionAmmount -= 1;
            PotionsCounterDisplay.text = "Potions: " + PotionAmmount;
        }
    }

}
