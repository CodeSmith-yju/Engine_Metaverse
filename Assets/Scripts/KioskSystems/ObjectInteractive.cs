using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteractive : MonoBehaviour
{
    public GameObject my;//해당 스크립트가 부착된 오브젝트 자신을 할당하는 변수
    
    //KioskSystem과의 상호작용을 프레임단위로 체크(useKioskNow)하고, 중복상호작용을 막기위한 변수(overlapCoroutine)
    private bool useKioskNow = false;
    private bool overlapCoroutine = false;
    private string parent_Tag;
    public Player player;

    private void Awake()
    {
        my = gameObject;
        my.GetComponent<ObjectInteractive>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        my.GetComponent<ObjectInteractive>().enabled = true;
        if (other.CompareTag("Player") )
        {
            player = other.GetComponent<Player>();
            ObjEnterChek();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObjStayChek();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        if (other.CompareTag("Player"))
        {
            ObjExitChek();
        }
        player = null;
        my.GetComponent<ObjectInteractive>().enabled = false;
    }

    private IEnumerator Interaction()
    {
        overlapCoroutine = true;

        //프레임단위로 키오스크와의 상호작용 키 입력을 체크하는 코드
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (parent_Tag)
                {
                    case "Kiosk":
                        useKioskNow = true;
                        //Debug.Log("충돌중인 오브젝트: " + my.transform.parent.name);
                        break;
                    case "Cup":
                        if (player.GetRole() == Role.Manager || player.GetRole() == Role.Empolyee)
                        {
                            player.cup = true;
                            Debug.Log("컵을 듦");
                        }
                        else
                        {
                            Debug.Log("권한이 없습니다.");
                        }
                        break;
                    case "POS":
                        KioskSystem.single.sellerImg.gameObject.SetActive(true);
                        Debug.Log("충돌한 오브젝트: " + parent_Tag);
                        break;
                }
            }
            yield return null; // 다음 프레임까지 대기
        }
    }

    public void PreOverlapKiosk()
    {
        overlapCoroutine = false;
        useKioskNow = false;
        my.gameObject.SetActive(false);
        my.gameObject.SetActive(true);
    }

    private void ObjEnterChek()
    {
        KioskSystem.single.announce.SetActive(true);// 상호작용가능한 범위에서 상호작용 키 이미지가 활성화되도록 함
        parent_Tag = my.transform.parent.tag;
        if (!overlapCoroutine)
        {
            StartCoroutine(Interaction());
        }
            
    }

    private void ObjStayChek()
    {
         // isTrigger가 체크된(통과 가능한) 오브젝트와 충돌한 상태가 유지될시, 각 오브젝트별로 상호작용이 이루어졌을 경우 발생할 이벤트를 등록
    }
    private void ObjExitChek()
    { 
        // isTrigger가 체크된(통과 가능한) 오브젝트와의 충돌 상태가 종료 되었을 때, 각 오브젝트별로 발생할 이벤트를 등록
        switch (parent_Tag)
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// 키오스크 상호작용화면 off
                PreOverlapKiosk();// 키오스크 상호작용, 중복상호작용 체크변수 초기화
                Debug.Log("충돌 끝난 오브젝트: " + parent_Tag);
                break;
            case "Cup":
                Debug.Log("충돌 끝난 오브젝트: " + parent_Tag);
                break;
            case "POS":
                Debug.Log("충돌 끝난 오브젝트: " + parent_Tag);
                break;
            default:
                break;
        }
        
        StopCoroutine(Interaction());// 실행중인 코루틴 종료
        overlapCoroutine = false;
        parent_Tag = null;
        KioskSystem.single.announce.SetActive(false);// 상호작용 키 이미지 비활성화
    }
}
