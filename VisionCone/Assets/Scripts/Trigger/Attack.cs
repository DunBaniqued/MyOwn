using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public List<GameObject> targets;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy") targets.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy") targets.Remove(other.gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E) && targets.Count > 0)
        {
            IKillable handler = targets[0].GetComponent<IKillable>();
            if (handler != null)
            {
               targets.RemoveAt(0);
               handler.Killed();
            }
        }
    }
}
