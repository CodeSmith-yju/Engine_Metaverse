using UnityEngine;
using Photon.Pun;

public class PlayerSit : MonoBehaviourPunCallbacks
{
    private bool isSitting = false; // �ɾ��ִ� ���¸� üũ
    private bool canSit = false; // ���� �� �ִ� ���¸� üũ
    private Character_Controller ch_con; // �÷��̾��� CharacterController
    private Animator animator; // �÷��̾��� �ִϸ�����
    private Chair currentChair; // ���� ���� Chair ��ũ��Ʈ

    private void Start()
    {
        ch_con = GetComponent<Character_Controller>(); // CharacterController ������Ʈ �ʱ�ȭ
        if (ch_con == null)
        {
            Debug.LogError("Character_Controller ������Ʈ�� ã�� �� �����ϴ�.");
        }
        animator = GetComponent<Animator>(); // Animator ������Ʈ �ʱ�ȭ
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // ���� �÷��̾ �ƴϸ� ��ȯ

        if (canSit && Input.GetKeyDown(KeyCode.F))
        {
            if (isSitting)
            {
                StandUp();
            }
            else
            {
                SitDown();
            }
        }
    }

    private void SitDown()
    {
        isSitting = true;
        animator.SetTrigger("Sit"); // "Sit" Ʈ���Ÿ� ȣ���Ͽ� �ɱ� �ִϸ��̼� ���
        currentChair.PlayerSitDown(); // Chair ��ũ��Ʈ�� �ɱ� ���� ����

        // �÷��̾��� �������� ��Ȱ��ȭ
        ch_con.enabled = false;

        Debug.Log("�÷��̾ �ɾҽ��ϴ�."); // ����� ���� �߰�
    }

    private void StandUp()
    {
        isSitting = false;
        animator.SetBool("Sit", false); // Sit �ִϸ��̼� ���� ����
        currentChair.PlayerStandUp(); // Chair ��ũ��Ʈ�� �Ͼ�� ���� ����

        // �÷��̾��� �������� Ȱ��ȭ
        ch_con.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            canSit = true;
            currentChair = other.GetComponent<Chair>(); // ���� Chair ��ũ��Ʈ ��������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            canSit = false;
            currentChair = null;
        }
    }
}
