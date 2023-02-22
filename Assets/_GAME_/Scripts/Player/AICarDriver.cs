using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarDriver : CarDriver
{
    Entity AIEntity;

    Entity currentTargetEntity;

    float minDistanceToHit = .15f;

    private void Awake()
    {
        AIEntity = GetComponent<Entity>();
    }

    public override void DriveCar()
    {
        if (AIEntity == null || AIEntity.entityStatus == Entity.EntityStatus.Defeated || GameManager.Instance.GameState != GameEntries.GameState.InGame )
        {
            Vertical = 0;
            Horizontal = 0;
            return;
        }
            
        if (!CheckIfTargetAvailable())
            GetTarget();

        if (currentTargetEntity != null)
        {
            float targetDistance = GetTargetDistance();

            if (targetDistance > minDistanceToHit)
            {
                DriveToTarget();
            }
            else
            {
                // Target hedef içinde
                Vertical = 1;

                var random = Random.Range(0,2);
                Horizontal = random == 0 ? 1 : -1;
            }

        }

        //Vertical = 0;
        //Horizontal = 0;

        base.DriveCar();
    }

    void GetTarget()
    {
        var entities = FindObjectsOfType<Entity>();

        List<Entity> targetEntities = new List<Entity>();
        foreach (var entity in entities)
        {
            if (AIEntity != null && entity == AIEntity) continue;
            if (entity.entityStatus == Entity.EntityStatus.Defeated) continue;
            //if (!entity.isGrounded) continue;
            

            targetEntities.Add(entity);
        }

        if (targetEntities.Count > 0)
        {
            currentTargetEntity = targetEntities[Random.Range(0, targetEntities.Count)];
        }
        
    }

    bool CheckIfTargetAvailable()
    {
        if (currentTargetEntity == null)
            return false;

        if (currentTargetEntity.entityStatus == Entity.EntityStatus.Defeated) return false;
        //if (!currentTargetEntity.isGrounded) return false;

        return true;
    }

    float GetTargetDistance()
    {
        var dist = Vector3.Distance(AIEntity.CurrentCar.transform.position, currentTargetEntity.transform.position);
        return dist;
    }

    void DriveToTarget()
    {
        Vector3 moveDir = (currentTargetEntity.transform.position - AIEntity.CurrentCar.transform.position).normalized;
        float dot = Vector3.Dot(AIEntity.CurrentCar.transform.forward, moveDir);

        if (dot > 0)    // Pozitif ise önünde, negatif arkasýnda
        {
            Vertical = 1f;
        }
        else
        {
            Vertical = 1f;
            //Horizontal = 1f;
        }

        var angleToDir = Vector3.SignedAngle(AIEntity.CurrentCar.transform.forward, moveDir, Vector3.up);

        if (angleToDir > 0)     // Pozitif ise saðýnda kalýyor, negatif ise solunda
        {
            Horizontal = 1;
        }
        else
        {
            Horizontal = -1;
        }
    }
}
