using UnityEngine;

public class Chair : MonoBehaviour
{
    private GameObject player; // 플레이어 GameObject 참조
    private bool isPlayerSitting = false; // 플레이어가 앉아있는지 여부

    public Vector3 playerRotationOffset; // 플레이어의 회전 오프셋
    public Vector3 playerPositionOffset; // 플레이어의 위치 오프셋

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
            // 플레이어의 회전을 Chair에 맞게 조절
            player.transform.rotation = Quaternion.Euler(playerRotationOffset);
            // 플레이어의 위치를 Chair에 맞게 조절
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
