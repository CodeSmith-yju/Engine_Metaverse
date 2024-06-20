using Photon.Pun;
using UnityEngine;

public class TriggerActivateObject : MonoBehaviour
{
    // 활성화/비활성화할 오브젝트를 할당합니다.
    public GameObject targetObject;

    // 플레이어 태그를 확인하기 위해 필요합니다.
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 플레이어인지 확인합니다.
        if (other.CompareTag(playerTag) && other.GetComponent<PhotonView>().IsMine)
        {
            // targetObject를 활성화합니다.
            targetObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌한 오브젝트가 플레이어인지 확인합니다.
        if (other.CompareTag(playerTag) && other.GetComponent<PhotonView>().IsMine)
        {
            // targetObject를 비활성화합니다.
            targetObject.SetActive(false);
        }
    }
}
