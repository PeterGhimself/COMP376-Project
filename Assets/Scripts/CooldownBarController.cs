using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarController : MonoBehaviour
{

    public PlayerController player;
    public Image imageMeleeCooldown, imageRangedCooldown, imageAbilityCooldown;
    private float meleeCooldown, rangedCooldown, abilityCooldown;
    private bool isMeleeCooldown, isRangedCooldown, isAbilityCooldown;

    void Start()
    {
        this.meleeCooldown = this.player.GetAttackCooldownTime();
        this.rangedCooldown = this.player.GetProjectileCooldownTime();
        this.abilityCooldown = this.player.GetAbilityCooldownTime();
        print("meleeCooldown: " + meleeCooldown);
        print("rangedCooldown: " + rangedCooldown);
        print("abilityCooldown: " + abilityCooldown);
    }

    void Update()
    {
        this.meleeCooldown = this.player.GetAttackCooldownTime();
        this.rangedCooldown = this.player.GetProjectileCooldownTime();
        this.abilityCooldown = this.player.GetAbilityCooldownTime();
        // "Fire" for melee
        // "FireRanged" for ranged
        // "Ability" for ability

        // melee
        if (Input.GetButton("Fire"))
        {
            isMeleeCooldown = true;
            print("FIRE");
        }
        if (isMeleeCooldown)
        {
            imageMeleeCooldown.fillAmount += 1 / meleeCooldown * Time.deltaTime;

            if (imageMeleeCooldown.fillAmount >= 1)
            {
                imageMeleeCooldown.fillAmount = 0;
                isMeleeCooldown = false;
            }
        }

        // ranged
        if (Input.GetButton("FireRanged"))
        {
            isRangedCooldown = true;
            print("FIRE RANGED");
        }
        if (isRangedCooldown)
        {
            imageRangedCooldown.fillAmount += 1 / rangedCooldown * Time.deltaTime;

            if (imageRangedCooldown.fillAmount >= 1)
            {
                imageRangedCooldown.fillAmount = 0;
                isRangedCooldown = false;
            }
        }

        // ability
        if (Input.GetButton("Ability"))
        {
            print("ABILITY");
            isAbilityCooldown = true;
        }
        if (isAbilityCooldown)
        {
            imageAbilityCooldown.fillAmount += 1 / abilityCooldown * Time.deltaTime;

            if (imageAbilityCooldown.fillAmount >= 1)
            {
                imageAbilityCooldown.fillAmount = 0;
                isAbilityCooldown = false;
            }
        }
    }
}
