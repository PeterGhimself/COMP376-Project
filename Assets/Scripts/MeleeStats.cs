using UnityEngine;
using UnityEngine.UI;


public class MeleeStats : MonoBehaviour
{
    public PlayerController player;
    public Text attackText;


    // Update is called once per frame
    void Update()
    {
        float damage = player.GetChosenWeapon().GetDamage();
        attackText.text = "Melee: " + (damage);
        //print("Attack: " + (damage));
    }
}
