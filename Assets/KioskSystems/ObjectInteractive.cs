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
    private void OnTriggerEnter(Collider other)//Debug.Log("Trigger Enter");
    {
        if (!ChekRole(other))
            return;

        if (other.CompareTag("Player") )
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
                case "POS_Machine":
                    Debug.Log("충돌한 오브젝트: " + my.transform.parent.name);
                    
                    break;
                default:
                    break;
            }
            KioskSystem.single.announce.SetActive(true);// 상호작용가능한 범위에서 상호작용 키 이미지가 활성화되도록 함
        }
    }
    private void OnTriggerStay(Collider other)//Debug.Log("Stay Enter");
    {
        if (!ChekRole(other))
            return;

        if (other.CompareTag("Player"))
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
                case "POS_Machine":
                    //충돌중인 사용자의 권한을 확인하여 매니저가 아니면 되돌림
                    OnSeller();
                    Debug.Log("충돌한 오브젝트: " + my.transform.parent.name);
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit(Collider other)//Debug.Log("Trigger Exit");
    {
        /*if (!ChekRole(other))
            return;*/

        if (other.CompareTag("Player"))
        {
            string parentName = my.transform.parent.name;
            // isTrigger가 체크된(통과 가능한) 오브젝트와의 충돌 상태가 종료 되었을 때, 각 오브젝트별로 발생할 이벤트를 등록
            switch (parentName)
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
                case "POS_Machine":
                    Debug.Log("충돌 끝난 오브젝트: " + my.transform.parent.name);
                    KioskSystem.single.sellerImg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
            KioskSystem.single.announce.SetActive(false);// 상호작용 키 이미지 비활성화
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
    private void OnSeller()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space))
        {
            KioskSystem.single.sellerImg.gameObject.SetActive(true);
        }
    }

    public void PreOverlapKiosk()
    {
        overlapCoroutine = false;
        useKioskNow = false;
        
        //변수 초기화해주면서 충돌리셋할것도 같이넣어놨음 예시-충돌중인상황에서 F눌러서 상호작용 UI를 팝업했는데 팝업UI를 종료 한 후 다시 해당 UI를띄우려면 지금코드에서는 다시 충돌상태에 들어가야함, 그래서넣어둠 다시 충돌시켜야해서
        my.SetActive(false);
        my.SetActive(true);
    }

    private bool ChekRole(Collider _other)//사용자 권한을 확인해서 매니저가아니면 접근못하게 막아버리는 메서드
    {
        Player player = _other.GetComponent<Player>();
        if (player.GetRole() != Role.Manager)
        {
            Debug.Log("당신의 권한: " + player.GetRole().ToString());
            return false;
        }
        else Debug.Log("당신의 권한: " + player.GetRole().ToString());
        return true;
    }

    /* 코드 깔끔하게 관리할수있을거같아서 넣었는데 더 복잡하고 ohter변수로 매개변수 넣어주고 하려니까 과정 복잡한것만 늘어나서 그냥 뺐음
    private void ObjEnterChek()
    {
        
    }

    private void ObjStayChek()
    {
        
    }
    private void ObjExitChek()
    {
        
    }*/
}
