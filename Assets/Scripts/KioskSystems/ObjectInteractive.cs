using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteractive : MonoBehaviour
{
    public GameObject my;//�ش� ��ũ��Ʈ�� ������ ������Ʈ �ڽ��� �Ҵ��ϴ� ����
    
    //KioskSystem���� ��ȣ�ۿ��� �����Ӵ����� üũ(useKioskNow)�ϰ�, �ߺ���ȣ�ۿ��� �������� ����(overlapCoroutine)
    private bool useKioskNow = false;
    private bool overlapCoroutine = false;
    private string parent_Tag;
    public Player player;

    private void Awake()
    {
        my = gameObject;
        my.GetComponent<ObjectInteractive>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        my.GetComponent<ObjectInteractive>().enabled = true;
        if (other.CompareTag("Player") )
        {
            player = other.GetComponent<Player>();
            ObjEnterChek();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjStayChek();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        if (other.CompareTag("Player"))
        {
            ObjExitChek();
        }
        player = null;
        my.GetComponent<ObjectInteractive>().enabled = false;
    }

    private IEnumerator Interaction()
    {
        overlapCoroutine = true;

        //�����Ӵ����� Ű����ũ���� ��ȣ�ۿ� Ű �Է��� üũ�ϴ� �ڵ�
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (parent_Tag)
                {
                    case "Kiosk":
                        useKioskNow = true;
                        //Debug.Log("�浹���� ������Ʈ: " + my.transform.parent.name);
                        break;
                    case "Cup":
                        if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                        {
                            player.cup = true;
                            Debug.Log("���� ��");
                        }
                        else
                        {
                            Debug.Log("������ �����ϴ�.");
                        }
                        break;
                    case "POS":
                        KioskSystem.single.sellerImg.gameObject.SetActive(true);
                        Debug.Log("�浹�� ������Ʈ: " + parent_Tag);
                        break;
                }
            }
            yield return null; // ���� �����ӱ��� ���
        }
    }

    public void PreOverlapKiosk()
    {
        overlapCoroutine = false;
        useKioskNow = false;
        my.gameObject.SetActive(false);
        my.gameObject.SetActive(true);
    }

    private void ObjEnterChek()
    {
        KioskSystem.single.announce.SetActive(true);// ��ȣ�ۿ밡���� �������� ��ȣ�ۿ� Ű �̹����� Ȱ��ȭ�ǵ��� ��
        parent_Tag = my.transform.parent.tag;
        if (!overlapCoroutine)
        {
            StartCoroutine(Interaction());
        }
            
    }

    private void ObjStayChek()
    {
         // isTrigger�� üũ��(��� ������) ������Ʈ�� �浹�� ���°� �����ɽ�, �� ������Ʈ���� ��ȣ�ۿ��� �̷������ ��� �߻��� �̺�Ʈ�� ���
    }
    private void ObjExitChek()
    { 
        // isTrigger�� üũ��(��� ������) ������Ʈ���� �浹 ���°� ���� �Ǿ��� ��, �� ������Ʈ���� �߻��� �̺�Ʈ�� ���
        switch (parent_Tag)
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// Ű����ũ ��ȣ�ۿ�ȭ�� off
                PreOverlapKiosk();// Ű����ũ ��ȣ�ۿ�, �ߺ���ȣ�ۿ� üũ���� �ʱ�ȭ
                Debug.Log("�浹 ���� ������Ʈ: " + parent_Tag);
                break;
            case "Cup":
                Debug.Log("�浹 ���� ������Ʈ: " + parent_Tag);
                break;
            case "POS":
                Debug.Log("�浹 ���� ������Ʈ: " + parent_Tag);
                break;
            default:
                break;
        }
        
        StopCoroutine(Interaction());// �������� �ڷ�ƾ ����
        overlapCoroutine = false;
        parent_Tag = null;
        KioskSystem.single.announce.SetActive(false);// ��ȣ�ۿ� Ű �̹��� ��Ȱ��ȭ
    }
}
