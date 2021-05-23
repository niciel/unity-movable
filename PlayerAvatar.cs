using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[RequireComponent(typeof(MovableObject))]

public class PlayerAvatar : MonoBehaviour
{



    private MovableObject Movable;
    private Inputs Input;
    private GameObject Head;

    // przesunięcie postaci ruch



    void Awake()
    {
        GameObject mainCore = GameObject.FindGameObjectWithTag("GameCore");

        this.Input = ManagerBase.GetManager<Inputs>();
        this.Head = transform.Find("head").gameObject;
        this.Movable = GetComponent<MovableObject>();
        if (Head == null)
        {
            Debug.LogError("brak glowy :D\n dodaj gameobject z nazwa 'head'");
        }
    }

    void Start()
    {

    }
    void LateUpdate()
    {

    }



    void Update()
    {
        LookUpdate();

        MoveUpdate();

    }





    [HeaderAttribute(header: "x- forward, y- back, z- sideWays")]
    public Vector3 NormalMaxVelocity;

    [HeaderAttribute(header: "x- forward, y- back, z- sideWays")]
    public Vector3 SprintingMaxVelocity;

    private float yaw = 0;
    private float pitch = 0;


    public float maxPitch = 80;
    public float minPitch = -80;

    public float MouseSensivity = 1;





    private Vector2 deltaMove;
    private Quaternion rotation;





    private void MoveUpdate()
    {
        deltaMove = this.Input.MoveDirection;

        if (deltaMove != Vector2.zero)
        {
            Vector3 maxVelocity;
            // TODO przypisanie
            if (this.Input.Shift)
                maxVelocity = SprintingMaxVelocity;
            else
                maxVelocity = NormalMaxVelocity;


            float maxSpeed;
            if (deltaMove.y > 0)
                maxSpeed = maxVelocity.x;
            else if (deltaMove.y < 0)
                maxSpeed = maxVelocity.y;
            else
                maxSpeed = maxVelocity.z;
            Vector3 directionalVector = new Vector3(deltaMove.x, 0, deltaMove.y);
            directionalVector = this.rotation * directionalVector;
            directionalVector = directionalVector.normalized * maxSpeed;
            this.Movable.CharacterMovement = directionalVector * Time.deltaTime;
        }
        else
            this.Movable.CharacterMovement = Vector3.zero;
    }

    private void LookUpdate()
    {
        if (GameLogic.IsPlayerLookRotationSuspended)
            return;
        Vector2 deltaMouse = this.Input.DeltaMousePosition;
        this.yaw += MouseSensivity * deltaMouse.x;
        if (yaw < 0)
            yaw += 360;
        else if (yaw > 360)
            yaw -= 360;
        this.pitch += MouseSensivity * -deltaMouse.y;
        if (this.pitch > maxPitch)
            this.pitch = maxPitch;
        else if (this.pitch < minPitch)
            this.pitch = minPitch;
        this.rotation = Quaternion.Euler(this.pitch, this.yaw, 0);
        this.Head.transform.rotation = this.rotation;
    }






}
