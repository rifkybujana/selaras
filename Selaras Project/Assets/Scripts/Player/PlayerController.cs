using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public GameData.Char character;
    [HideInInspector] public GameData.Boat boat;

    public GameObject[] boats;

    public Animator[] anim;

    [Space(5)]
    public ParticleSystem splash;
    public Transform splashPos;

    private bool isAccel;
    private bool isBrake;
    private float accelTime;

    [SerializeField] private GameManager manager = null;

    [Space(10)]

    [SerializeField] private PlayerInput playerInput = new PlayerInput();

    [Space(10)] 
    [Header("Player Movement and Controll")]

    [Tooltip("Kecepatan perubahan dari kecepatan player")]
    [Range(1, 20)] public float accelleration = 10;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float speedThreshold = 30;

    [Space(5)]
    [Header("Ground Check")]

    [SerializeField] private LayerMask GroundLayerMask = new LayerMask();

    [SerializeField] private Vector2 GroundCheckScale = new Vector2(1, 1);
    [SerializeField] private Vector2 GroundCheckPos = new Vector2(0, 0);


    #region Private Variable

    private Rigidbody2D rb;

    public Collider2D Ground() => Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask);

    [HideInInspector]
    public bool isGrounded() => Ground();

    [HideInInspector]
    public Transform GroundPos() => Ground().GetComponent<Transform>();

    [HideInInspector]
    public BuoyancyEffector2D GroundBuoyancy() => Ground().GetComponent<BuoyancyEffector2D>();

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim[character.Index].gameObject.SetActive(true);
        boats[boat.Index].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (manager.isDeath || manager.uiPos == GameManager.UIPos.Pause || !manager.isStart) return;

        CheckDeath();

        if (isGrounded())
        {
            //Jika player menekan mouse kiri
            if (Input.GetMouseButton(0))
            {
                if (playerInput.buttonHoldTime >= playerInput.buttonHoldMin && rb.velocity.x > 0.5f)
                {
                    rb.velocity = new Vector2(rb.velocity.x - (Time.deltaTime * speedThreshold / 2), rb.velocity.y);
                    isBrake = true;
                }

                playerInput.buttonHoldTime += Time.deltaTime;
            }

            //jika player berhenti menekan mouse kiri
            if (Input.GetMouseButtonUp(0))
            {
                if (playerInput.buttonHoldTime < playerInput.buttonHoldMin)
                {
                    rb.AddForce(Vector2.right * accelleration );
                    accelTime = 0.5f;
                }

                playerInput.buttonHoldTime = 0;
                isBrake = false;
            }

            isAccel = accelTime > 0;

            if (isAccel) 
            { 
                accelTime -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                rb.angularDrag -= Time.deltaTime;
            }
        }

        if (rb.angularDrag < 0.05f)
        {
            rb.angularDrag += Time.deltaTime / 2;
        }
        anim[character.Index].SetBool("isAccelerating", isAccel);
        anim[character.Index].SetBool("isBrake", isBrake);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.velocity.y < -speedThreshold)
        {
            manager.isDeath = true;
        }

        if(rb.velocity.y < -speedThreshold / 2)
        {
            Instantiate(splash.gameObject, splashPos.position, Quaternion.identity);

            manager.audioManager.PlaySound("Hit");
            manager.Shake();
        }

        manager.pGen = collision.gameObject.GetComponent<ProceduralGenerator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.y < -speedThreshold)
        {
            manager.isDeath = true;
        }

        if (rb.velocity.y < -speedThreshold / 2)
        {
            Instantiate(splash.gameObject, splashPos.position, Quaternion.identity);

            manager.audioManager.PlaySound("Hit");
            manager.Shake();
        }

        manager.pGen = collision.gameObject.GetComponent<ProceduralGenerator>();
    }

    private void CheckDeath()
    {
        if ((isGrounded() && transform.rotation.eulerAngles.z > 60 && transform.rotation.eulerAngles.z < 300))
        {
            manager.isDeath = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale);
    }
}
