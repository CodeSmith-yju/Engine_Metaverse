using UnityEngine;
using Photon.Pun;

public class PlayerSit : MonoBehaviourPunCallbacks
{
    private bool isSitting = false; // 앉아있는 상태를 체크
    private bool canSit = false; // 앉을 수 있는 상태를 체크
    private Character_Controller ch_con; // 플레이어의 CharacterController
    private Animator animator; // 플레이어의 애니메이터
    private Chair currentChair; // 현재 앉은 Chair 스크립트

    private void Start()
    {
        ch_con = GetComponent<Character_Controller>(); // CharacterController 컴포넌트 초기화
        if (ch_con == null)
        {
            Debug.LogError("Character_Controller 컴포넌트를 찾을 수 없습니다.");
        }
        animator = GetComponent<Animator>(); // Animator 컴포넌트 초기화
        if (animator == null)
        {
            Debug.LogError("Animator 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // 로컬 플레이어가 아니면 반환

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
        animator.SetTrigger("Sit"); // "Sit" 트리거를 호출하여 앉기 애니메이션 재생
        currentChair.PlayerSitDown(); // Chair 스크립트에 앉기 상태 전달

        // 플레이어의 움직임을 비활성화
        ch_con.enabled = false;

        Debug.Log("플레이어가 앉았습니다."); // 디버그 문구 추가
    }

    private void StandUp()
    {
        isSitting = false;
        animator.SetBool("Sit", false); // Sit 애니메이션 상태 해제
        currentChair.PlayerStandUp(); // Chair 스크립트에 일어나기 상태 전달

        // 플레이어의 움직임을 활성화
        ch_con.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chair"))
        {
            canSit = true;
            currentChair = other.GetComponent<Chair>(); // 현재 Chair 스크립트 가져오기
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
