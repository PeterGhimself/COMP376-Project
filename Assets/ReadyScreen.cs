using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyScreen : MonoBehaviour
{
    [SerializeField] private Animator[] m_weaponSelectionAnimators = default;
    [SerializeField] private Animator[] m_projectileSelectionAnimators = default;

    public void SetWeaponSelection(int choice)
    {
        for(int i = 0; i < m_weaponSelectionAnimators.Length; i++)
        {
            m_weaponSelectionAnimators[i].SetBool("Active", i == choice);
            if (i == choice) m_weaponSelectionAnimators[i].SetTrigger("Selected");
        }
    }

    public void SetProjectileSelection(int choice)
    {
        for (int i = 0; i < m_projectileSelectionAnimators.Length; i++)
        {
            m_projectileSelectionAnimators[i].SetBool("Active", i == choice);
            if(i == choice) m_projectileSelectionAnimators[i].SetTrigger("Selected");
        }
    }

    private void Start()
    {
        SetWeaponSelection(0);
        SetProjectileSelection(0);
    }
}
