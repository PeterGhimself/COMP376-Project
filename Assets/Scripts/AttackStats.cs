using UnityEngine;
using UnityEngine.UI;


public class AttackStats : MonoBehaviour
{
    public PlayerController player;
    public Text attackText;


    // Update is called once per frame
    void Update()
    {
        /*
        Dagger = 0,
        Sword = 1,
        Hammer = 2
        */
        int weapon = player.GetChosenWeapon();
        int damage = 0;

        switch (weapon) {
            case 0:
                damage = 1;
                break;
            case 1:
                damage = 2;
                break;
            case 2:
                damage = 5;
                break;
        }
        attackText.text = "Attack: " + (damage + player.getMeleeDamageModifier());
        print("Attack: " + (damage + player.getMeleeDamageModifier()));
    }
}
