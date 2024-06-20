using Photon.Pun;
using UnityEngine;

public class TriggerActivateObject : MonoBehaviour
{
    // Ȱ��ȭ/��Ȱ��ȭ�� ������Ʈ�� �Ҵ��մϴ�.
    public GameObject targetObject;

    // �÷��̾� �±׸� Ȯ���ϱ� ���� �ʿ��մϴ�.
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ���մϴ�.
        if (other.CompareTag(playerTag) && other.GetComponent<PhotonView>().IsMine)
        {
            // targetObject�� Ȱ��ȭ�մϴ�.
            targetObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ���մϴ�.
        if (other.CompareTag(playerTag) && other.GetComponent<PhotonView>().IsMine)
        {
            // targetObject�� ��Ȱ��ȭ�մϴ�.
            targetObject.SetActive(false);
        }
    }
}
