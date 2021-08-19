﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;

    Camera playerCamera;
    GameObject LeftDoor, RightDoor;
    GameObject elevatorUI;
    float h, v, h1, v1;
    float speed = 2.0f; 
    // Start is called before the first frame update

    void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = this.GetComponentInChildren<Camera>();
        // Cursor visible
        LeftDoor = GameObject.Find("Door_4");
        RightDoor = GameObject.Find("Door_3");
        elevatorUI = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        Cursor.visible = false;
        playerCamera.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetKey(KeyCode.Space))
                speed = 4.0f;
            else
                speed = 2.0f;

            h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            v = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            h1 = Input.GetAxis("Mouse X") * 2.0f;
            v1 = Input.GetAxis("Mouse Y") * 2.0f;

            transform.Rotate(0, h1, 0);

            transform.Translate(-h, 0, -v);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            elevatorUI.SetActive(true);
            StartCoroutine("Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            elevatorUI.SetActive(false);
            StartCoroutine("Close");
        }
    }

    IEnumerator Open()
    {
        for (float f = 2f; f >= 0; f -= 0.01f)
        {
            LeftDoor.transform.Translate(0.01f, 0, 0);
            RightDoor.transform.Translate(-0.01f, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Close()
    {
        for (float f = 2f; f >= 0; f -= 0.01f)
        {
            LeftDoor.transform.Translate(-0.01f, 0, 0);
            RightDoor.transform.Translate(0.01f, 0, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
