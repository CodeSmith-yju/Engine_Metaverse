using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ��ü���� UI ������ ���⼭ �ϸ� ������? �̹����� �ؽ�Ʈ �� Ű����ũ UI�� ����� �ű�� ������
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
