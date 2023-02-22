using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSphere : MonoBehaviour
{

    public float RotateSpeed = 5f;
    public float Radius;

    private float angle;

    float timeCounter = 0;
    bool started = false;
    Entity entity;

    private void Start()
    {
        
    }

    private void Update()
    {
 
        if (!started)
            return;

        timeCounter += Time.deltaTime;

        if (timeCounter >= duration)
        {
            entity.ResetBonus();
            Destroy(gameObject);
        }

        angle += RotateSpeed * Time.deltaTime;

        transform.position = entity.CurrentCar.transform.position + (new Vector3(Mathf.Cos(angle), .2f, Mathf.Sin(angle))) * Radius;
    }


    float duration = 0;
    public void Set(Entity _entity, float _duration)
    {
        entity = _entity;
        duration = _duration;

        started = true;
    }


    //private void OnTriggerEnter(Collider col)
    //{
    //    //Debug.Log("Hit detect TRIGGER col: "+ col.name);

    //    if (col.CompareTag("Car"))
    //    {

    //        var carController = col.GetComponent<CarController>();

    //        if (carController.entity == entity)
    //            return;

    //        //FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

    //        carController.entity.GetHit(entity, true, Vector3.one);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit detect Collision col: " + collision.collider.name);
        if (collision.collider.CompareTag("Car"))
        {

            var carController = collision.collider.GetComponent<CarController>();

            if (carController.entity == entity)
                return;

            //FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

            carController.entity.GetHit(entity, true, collision.GetContact(0).normal);
        }
    }
}
