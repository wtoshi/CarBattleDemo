using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolderFollower : MonoBehaviour
{
    Transform currentTarget;

    float initialY;

    private void Start()
    {
        initialY = transform.localPosition.y;    
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            SetTarget();
        }

        if (currentTarget == null)
            return;

        var pos = transform.position;
        pos.x = currentTarget.position.x;
        pos.y = currentTarget.position.y + initialY;
        pos.z = currentTarget.position.z;
        transform.position = pos;
    }


    void SetTarget()
    {
        var entity = GetComponentInParent<Entity>();
        currentTarget = entity.CurrentCar.transform;
    }
}
