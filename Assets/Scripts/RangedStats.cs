using UnityEngine;
using UnityEngine.UI;


public class RangedStats : MonoBehaviour
{
    public PlayerController player;
    public Text rangedText;


    // Update is called once per frame
    void Update()
    {
        float damage = player.GetChosenProjectile().GetDamage();
        rangedText.text = "Ranged: " + (damage);
    }
}