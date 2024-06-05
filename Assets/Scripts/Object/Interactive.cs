using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    GameObject my;
    string parent_Tag;
    bool isEnter = false;
    bool isCheck = false;
    Player player;
    bool isCoffee = false;

    private void Awake()
    {
        my = this.gameObject;
    }

    // ������ ������Ʈ �̸����� ���� �� ���� ����� ������ �װɷ� �ٲ� ����
    private void Start()
    {
        parent_Tag = my.transform.parent.tag;
        my.GetComponent<Interactive>().enabled = false;
    }

    private void Update()
    {
        if (isCheck && isEnter) 
        {
            ActiveObjectName(parent_Tag);
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractWithPlayer(player, parent_Tag);
            }
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        my.GetComponent<Interactive>().enabled = true;
        KioskSystem.single.announce.SetActive(true);
        if (other.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player>();
            isEnter = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractExitPlayer(parent_Tag); // player �Ű������� �ʿ��ϸ� �߰� ����
        player = null;
        my.GetComponent<Interactive>().enabled = false;
    }

    private void InteractWithPlayer(Player player, string obj_Tag)
    {
        switch (obj_Tag)
        {
            case "Kiosk":
                Debug.Log("Ű����ũ ����");
                KioskSystem.single.KioskUsing();
                break;
            case "Cup":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.cup)
                    {
                        Debug.Log("�̹� ���� ��� �ֽ��ϴ�.");
                        return;
                    }
                    player.cup = true;
                    Debug.Log("���� ��");
                }
                else
                {
                    Debug.Log("������ �����ϴ�.");
                    return;
                }
                break;
            case "POS":
                KioskSystem.single.sellerImg.gameObject.SetActive(true);
                KioskSystem.single.kiosck = true;
                Debug.Log("�浹�� ������Ʈ: " + parent_Tag);
                break;
            case "Espresso":
                Debug.Log("Ŀ�Ǹӽ�");//��ȣ�ۿ������ϱ�
                CoffeMachine coffemachine = my.GetComponent<CoffeMachine>();
                coffemachine.StartTimer(30f);

                break;
            default:
                return;
        }
    }

    private void InteractExitPlayer(string obj_Tag)
    {
        isEnter = false;
        isCheck = false;

        switch (obj_Tag) 
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// Ű����ũ ��ȣ�ۿ�ȭ�� off
                break;
            case "POS":
                KioskSystem.single.sellerImg.gameObject.SetActive(false);
                KioskSystem.single.kiosck = false;
                break;
            default:
                Debug.Log("��ȣ�ۿ� Ʈ���ſ��� ���");
                break;
        }

        KioskSystem.single.announce.SetActive(false);
        KioskSystem.single.textannounce.gameObject.SetActive(false);
    }

    private void ActiveObjectName(string _str)
    {
        switch (_str)
        {
            case "Kiosk":
                KioskSystem.single.textannounce.text = "Ű����ũ";
                break;
            case "POS":
                KioskSystem.single.textannounce.text = "������";
                break;
            default:
                KioskSystem.single.textannounce.text = _str;
                break;
        }
        KioskSystem.single.textannounce.gameObject.SetActive(true);
    }
    
}
