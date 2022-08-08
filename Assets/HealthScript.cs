using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public Slider HealthSlider;
    public Slider RegenSlider;

    public int MaxHealth = 100;
    public int RegenMaxHealth;
    public int CurrentHealth;

    public float RegenTimer = 1f;
    

    // Start is called before the first frame update
    void Awake()
    {
        CurrentHealth = MaxHealth;
        RegenMaxHealth = MaxHealth;
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
            HealthSlider.value += 1;
            CurrentHealth += 1;
            RegenTimer = 1f; // Make this higher for longer regen effect
        }
        else
        {
            RegenTimer -= Time.deltaTime;
        }
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

    public void TakeDamage(int Damage)
    {
        int RegenDamage = (int)(Damage / 2);

        RegenSlider.value = HealthSlider.value;

        HealthSlider.value -= Damage;
        RegenSlider.value -= RegenDamage;
        CurrentHealth = (int) HealthSlider.value;
        RegenMaxHealth = (int) RegenSlider.value;


    }

}
