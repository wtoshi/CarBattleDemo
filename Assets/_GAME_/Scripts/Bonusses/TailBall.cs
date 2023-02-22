using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailBall : MonoBehaviour
{
    Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (entity == null)
            return;
        
        //Debug.Log("Hit detect Collision col: " + collision.collider.name);
        if (collision.collider.CompareTag("Car"))
        {
            
            var carController = collision.collider.GetComponent<CarController>();

            if (carController.entity == entity)
                return;

            FXController.CreateEffect(GameUtils.GetRandomGetHitFX(true), collision.contacts[0].point, null, .1f, 2);

            carController.entity.GetHit(entity, true, collision.GetContact(0).normal);
        }
    }
}
