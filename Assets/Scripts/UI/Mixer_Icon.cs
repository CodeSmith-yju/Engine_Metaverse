using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mixer_Icon : MonoBehaviour
{
    public Transform inner;

    public void PrefabsMove()
    {
        foreach (Transform icons in GameMgr.Instance.ui.cup_List.transform) 
        {
            if (icons != null)
            {
                GameObject icon = icons.gameObject;
                if (icons.tag != "Mixer")
                {
                    RectTransform icon_Size = icon.GetComponent<RectTransform>();
                    icon_Size.sizeDelta = new Vector2(80f, 80f);
                    icon.transform.SetParent(inner);
                }
            }
            
        }
    }

}
