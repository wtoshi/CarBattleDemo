using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailHitDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Car")
        {

            var carController = col.GetComponent<CarController>();

            //FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

            //carController.entity.GetHit();

            Debug.Log("Hit detect TRIGGER");

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Car")
        {

            var carController = collision.collider.GetComponent<CarController>();

            //FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

            Debug.Log("Hit detect COLLISION: impulse: "+collision.impulse +" veloc "+ collision.relativeVelocity);
            //carController.entity.GetHit();

        }
    }

}
