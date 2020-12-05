using UnityEngine;
using UnityEngine.UI;


public class AttackStats : MonoBehaviour
{
    public PlayerController player;
    public PlayerWeapon weapon;
    public Text attackText;


    // Update is called once per frame
    void Update()
    {
        attackText.text = "Attack: " + weapon.getDamage() + player.getMeleeDamageModifier();
        print("Attack: " + (weapon.getDamage() + player.getMeleeDamageModifier()));
    }
}
