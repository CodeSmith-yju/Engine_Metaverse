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
    bool coffee_Check = false;
    bool coffee_Done = false;
    Player player;
    List<string> temp_List = new List<string>();

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
                Debug.Log("�浹�� ������Ʈ: " + parent_Tag);
                break;
            case "Grinder":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.coffee)
                    {
                        Debug.Log("�̹� Ŀ�� ���縦 ������ �ֽ��ϴ�.");
                        return;
                    }
                    player.coffee = true;
                    Debug.Log("Ŀ�� ���� ���");
                }
                else
                {
                    Debug.Log("������ �����ϴ�.");
                    return;
                }
                break;
            case "Espresso":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (coffee_Check && player.cup)
                    {
                        Debug.Log("Ŀ�Ǹ� �����ϴ�. (30��)");
                        coffee_Check = false;

                        foreach (string ingr in player.cur_IngrList)
                        {
                            temp_List.Add(ingr);
                        }

                        player.cur_IngrList.Clear();
                        player.cup = false;

                        StartCoroutine(CoffeeRoutine());
                        return;
                    }

                    if (coffee_Done && !player.cup) 
                    {
                        player.cup = true;
                        Debug.Log("���������Ұ� ��� �� ��������");
                        foreach (string ingr in temp_List)
                        {
                            player.cur_IngrList.Add(ingr);
                        }
                        player.cur_IngrList.Add("����������");
                        return;
                    }

                    if (player.coffee && !coffee_Check)
                    {
                        Debug.Log("Ŀ�� ���縦 Ŀ�� �ӽſ� �־����ϴ�.");
                        coffee_Check = true;
                        player.coffee = false;
                    }
                    else if (player.coffee && coffee_Check)
                    {
                        Debug.Log("Ŀ�� �ӽſ� �̹� Ŀ�� ���簡 ��� �� �ֽ��ϴ�.");
                        return;
                    }
                    else
                    {
                        Debug.Log("Ŀ�Ǹ� ������ �ְų� Ŀ�� ���縦 ������ ���� �ʽ��ϴ�.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("������ �����ϴ�.");
                    return;
                }
                break;
            case "Ice":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.cup)
                    {
                        player.cur_IngrList.Add("����");
                        Debug.Log("�ſ� ���� �ֱ�");
                    }
                    else
                    {
                        Debug.Log("���� ��� ���� �ʽ��ϴ�.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("������ �����ϴ�.");
                    return;
                }
                break;
            case "Done":
                if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                {
                    if (player.done)
                    {
                        Debug.Log("���� ���� �Ϸ�" + player.cur_Ordered_Menu);
                        player.Done();
                    }
                    else
                    {
                        Debug.Log("���� ������ �Ϸ� ���� �ʾҽ��ϴ�.");
                        return;
                    }
                }
                else
                {
                    Debug.Log("������ �����ϴ�");
                    return;
                }
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
    
    private IEnumerator CoffeeRoutine()
    {
        yield return StartCoroutine(Espresso());

        Debug.Log("Ŀ�� ������ �Ϸ�");
        coffee_Done = true;
    }


    private IEnumerator Espresso()
    {
        CoffeMachine coffemachine = my.GetComponent<CoffeMachine>();
        coffemachine.StartTimer(30f);
        yield return new WaitForSeconds(30);
    }
}
