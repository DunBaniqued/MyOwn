using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour, IKillable
{
    [SerializeField] Transform spawn = null;

    public void Killed()
    {
        if (spawn != null) this.transform.position = spawn.position;
    }
}
