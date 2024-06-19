using UnityEngine;
using Photon.Pun;

public class PlayerUse : MonoBehaviourPunCallbacks
{
    private bool canUseObject = false; // 객체를 사용할 수 있는 상태를 체크
    public Character_Controller characterController; // 플레이어의 CharacterController
    public Animator playerAnimator; // 플레이어의 애니메이터
    private Interactable currentInteractable; // 현재 사용 중인 상호작용 가능한 객체
    public Players player; // 플레이어 객체

    private bool isUsingObject = false; // 객체 사용 중인지 여부

    private void Start()
    {
        // 초기화 코드 추가
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // 로컬 플레이어가 아니면 반환

        if (canUseObject && Input.GetKeyDown(KeyCode.F) && !isUsingObject)
        {
            if (player.GetRole() == Role.Manager || player.GetRole() == Role.Employee)
            {
                StartUsingObject();
                // 2초 후에 StopUsingObject 메서드를 호출
                Invoke("StopUsingObject", 3.5f);
            }
            else
            {
                Debug.Log("권한이 없습니다.");
            }
        }
    }

    private void StartUsingObject()
    {
        if (characterController == null)
        {
            Debug.LogError("CharacterController가 없습니다. 플레이어 게임 오브젝트에 CharacterController를 추가해야 합니다.");
            return;
        }

        if (currentInteractable != null)
        {
            playerAnimator.SetBool("Working", true); // "Working" 트리거를 호출하여 사용 애니메이션 재생
            currentInteractable.OnUseStart(); // Interactable 스크립트에 사용 시작 상태 전달
        }

        // 플레이어의 움직임을 비활성화
        characterController.enabled = false;

        isUsingObject = true; // 객체 사용 중 상태로 설정

        Debug.Log("플레이어가 객체를 사용 중입니다."); // 디버그 문구 추가
    }

    private void StopUsingObject()
    {
        if (currentInteractable != null)
        {
            playerAnimator.SetBool("Working", false); // Working 애니메이션 상태 해제
            currentInteractable.OnUseStop(); // Interactable 스크립트에 사용 종료 상태 전달
        }

        if (characterController != null)
        {
            characterController.enabled = true; // 플레이어의 움직임을 활성화
        }

        isUsingObject = false; // 객체 사용 중 상태 해제

        Debug.Log("플레이어가 객체 사용을 종료했습니다."); // 디버그 문구 추가
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            canUseObject = true;
            currentInteractable = other.GetComponent<Interactable>(); // 현재 Interactable 스크립트 가져오기
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            canUseObject = false;
            currentInteractable = null;
            StopUsingObject(); // 상호작용 범위를 벗어날 때 사용을 중지
        }
    }
}
