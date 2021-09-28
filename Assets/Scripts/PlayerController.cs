using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isAlive;

    [Range(0, 100)]
    public float angularSpeed = 50.0f; // degree per frame

    [Range(0, 10)]
    public float speed = 5.0f; // 5 m/s


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //Vector3 moveDir=way

        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(moveDir), 5 * Time.deltaTime);
        this.transform.localEulerAngles += new Vector3(0, h*angularSpeed*Time.deltaTime, 0);

        this.transform.position += new Vector3(0, 0, v * speed * Time.deltaTime);
    }
}
