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

    private void Start()
    {
        my = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.CompareTag("Player") )
        {
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
    }

    private IEnumerator OnKioskEvent()
    {
        //Ű����ũ �ߺ� ��ȣ�ۿ� �߻��� �����ϴ� �ڵ�
        if (overlapCoroutine == true)
            yield break;

        //�����Ӵ����� Ű����ũ���� ��ȣ�ۿ� Ű �Է��� üũ�ϴ� �ڵ�
        while (true)
        {
            if (useKioskNow == true)
            {
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
                {
                    KioskSystem.single.KioskUsing();
                    overlapCoroutine = true;
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
        // isTrigger�� üũ��(��� ������) ������Ʈ�� �浹��, �浹�� ������Ʈ�� �θ� ������Ʈ�� �̸������� ��ȣ�ۿ�
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                useKioskNow = true;
                // �Ʒ��� ��ȣ�ۿ� Ű �̹����� Ȱ��ȭ�ɶ�, my.transform.parent.name �� �ڵ带 ���� ��ȣ�ۿ��ϴ� ������Ʈ�� �̸��� �Բ� ����� �� �ְ� �ϸ� ������
                Debug.Log("�浹�� ������Ʈ: " + my.transform.parent.name);
                break;

            case "Cup":
                Debug.Log("�浹�� ������Ʈ: " + my.transform.parent.name);
                break;
            default:
                break;
        }
        KioskSystem.single.announce.SetActive(true);// ��ȣ�ۿ밡���� �������� ��ȣ�ۿ� Ű �̹����� Ȱ��ȭ�ǵ��� ��
    }

    private void ObjStayChek()
    {
        // isTrigger�� üũ��(��� ������) ������Ʈ�� �浹�� ���°� �����ɽ�, �� ������Ʈ���� ��ȣ�ۿ��� �̷������ ��� �߻��� �̺�Ʈ�� ���
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                useKioskNow = true;
                StartCoroutine(OnKioskEvent());
                //Debug.Log("�浹���� ������Ʈ: " + my.transform.parent.name);
                break;
            case "Cup":
                Debug.Log("�浹���� ������Ʈ: " + my.transform.parent.name);
                break;
            default:
                break;
        }
    }
    private void ObjExitChek()
    {
        // isTrigger�� üũ��(��� ������) ������Ʈ���� �浹 ���°� ���� �Ǿ��� ��, �� ������Ʈ���� �߻��� �̺�Ʈ�� ���
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// Ű����ũ ��ȣ�ۿ�ȭ�� off
                PreOverlapKiosk();// Ű����ũ ��ȣ�ۿ�, �ߺ���ȣ�ۿ� üũ���� �ʱ�ȭ
                
                StopCoroutine("OnKioskEvnet");// �������� �ڷ�ƾ ����
                KioskSystem.single.announce.SetActive(false);// ��ȣ�ۿ� Ű �̹��� ��Ȱ��ȭ
                Debug.Log("�浹 ���� ������Ʈ: " + my.transform.parent.name);
                break;
            case "Cup":
                Debug.Log("�浹 ���� ������Ʈ: " + my.transform.parent.name);
                break;
            default:
                break;
        }
        KioskSystem.single.announce.SetActive(false);// ��ȣ�ۿ� Ű �̹��� ��Ȱ��ȭ
        my.transform.parent.name = null;
    }
}