using UnityEngine;

public class Interactable : MonoBehaviour
{
    private GameObject player; // 플레이어 GameObject 참조
    private bool isPlayerUsing = false; // 플레이어가 객체를 사용 중인지 여부

    public Vector3 playerRotationOffset; // 플레이어의 회전 오프셋
    public Vector3 playerPositionOffset; // 플레이어의 위치 오프셋

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
            isPlayerUsing = false; // 플레이어가 범위를 벗어날 때 사용 중지
        }
    }

    private void Update()
    {
        if (player != null && isPlayerUsing)
        {
            // 플레이어의 회전을 Interactable에 맞게 조절
            player.transform.rotation = Quaternion.Euler(playerRotationOffset);
            // 플레이어의 위치를 Interactable에 맞게 조절
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
