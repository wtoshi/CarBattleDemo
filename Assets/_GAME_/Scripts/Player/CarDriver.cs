using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class CarDriver : MonoBehaviour
{
    public CarController currentCar;


    public float MoveSpeed = 50;
    public float MaxSpeed = 15;
    public float Drag = 0.98f;
    public float SteerAngle = 20f;

    public float Traction = 10;

    Vector3 MoveForce;

    protected float Vertical;
    protected float Horizontal;

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        DrawDebugRay();

        if (currentCar == null)
            return;

        DriveCar();
    }

    public virtual void DriveCar()
    {
        // Moving
        MoveForce += currentCar.transform.forward * MoveSpeed * Vertical * Time.deltaTime;
        currentCar.transform.position += MoveForce * Time.deltaTime;

        // Steering
        float steerInput = Horizontal;
        currentCar.transform.Rotate(Vector3.up * steerInput * MoveForce.magnitude * SteerAngle * Time.deltaTime);

        // Drag
        MoveForce *= Drag;
        MoveForce = Vector3.ClampMagnitude(MoveForce, MaxSpeed);

        // Traction
        Debug.DrawRay(currentCar.transform.position, MoveForce.normalized * 3);
        Debug.DrawRay(currentCar.transform.position, currentCar.transform.forward * 3, Color.blue);
        MoveForce = Vector3.Lerp(MoveForce.normalized, currentCar.transform.forward, Traction * Time.deltaTime) * MoveForce.magnitude;
    }

    #region GROUNDING

    float checkingDistance = .02f;

    public bool CheckIsGrounded()
    {
        if (currentCar == null)
            return false;

        List<bool> hitCount = new List<bool>();

        foreach (var wheel in currentCar.wheelTransforms)
        {
            Ray ray = new Ray(wheel.position, -wheel.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hitCount.Add(hit.collider.tag == "Ground");
            }
        }

        var groundHitCount = hitCount.Select(x => x == true).Count();

        return groundHitCount >= 3;
    }

    void DrawDebugRay()
    {
        if (currentCar == null)
            return;

        foreach (var wheel in currentCar.wheelTransforms)
        {
            Debug.DrawRay(wheel.position, -wheel.up* checkingDistance, Color.red);
        }
    }
    #endregion
}
