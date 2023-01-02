using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;

    public float pickUpDistance;
    public float forceMulti;

    public bool readyToThrow;
    public bool itemIsPicked;

    private Rigidbody rb;
    public HingeJoint tempHinge;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        PickUpPoint = GameObject.Find("PickUpPoint").transform;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.E) && itemIsPicked == true && readyToThrow)
        {
            forceMulti += 300 * Time.deltaTime;
        }

        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if(pickUpDistance <= 2)
        {
            if(Input.GetKeyDown(KeyCode.E) && itemIsPicked == false && PickUpPoint.childCount < 1)
            {
                Debug.Log("ObjectPicked");
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = PickUpPoint.position;
                this.transform.parent = GameObject.Find("PickUpPoint").transform;
                tempHinge = GameObject.Find("Player").AddComponent<HingeJoint>();
                tempHinge.connectedBody = GetComponent<Rigidbody>();

                itemIsPicked = true;
                forceMulti = 0;
            }
        }

        if(Input.GetKeyUp(KeyCode.E) && itemIsPicked == true)
        {
            readyToThrow = true;

            if (forceMulti > 10)
            {
                Destroy(player.GetComponent<HingeJoint>());
                rb.AddForce(player.transform.forward * forceMulti);
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                this.transform.parent = null;
                itemIsPicked = false;

                forceMulti = 0;
                readyToThrow = false;
            }

            forceMulti = 0;
        }
    }
}
