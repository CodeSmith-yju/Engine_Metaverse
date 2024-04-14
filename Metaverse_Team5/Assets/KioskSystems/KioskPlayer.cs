using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KioskPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    public float interactionDistance = 4f; // ��ȣ�ۿ� ������ �Ÿ�
    //public LayerMask interactableLayer; // ��ȣ�ۿ� ������ Layer
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
        //interactableLayer = LayerMask.GetMask("Object");// Layer�� Object�϶��� RayCast�� �۵��ϵ��� ����
        //RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionDistance))
        {
            usingChek = true;
            GameObject interactedObject = hit.collider.gameObject;// ��ȣ�ۿ��� ������Ʈ�� ã���� �� ó��

            switch (interactedObject.name)//��ȣ�ۿ��� ������Ʈ�� �̸������� �ٸ� ��ȣ�ۿ��� �� �� �ְ� ��.
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
   

            // ��ȣ�ۿ� ó�� �� �ʿ��� �۾� �߰�
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red, 0.5f);
        }
        //usingChek = false;
    }


}
