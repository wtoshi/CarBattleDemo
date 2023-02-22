using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle" || other.tag == "Ground")
        {
            Destroy(other.gameObject);

            Debug.Log("Destroyed Object: "+other.gameObject.name );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Obstacle" || collision.collider.tag == "Ground")
        {
            Destroy(collision.gameObject);

            Debug.Log("Destroyed Object: " + collision.gameObject.name);
        }
    }
}
