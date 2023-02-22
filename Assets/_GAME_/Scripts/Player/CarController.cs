using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    [SerializeField] MeshRenderer carMesh;
    [SerializeField] BoxCollider carCollider;
    [SerializeField] GameObject tailHolder;
    [SerializeField] GameObject tailBall;
    [SerializeField] Transform bonusSphereHolder;
    [SerializeField] Animator driverAnimator;
    [SerializeField] TextMeshPro carNumberTxt;

    public List<Transform> wheelTransforms = new List<Transform>();
    public Transform BonusSphereHolder => bonusSphereHolder;



    Rigidbody carRB;

    public Animator DriverAnimator => driverAnimator;
    public Rigidbody CarRB => carRB;

    [HideInInspector]
    public Entity entity;
    GameEntries.CarData carData;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        carRB = GetComponent<Rigidbody>();

        //fixedJoint.connectedBody = LevelController.Instance.Player.GetComponent<Rigidbody>();

        carNumberTxt.text = "";
    }

    public void Init(GameEntries.CarData _carData, int paintID)
    {
        carData = _carData;

        PaintCar(_carData.carPaints[paintID]);
    }

    public void PaintCar(Material carMat)
    {
        carMesh.material = carMat;
    }

    public void SetCarNumber(int number)
    {
        carNumberTxt.text = number.ToString();
    }

    public void DisableMotion(bool mode)
    {
        carCollider.enabled = !mode;
        carRB.useGravity = !mode;
        carRB.isKinematic = mode;

        if (mode)
        {
            carRB.velocity = Vector3.zero;
            carRB.angularVelocity = Vector3.zero;
        }
    }

    public void SetTail(bool mode)
    {
        tailHolder.SetActive(mode);
    }

    public void UpgradeTailBall()
    {
        var current = tailBall.transform.localScale;
        tailBall.transform.DOScale(current * 1.25f, .2f).SetEase(Ease.OutBack);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            if (GameManager.Instance.GameState != GameEntries.GameState.InGame)
                return;

            carCollider.enabled = false;

            entity.Defeated();

            FXController.CreateEffect(GameUtils.GetEffectSettings().hitWaterFX, entity.CurrentCar.transform.position, null, .5f, 2);
        }

        //if (other.tag == "Car")
        //{
        //    var carController = other.GetComponent<CarController>();

        //    carController.entity.GetHit(entity,false);
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Car")
        {
            var carController = collision.collider.GetComponent<CarController>();

            FXController.CreateEffect(GameUtils.GetRandomGetHitFX(false), collision.contacts[0].point, null, .1f, 2);

            carController.entity.GetHit(entity, false,Vector3.zero);
        }
    }
}
