using UnityEngine;
using Photon.Pun;

public class PlayerUse : MonoBehaviourPunCallbacks
{
    private bool canUseObject = false; // ��ü�� ����� �� �ִ� ���¸� üũ
    public Character_Controller characterController; // �÷��̾��� CharacterController
    public Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private Interactable currentInteractable; // ���� ��� ���� ��ȣ�ۿ� ������ ��ü
    public Players player; // �÷��̾� ��ü

    private bool isUsingObject = false; // ��ü ��� ������ ����

    private void Start()
    {
        // �ʱ�ȭ �ڵ� �߰�
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // ���� �÷��̾ �ƴϸ� ��ȯ

        if (canUseObject && Input.GetKeyDown(KeyCode.F) && !isUsingObject)
        {
            if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
            {
                StartUsingObject();
                // 2�� �Ŀ� StopUsingObject �޼��带 ȣ��
                Invoke("StopUsingObject", 3.5f);
            }
            else
            {
                Debug.Log("������ �����ϴ�.");
            }
        }
    }

    private void StartUsingObject()
    {
        if (characterController == null)
        {
            Debug.LogError("CharacterController�� �����ϴ�. �÷��̾� ���� ������Ʈ�� CharacterController�� �߰��ؾ� �մϴ�.");
            return;
        }

        if (currentInteractable != null)
        {
            playerAnimator.SetBool("Working", true); // "Working" Ʈ���Ÿ� ȣ���Ͽ� ��� �ִϸ��̼� ���
            currentInteractable.OnUseStart(); // Interactable ��ũ��Ʈ�� ��� ���� ���� ����
        }

        // �÷��̾��� �������� ��Ȱ��ȭ
        characterController.enabled = false;

        isUsingObject = true; // ��ü ��� �� ���·� ����

        Debug.Log("�÷��̾ ��ü�� ��� ���Դϴ�."); // ����� ���� �߰�
    }

    private void StopUsingObject()
    {
        if (currentInteractable != null)
        {
            playerAnimator.SetBool("Working", false); // Working �ִϸ��̼� ���� ����
            currentInteractable.OnUseStop(); // Interactable ��ũ��Ʈ�� ��� ���� ���� ����
        }

        if (characterController != null)
        {
            characterController.enabled = true; // �÷��̾��� �������� Ȱ��ȭ
        }

        isUsingObject = false; // ��ü ��� �� ���� ����

        Debug.Log("�÷��̾ ��ü ����� �����߽��ϴ�."); // ����� ���� �߰�
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            canUseObject = true;
            currentInteractable = other.GetComponent<Interactable>(); // ���� Interactable ��ũ��Ʈ ��������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            canUseObject = false;
            currentInteractable = null;
            StopUsingObject(); // ��ȣ�ۿ� ������ ��� �� ����� ����
        }
    }
}
