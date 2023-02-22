using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject parachute;
    [SerializeField] MeshRenderer parachuteMeshRenderer;
    [SerializeField] GameObject landedFX;

    bool landed = false;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.tag == "Ground")
    //    {
    //        anim.enabled = false;

    //        FXController.CreateEffect(GameUtils.GetEffectSettings().perkLandedFx, collision.contacts[0].point, null, .02f, 2);

    //        parachute.AddComponent<Cloth>();

    //        Master.Instance.WaitAndDo(.5f, ()=> {
    //            Destroy(parachute);

    //            landedFX.SetActive(true);

    //            landed = true;
    //        });
    //    }

    //    if (collision.collider.tag == "Car")
    //    {
    //        if (!landed)
    //            return;

    //        var carController = collision.collider.GetComponent<CarController>();

    //        FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

    //        carController.entity.InitBonus();
            
    //    }

    //}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Ground")
        {
            anim.enabled = false;

            FXController.CreateEffect(GameUtils.GetEffectSettings().perkLandedFx, transform.position, null, .02f, 2);

            parachuteMeshRenderer.enabled = false;
            parachute.AddComponent<Cloth>();

            Master.Instance.WaitAndDo(.25f, () => {

                Destroy(parachute);

                landedFX.SetActive(true);

                landed = true;
            });
        }

        if (col.tag == "Car")
        {
            if (!landed)
                return;

            var carController = col.GetComponent<CarController>();

            //FXController.CreateEffect(GameUtils.GetEffectSettings().perkPickUPFx, collision.contacts[0].point, null, .1f, 2);

            carController.entity.InitBonus();

            Destroy(gameObject);

        }

    }
}
