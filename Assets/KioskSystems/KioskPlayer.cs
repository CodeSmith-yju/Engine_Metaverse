using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KioskPlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDerection = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(moveDerection * moveSpeed * Time.deltaTime);

    }

}
