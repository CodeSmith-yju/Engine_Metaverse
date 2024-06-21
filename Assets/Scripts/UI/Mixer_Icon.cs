using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer_Icon : MonoBehaviour
{
    public Transform inner;
    List<Transform> iconsToMove = new List<Transform>();

    public void PrefabsMove()
    {
        foreach (Transform icons in GameMgr.Instance.ui.cup_List.transform) 
        {
            if (icons != null && icons.tag != "Mixer")
            {
                GameObject icon = icons.gameObject;
                iconsToMove.Add(icons);
            }
        }

        foreach (Transform icon in iconsToMove)
        {
            RectTransform icon_Size = icon.GetComponent<RectTransform>();
            icon_Size.sizeDelta = new Vector2(80f, 80f);
            icon.SetParent(inner);
        }

    }

}
