using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    private Slider health_Stats, stamina_Stats;
    public Text BulletsCountTxt;
    public Text reloadCountTxt;
    public float health = 0;
    public bool is_Dead;
    public bool is_Player;
    private void Update()
    {
      //  Display_HealthStats(health);
    }
    public void Display_HealthStats(float healthValue) {

        healthValue /= 100f;

        health_Stats.value = healthValue;

    }

    public void Display_StaminaStats(float staminaValue) {

        staminaValue /= 100f;

        stamina_Stats.value  = staminaValue;

    }

    public void BulletsAvailable(int Bullet)
    {
        BulletsCountTxt.text = Bullet.ToString();
    }

    public void ReloadAvailable(int Reloadcnt)
    {
        reloadCountTxt.text = Reloadcnt.ToString();
    }

    public void ApplyDamage(float damage)
    {

        // if we died don't execute the rest of the code
        if (is_Dead)
            return;

        health -= damage;

       // Debug.LogError(health);

        //if (is_Player)
        {
            // show the stats(display the health UI value)
            Display_HealthStats(health);
        }

    
        if (health <= 0f)
        {
            is_Dead = true;
            Time.timeScale = 0f;
            UIManager.instance.gamestarted = false;
            CamShake.Camshake = false;
            UIManager.instance.EnterUIState(UIManager.UIState.death);
        }

    } // apply damage


} // class





























