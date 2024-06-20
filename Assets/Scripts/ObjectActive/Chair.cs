using UnityEngine;

public class Chair : MonoBehaviour
{
    private GameObject player; // �÷��̾� GameObject ����
    private bool isPlayerSitting = false; // �÷��̾ �ɾ��ִ��� ����

    public Vector3 playerRotationOffset; // �÷��̾��� ȸ�� ������
    public Vector3 playerPositionOffset; // �÷��̾��� ��ġ ������

    private void OnTriggerEnter(Collider other)
    {
        if (!isPlayerSitting && other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void Update()
    {
        if (player != null && isPlayerSitting)
        {
            // �÷��̾��� ȸ���� Chair�� �°� ����
            player.transform.rotation = Quaternion.Euler(playerRotationOffset);
            // �÷��̾��� ��ġ�� Chair�� �°� ����
            player.transform.position = transform.TransformPoint(playerPositionOffset);
        }
    }

    public void PlayerSitDown()
    {
        isPlayerSitting = true;
    }

    public void PlayerStandUp()
    {
        isPlayerSitting = false;
    }
}
