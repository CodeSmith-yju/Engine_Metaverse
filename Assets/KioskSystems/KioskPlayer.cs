using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KioskPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public float interactionDistance = 4f; // 상호작용 가능한 거리
    //public LayerMask interactableLayer; // 상호작용 가능한 Layer
    public bool usingChek = false;

    //public GameObject go;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDerection = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(moveDerection * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Z))
        {
            TryInteract();
        }

        if (usingChek == true)
        {
            //go.SetActive(true);
        }

    }

    public void TryInteract()
    {
        //interactableLayer = LayerMask.GetMask("Object");// Layer가 Object일때만 RayCast가 작동하도록 한정
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            usingChek = true;
            GameObject interactedObject = hit.collider.gameObject;// 상호작용할 오브젝트를 찾았을 때 처리

            switch (interactedObject.name)//상호작용한 오브젝트의 이름에따라 다른 상호작용을 할 수 있게 함.
            {
                case "Kiosk":
                    KioskSystem.single.btnQuiteKiosk.gameObject.SetActive(true);
                    if (KioskSystem.single.kioIndex == 0)
                    {
                        KioskSystem.single.kioskScene[0].SetActive(true);
                    }
                    if (KioskSystem.single.buyCheck == false)
                    {
                        KioskSystem.single.KioskSceneChange();
                    }
                    break;
                case "Distroyer":
                    KioskSystem.single.RemoveNum();
                    break;
                default:
                    break;
            }
   

            // 상호작용 처리 후 필요한 작업 추가
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red, 0.5f);
        }
        //usingChek = false;
    }


}
