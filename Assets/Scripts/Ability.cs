using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
	[SerializeField] private PlayerController.EAbility m_abilityType = default;
	[SerializeField] private LayerMask m_playerLayer = default;
	[SerializeField] private SpriteRenderer m_spriteRenderer = default;
	[SerializeField] private Canvas m_tooltip = default;
	[SerializeField] private TMPro.TMP_Text m_text = default;

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
				ChangeColor();
			}
			else
			{
				Debug.LogError("No playercontroller script on " + collider.gameObject.name);
			}

			//spawn effect here
		}
	}

	Ray ray;
	RaycastHit2D[] hits;

	private void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
		if (hits.Length > 0)
		{
			foreach(RaycastHit2D hit in hits)
			{
				if(hit.collider.gameObject == gameObject)
				{
					ShowTooltip();
					return;
				}
			}
		}

		HideTooltip();
	}

	private void ShowTooltip()
	{
		m_tooltip.gameObject.SetActive(true);
	}

	private void HideTooltip()
	{
		m_tooltip.gameObject.SetActive(false);
	}

	private void ChangeColor()
	{
		switch(m_abilityType)
		{
			case PlayerController.EAbility.Dash:
				m_spriteRenderer.color = Color.blue;
				m_text.text = "Dash";
				break;
			case PlayerController.EAbility.ProjectileExplosion:
				m_spriteRenderer.color = Color.red;
				m_text.text = "Projectile Explosion";
				break;
			case PlayerController.EAbility.Slowmo:
				m_spriteRenderer.color = Color.yellow;
				m_text.text = "Slowmo";
				break;
		}
	}
}
