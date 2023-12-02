using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.EventSystems;
using TMPro;
public class ColorButtonManager : MonoBehaviour,  ISelectHandler, IDeselectHandler
{
    [SerializeField]
    private ColorPalette.ColorLM defaultForeground;
    [SerializeField]
    private ColorPalette.ColorLM hoverForeground;

    private TMP_Text btnText;

    private void Start(){
        btnText = GetComponentInChildren<TMP_Text>();
    }

    void SwapActiveColor(){
        btnText.color = ColorPalette.getColor(hoverForeground);
    }

    void SwapDefaultColor(){
        btnText.color = ColorPalette.getColor(defaultForeground); 
    }
    /*
    public void OnPointerEnter(PointerEventData data){
        SwapActiveColor();
    }

    public void OnPointerExit(PointerEventData data){
        SwapDefaultColor();
    }
    */

    public void OnSelect(BaseEventData eventData){
        SwapActiveColor();
    }

    public void OnDeselect (BaseEventData data) {
        SwapDefaultColor();
	}
}
