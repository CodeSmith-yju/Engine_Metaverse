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

    private void Start()
    {
        my = gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.CompareTag("Player") )
        {
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
    }

    private IEnumerator OnKioskEvent()
    {
        //키오스크 중복 상호작용 발생을 예방하는 코드
        if (overlapCoroutine == true)
            yield break;

        //프레임단위로 키오스크와의 상호작용 키 입력을 체크하는 코드
        while (true)
        {
            if (useKioskNow == true)
            {
                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
                {
                    KioskSystem.single.KioskUsing();
                    overlapCoroutine = true;
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
        // isTrigger가 체크된(통과 가능한) 오브젝트와 충돌시, 충돌된 오브젝트의 부모 오브젝트의 이름을통해 상호작용
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                useKioskNow = true;
                // 아래의 상호작용 키 이미지가 활성화될때, my.transform.parent.name 이 코드를 통해 상호작용하는 오브젝트의 이름도 함께 출력할 수 있게 하면 좋을듯
                Debug.Log("충돌한 오브젝트: " + my.transform.parent.name);
                break;

            case "Cup":
                Debug.Log("충돌한 오브젝트: " + my.transform.parent.name);
                break;
            default:
                break;
        }
        KioskSystem.single.announce.SetActive(true);// 상호작용가능한 범위에서 상호작용 키 이미지가 활성화되도록 함
    }

    private void ObjStayChek()
    {
        // isTrigger가 체크된(통과 가능한) 오브젝트와 충돌한 상태가 유지될시, 각 오브젝트별로 상호작용이 이루어졌을 경우 발생할 이벤트를 등록
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                useKioskNow = true;
                StartCoroutine(OnKioskEvent());
                //Debug.Log("충돌중인 오브젝트: " + my.transform.parent.name);
                break;
            case "Cup":
                Debug.Log("충돌중인 오브젝트: " + my.transform.parent.name);
                break;
            default:
                break;
        }
    }
    private void ObjExitChek()
    {
        // isTrigger가 체크된(통과 가능한) 오브젝트와의 충돌 상태가 종료 되었을 때, 각 오브젝트별로 발생할 이벤트를 등록
        switch (my.transform.parent.name)
        {
            case "Kiosk":
                KioskSystem.single.OnQuiteKiosk();// 키오스크 상호작용화면 off
                PreOverlapKiosk();// 키오스크 상호작용, 중복상호작용 체크변수 초기화
                
                StopCoroutine("OnKioskEvnet");// 실행중인 코루틴 종료
                KioskSystem.single.announce.SetActive(false);// 상호작용 키 이미지 비활성화
                Debug.Log("충돌 끝난 오브젝트: " + my.transform.parent.name);
                break;
            case "Cup":
                Debug.Log("충돌 끝난 오브젝트: " + my.transform.parent.name);
                break;
            default:
                break;
        }
        KioskSystem.single.announce.SetActive(false);// 상호작용 키 이미지 비활성화
        my.transform.parent.name = null;
    }
}
