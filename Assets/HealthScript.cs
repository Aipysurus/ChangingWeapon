using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public Slider slider;
    public int MaxHealth = 100;
    public int CurrentHealth;
    

    // Start is called before the first frame update
    void Awake()
    {
        CurrentHealth = MaxHealth;
        SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            slider.value -= 10;
        }
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

}
