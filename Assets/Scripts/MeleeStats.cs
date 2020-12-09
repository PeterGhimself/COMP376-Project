using UnityEngine;
using UnityEngine.UI;


public class MeleeStats : MonoBehaviour
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
        float damage = player.GetChosenWeapon().getDamage();
        attackText.text = "Melee: " + (damage);
        //print("Attack: " + (damage));
    }
}
