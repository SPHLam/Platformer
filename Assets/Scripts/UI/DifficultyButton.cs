using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _difficultyInfo;
    [TextArea]
    [SerializeField] private string _description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _difficultyInfo.text = _description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _difficultyInfo.text = "";
    }
}
