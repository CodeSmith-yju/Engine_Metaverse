using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin_Init : MonoBehaviour
{
    public TMP_Text coin_Text;

    public void Init(int coin)
    {
        coin_Text.text = coin.ToString();
    }
}
