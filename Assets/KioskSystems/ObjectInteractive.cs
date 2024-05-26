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
    private void OnTriggerEnter(Collider other)//Debug.Log("Trigger Enter");
    {
        if (!ChekRole(other))
            return;

        if (other.CompareTag("Player") )
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
                case "POS_Machine":
                    Debug.Log("�浹�� ������Ʈ: " + my.transform.parent.name);
                    
                    break;
                default:
                    break;
            }
            KioskSystem.single.announce.SetActive(true);// ��ȣ�ۿ밡���� �������� ��ȣ�ۿ� Ű �̹����� Ȱ��ȭ�ǵ��� ��
        }
    }
    private void OnTriggerStay(Collider other)//Debug.Log("Stay Enter");
    {
        if (!ChekRole(other))
            return;

        if (other.CompareTag("Player"))
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
                case "POS_Machine":
                    //�浹���� ������� ������ Ȯ���Ͽ� �Ŵ����� �ƴϸ� �ǵ���
                    OnSeller();
                    Debug.Log("�浹�� ������Ʈ: " + my.transform.parent.name);
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)//Debug.Log("Trigger Exit");
    {
        /*if (!ChekRole(other))
            return;*/

        if (other.CompareTag("Player"))
        {
            string parentName = my.transform.parent.name;
            // isTrigger�� üũ��(��� ������) ������Ʈ���� �浹 ���°� ���� �Ǿ��� ��, �� ������Ʈ���� �߻��� �̺�Ʈ�� ���
            switch (parentName)
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
                case "POS_Machine":
                    Debug.Log("�浹 ���� ������Ʈ: " + my.transform.parent.name);
                    KioskSystem.single.sellerImg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            KioskSystem.single.announce.SetActive(false);// ��ȣ�ۿ� Ű �̹��� ��Ȱ��ȭ
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
    private void OnSeller()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
        {
            KioskSystem.single.sellerImg.gameObject.SetActive(true);
        }
    }

    public void PreOverlapKiosk()
    {
        overlapCoroutine = false;
        useKioskNow = false;
        
        //���� �ʱ�ȭ���ָ鼭 �浹�����Ұ͵� ���̳־���� ����-�浹���λ�Ȳ���� F������ ��ȣ�ۿ� UI�� �˾��ߴµ� �˾�UI�� ���� �� �� �ٽ� �ش� UI�������� �����ڵ忡���� �ٽ� �浹���¿� ������, �׷����־�� �ٽ� �浹���Ѿ��ؼ�
        my.SetActive(false);
        my.SetActive(true);
    }

    private bool ChekRole(Collider _other)//����� ������ Ȯ���ؼ� �Ŵ������ƴϸ� ���ٸ��ϰ� ���ƹ����� �޼���
    {
        Player player = _other.GetComponent<Player>();
        if (player.GetRole() != Role.Manager)
        {
            Debug.Log("����� ����: " + player.GetRole().ToString());
            return false;
        }
        else Debug.Log("����� ����: " + player.GetRole().ToString());
        return true;
    }

    /* �ڵ� ����ϰ� �����Ҽ������Ű��Ƽ� �־��µ� �� �����ϰ� ohter������ �Ű����� �־��ְ� �Ϸ��ϱ� ���� �����Ѱ͸� �þ�� �׳� ����
    private void ObjEnterChek()
    {
        
    }

    private void ObjStayChek()
    {
        
    }
    private void ObjExitChek()
    {
        
    }*/
}
