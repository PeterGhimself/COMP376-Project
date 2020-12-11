using UnityEngine;
using UnityEngine.UI;


public class HealthStats : MonoBehaviour
{
    public PlayerController player;
    public Text healthText;


    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.GetCurrentHealth() + "/" + player.GetMaxHealth();
    }
}
