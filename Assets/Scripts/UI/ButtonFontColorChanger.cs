using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonFontColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Color defaultColor = new Color(255, 255, 255, 255);
	[SerializeField] private Color highlightedColor = new Color(255, 0, 0, 255);

	public void OnPointerEnter(PointerEventData eventData)
	{
		gameObject.GetComponentInChildren<TextMeshProUGUI>().color = highlightedColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		gameObject.GetComponentInChildren<TextMeshProUGUI>().color = defaultColor;
	}
}
