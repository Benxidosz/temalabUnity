using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TradeButtonClicks : MonoBehaviour
{
    public void PlusClicked()
    {
        Text txt = GameObject.Find("Amount").GetComponent<Text>();
        int nr = int.Parse(txt.text);
        if(nr < 5)
        {
            nr++;
            txt.text = nr.ToString();
        }
    }

    public void MinusClicked()
    {
        Text txt = GameObject.Find("Amount").GetComponent<Text>();
        int nr = int.Parse(txt.text);
        if(nr > 0)
        {
            nr--;
            txt.text = nr.ToString();
        } 
    }

    public void MaterialsClicked()
    {
        Text txt = GameObject.Find("SelectedMaterial").GetComponent<Text>();
        string material = EventSystem.current.currentSelectedGameObject.name;
        txt.text = material;
    }

    public void OfferClicked()
    {
        Text material = GameObject.Find("SelectedMaterial").GetComponent<Text>();
        Text amount = GameObject.Find("Amount").GetComponent<Text>();
    }
}
