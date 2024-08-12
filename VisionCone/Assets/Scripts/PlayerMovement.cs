using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IKillable
{

    [SerializeField] float MoveSpeed = 10f;
    //[SerializeField] Transform spawn = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W)) this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            if (Input.GetKey(KeyCode.A)) this.transform.Translate(Vector3.left * Time.deltaTime * MoveSpeed);
            if (Input.GetKey(KeyCode.S)) this.transform.Translate(Vector3.back * Time.deltaTime * MoveSpeed);
            if (Input.GetKey(KeyCode.D)) this.transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
        }
    }

    public void Killed()
    {
        //if(spawn != null) this.transform.position = spawn.position;
    }
}
