using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject player; // �÷��̾� GameObject ����
    private bool isPlayerUsing = false; // �÷��̾ ��ü�� ��� ������ ����

    public Vector3 playerRotationOffset; // �÷��̾��� ȸ�� ������
    public Vector3 playerPositionOffset; // �÷��̾��� ��ġ ������

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlayerUsing && other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isPlayerUsing = false; // �÷��̾ ������ ��� �� ��� ����
        }
    }

    private void Update()
    {
        if (player != null && isPlayerUsing)
        {
            // �÷��̾��� ȸ���� Interactable�� �°� ����
            player.transform.rotation = Quaternion.Euler(playerRotationOffset);
            // �÷��̾��� ��ġ�� Interactable�� �°� ����
            player.transform.position = transform.TransformPoint(playerPositionOffset);
        }
    }

    public void OnUseStart()
    {
        isPlayerUsing = true;
    }

    public void OnUseStop()
    {
        isPlayerUsing = false;
    }
}
