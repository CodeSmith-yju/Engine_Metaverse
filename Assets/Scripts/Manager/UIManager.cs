using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 전체적인 UI 관리를 여기서 하면 좋을듯? 이미지나 텍스트 등 키오스크 UI도 여기로 옮기면 좋을듯
    private static UIManager instance_UI = null;

    public static UIManager Instance_UI
    {
        get
        {
            if (null == instance_UI)
            {
                return null;
            }
            return instance_UI;
        }
    }





    private void Awake()
    {
        if (null == instance_UI)
        {
            instance_UI = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
