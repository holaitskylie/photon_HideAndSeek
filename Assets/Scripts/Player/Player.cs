using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviourPunCallbacks
{
    public float speed = 2.0f;
    private Animator animator;
    private Rigidbody rb;
    [SerializeField] PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            bool isMoving = UpdateMove();
        }
        
    }

    bool UpdateMove()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        bool isPressed = h > 0.01f || h < -0.01f || v > 0.01f || v < -0.1f;

        if (isPressed)
        {
            Vector3 dir = h * Vector3.right + v * Vector3.forward;

            transform.rotation = Quaternion.LookRotation(dir);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        return isPressed;
    }
}
