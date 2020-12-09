using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
	[SerializeField] private PlayerController.EAbility m_abilityType = default;
	[SerializeField] private LayerMask m_playerLayer = default;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (m_playerLayer == (m_playerLayer | (1 << collider.gameObject.layer)))
		{
			PlayerController playerCtrl = collider.gameObject.GetComponent<PlayerController>();
			if (playerCtrl)
			{
				PlayerController.EAbility oldAbility = playerCtrl.GetPlayerAbility();
				playerCtrl.ChangePlayerAbility(m_abilityType);
				m_abilityType = oldAbility;
			}
			else
			{
				Debug.LogError("No playercontroller script on " + collider.gameObject.name);
			}

			//spawn effect here
		}
	}
}
