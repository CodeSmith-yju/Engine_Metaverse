using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    bool isEnter = false;
    bool isCheck = false;
    string ingredient = null;
    bool isHot = false;
    Player player;

    // ������ ������Ʈ �̸����� ���� �� ���� ����� ������ �װɷ� �ٲ� ����
    private void Start()
    {
        if (name == "Water" && tag == "Hot")
        {
            isHot = true;
        }

        switch(name)
        {
            case "Espresso":
                ingredient = "����������";
                break;
            case "Strawberry":
                ingredient = "����";
                break;
            case "Chocolate":
                ingredient = "���ݸ�";
                break;
            case "Milk":
                ingredient = "����";
                break;
            case "Mixer":
                ingredient = "�ͼ���";
                break;
            case "Water":
                if (isHot)
                {
                    ingredient = "�¼�";
                }
                else
                {
                    ingredient = "�ü�";
                }
                break;
            case "Ice":
                ingredient = "����";
                break;
        }
    }

    private void Update()
    {
        if (isCheck && isEnter) 
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InteractWithPlayer(player);
            }
        }
        else if (!isCheck && isEnter)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�����̰ų� ������ ��ȣ�ۿ� ������ ������Ʈ�Դϴ�.");
                return;
            }
        }
    }

    // �÷��̾� �±׸� ���� ������Ʈ�� �ε�ġ�� ������Ʈ�� �浹 üũ �׽�Ʈ�� ( ���߿��� ����ĳ��Ʈ�� �̿��ؼ� �繰�� �ٶ󺸰� ���� �� �� �� ���� )
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isEnter = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        player = collision.gameObject.GetComponent<Player>();

        if (isEnter && collision.collider.CompareTag("Player") && player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
        {
            isCheck = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isCheck = false;
        isEnter = false;
    }

    private void InteractWithPlayer(Player player)
    {
        if (name == "Done")
        {
            if (player.cup && player.done)
            {
                Debug.Log("���� �Ϸ� �մ� ����");
                player.Done();
            }
            else
            {
                Debug.Log("�ϼ��� ���Ḧ ������ ���� �ʰų�, ������ �ʾҽ��ϴ�.");
                return;
            }

        }
        else if (name == "Create" && player.cup)
        {
            Debug.Log("���� �õ�");
            player.Create();
        }
        else if (name == "Role")
        {
            if (player.GetRole() == Role.Manager) // ������ �Ŵ����� ��쿡�� ��밡���ϰ�
            {
                Debug.Log("���� ����");
                GameMgr.Instance.player_List[0].gameObject.GetComponent<Player>().SetRole(Role.Empolyee);
                // �� �ڵ带 �÷��̾� ����Ʈ�� �����ͼ� UI���� �����ϴ� �ɷ� �ٲ� ����
            }
            else
            {
                Debug.Log("������ �����ϴ�.");
                return;
            }
        }
        else if (name == "Cup")
        {
            if (player.cup == false)
            {
                Debug.Log("�տ� �� ��");
                player.cup = true;
            }
            else
            {
                Debug.Log("�̹� ���� ��� ����");
                return;
            }
        }
        else
        {
            if (player.done == false && player.cup == true && name != "Done" && name != "Role")
            {
                player.cur_IngrList.Add(ingredient);
                Debug.Log(ingredient + " ��� �߰�");
            }
            else
            {
                Debug.Log("���� ��� ���� �ʽ��ϴ�.");
                return;
            }
        }
    }
}
