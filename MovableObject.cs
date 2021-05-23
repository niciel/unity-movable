using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class MovableObject : MonoBehaviour
{
    private CharacterController character;
    private Vector3 _CharacterMovement;
    private Vector3 _Velocity;
    private Vector3 _PreviusFrameVelocity;
    public Vector3 _ConstantForce;

    public bool FrazePlayerMovement;

    [SerializeField]
    private bool _OnGround;

    [SerializeField]
    public float TouchDistance = 0.004f;
    [Tooltip("do tego k¹ta pochylu terenu nie jest uwzglêdniane zmniejszenie prêdkoœci gracza")]
    [SerializeField]
    public float MinSurfaceSlope = 45;


    [SerializeField]
    [Tooltip("modyfikator prêdkoœci gracza od nachy³u terenu 0 = przechyl 'MinSurfaceSlope' 1 = z charactercontroler slopelimit")]
    public AnimationCurve PlayerMovementDecresOnAngle;


    void Awake()
    {
        this._PreviusFrameVelocity = Vector3.zero;
        this.character = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        FrazePlayerMovement = false;
        CheckGrounded();
        ApplayForces();
    }

    void LateUpdate()
    {
        
    }

    public void ApplayForces()
    {
        Vector3 move = Vector3.zero;
        if (this.FrazePlayerMovement == false)
        {
            move += this._CharacterMovement;
        }
        move += _Velocity + _ConstantForce;
        this.character.Move(move);
        this._PreviusFrameVelocity = this._Velocity;
        this._Velocity = Vector3.zero;
        this._CharacterMovement = Vector3.zero;
    }

    private float timeSinceStartFalling = 0;
    private bool lastFrameFalling = false;


    protected void CheckGrounded()
    {
        Vector3 pos = this.character.center + this.transform.position;
        float radius = this.character.radius;
        float height = this.character.height / 2 - radius;//+ this.character.skinWidth
        pos.y -= height;
        DebugExtension.DebugPoint(pos, Color.red);
        //        RaycastHit[] collids = Physics.SphereCastAll(pos, radius , Vector3.down , 2*this.character.skinWidth);
        RaycastHit[] collids = Physics.SphereCastAll(pos, radius, Vector3.down, this.character.stepOffset);
        DebugExtension.DebugWireSphere(pos, radius);
        this._OnGround = false;

        List<SomeInfo> touching = new List<SomeInfo>();
        float fixedBadTouch = radius + TouchDistance;


        float angle;
        float distance;
        bool touched;
        foreach (RaycastHit c in collids)
        {
            if (c.transform.gameObject.GetInstanceID() == this.character.gameObject.GetInstanceID())
            {
                continue;
            }
            DebugExtension.DebugPoint(c.point, scale: 0.25f, color: Color.red);
            //Debug.DrawLine(c.point, c.point + c.normal * 3 , Color.red , 0.1f);
            angle = Mathf.Asin(c.normal.y);
            if (float.IsNaN(angle))
                angle = 0;
            else
                angle = Mathf.PI / 2 - angle;
            distance = (pos - c.point).magnitude;

            if (distance <= fixedBadTouch)
            {
                touched = true;
                this._OnGround = true;
            }
            else
                touched = false;

            touching.Add(new SomeInfo()
            {
                hit = c,
                angle = angle,
                distance = distance,
                isTouching = touched
            });

        }
        // korekcja predkosci gracza wzgledem pochylenia teren / poziom do pochy³u
        float RadMinSurfaceSlope = Mathf.Deg2Rad * this.MinSurfaceSlope;
        float RadMaxSurfaceSlope = Mathf.Deg2Rad * this.character.slopeLimit;
        float maxRad = Mathf.PI / 2;
        bool flag = false;
        float deltaAngle = 90;
        float ang;

        //spadek / slizganie sie
        Vector3 slopeDirection = Vector3.zero;
        bool touchingGroundFlag = false;
        foreach (SomeInfo info in touching)
        {
            // korekcja predkosci gracza wzgledem pochylenia terenu
            //Debug.Log("angle " + info.angle * Mathf.Rad2Deg + " max " + RadMaxSurfaceSlope*Mathf.Rad2Deg);
            if (info.isTouching)
            {
                if (info.angle >= RadMinSurfaceSlope && info.angle <= RadMaxSurfaceSlope)
                {
                    DebugExtension.DebugArrow(info.hit.point, info.hit.normal, Color.blue);
                    if (maxRad > info.angle)
                    {
                        Vector3 v = _CharacterMovement;
                        v.y = 0;
                        v.Normalize();
                        Vector3 dif = info.hit.normal;
                        dif.y = 0;
                        dif.Normalize();
                        ang = 180 - Vector3.Angle(v, dif);

                        if (ang <= deltaAngle)
                        {
                            maxRad = info.angle;
                            flag = true;
                            deltaAngle = ang;
                        }
                    }
                }
            }
            //spadek / slizganie sie

            if (info.angle >= RadMaxSurfaceSlope)
            {
                Vector3 vec = info.hit.normal;
                vec.y = -vec.y;
                slopeDirection += vec;
                slopeDirection.Normalize();
            }
            else
                touchingGroundFlag = true;
        }

        if (!touchingGroundFlag)
        {
            if (lastFrameFalling == false)
            {
                this.timeSinceStartFalling = Time.realtimeSinceStartup;
                lastFrameFalling = true;
            }
            DebugExtension.DebugArrow(this.gameObject.transform.position, slopeDirection, Color.blue, duration: 1);
            this._Velocity += slopeDirection * Time.deltaTime * 10 * (Time.realtimeSinceStartup - this.timeSinceStartFalling);
            this._Velocity += Vector3.down * 10 * Time.deltaTime;
            Debug.Log("velocity " + this._Velocity.magnitude);
            //TODO
        }
        else
        {
            this.lastFrameFalling = false;

        }
        // korekcja predkosci gracza wzgledem pochylenia terenu
        if (flag)
        {
            float delta = (maxRad - RadMinSurfaceSlope) / (RadMaxSurfaceSlope - RadMinSurfaceSlope);
            //Debug.Log("pierwsze zmniejszenie " + (1 - delta));
            if (deltaAngle < 90)
            {
                //Debug.Log("k¹towe zmniejszenie " + deltaAngle + " zmniejszenie " + (1 - Mathf.Sin(Mathf.Deg2Rad * deltaAngle
                delta *= (1 - Mathf.Sin(Mathf.Deg2Rad * deltaAngle));
            }
            delta = 1 - delta;

            //Debug.Log("zmniejszenie predkosci o " + delta);
            this._CharacterMovement *= this.PlayerMovementDecresOnAngle.Evaluate(delta);
        }
        // END korekcja predkosci gracza wzgledem pochylenia terenu END



    }
    public struct SomeInfo
    {
        public RaycastHit hit;
        //dystans od srodka okregu kapsuly do punktu zderzenia
        public float distance;
        public bool isTouching;
        //in radians
        public float angle;
    }



    public bool OnGround
    {
        get { return this._OnGround; }
    }

    public Vector3 CharacterMovement
    {
        get { return this._CharacterMovement; }
        set { this._CharacterMovement = value; }
    }
    public Vector3 Velocity
    {
        get { return this._Velocity; }
        set { this._Velocity = value; }
    }
    public Vector3 PreviusFrameVelocity
    {
        get { return this._PreviusFrameVelocity; }
        set { this._PreviusFrameVelocity = value; }
    }

}
