using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    [Space(10)]

    [SerializeField] private PlayerInput playerInput = new PlayerInput();

    [Space(10)] 
    [Header("Player Movement and Controll")]

    [Tooltip("Kecepatan perubahan dari kecepatan player")]
    [Range(1, 20)] public float accelleration = 10;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float maxSpeed = 30;

    [Space(5)]
    [Header("Ground Check")]

    [SerializeField] private LayerMask GroundLayerMask;
    [SerializeField] private LayerMask FlowLayerMask;

    [SerializeField] private Vector2 GroundCheckScale = new Vector2(1, 1);
    [SerializeField] private Vector2 GroundCheckPos = new Vector2(0, 0);


    #region Private Variable

    private Rigidbody2D rb;

    private float speed;

    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.Clamp((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min, out_min, out_max);
    }

    [HideInInspector]
    public bool isGrounded()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask);
    }

    private bool isDeath;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Jika player menekan mouse kiri
        if (Input.GetMouseButton(0))
        {
            if (playerInput.buttonHoldTime >= playerInput.buttonHoldMin)
            {
                speed = Mathf.Clamp(brake(), 0, maxSpeed);
            }

            playerInput.buttonHoldTime += Time.deltaTime;

            playerInput.inputTimer = 0;
        }

        //jika player berhenti menekan mouse kiri
        if (Input.GetMouseButtonUp(0))
        {
            if (playerInput.buttonHoldTime < playerInput.buttonHoldMin)
            {
                speed = accel();
            }

            playerInput.buttonHoldTime = 0;

            playerInput.inputTimer = 0;
        }

        bool isOnFlow = Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, FlowLayerMask);

        if (playerInput.inputTimer < playerInput.MaxInputTime && !isOnFlow)
        {
            rb.velocity = new Vector2(Mathf.Clamp(speed, -maxSpeed, maxSpeed), rb.velocity.y);
        }
        else
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + Time.deltaTime, -maxSpeed, 0), rb.velocity.y);
            }
            else if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime, 0, maxSpeed), rb.velocity.y);
            }

            speed = rb.velocity.x;
        }

        playerInput.inputTimer += Time.deltaTime;

        SetEffect();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.rotation.eulerAngles.z > 90)
        {
            //Death();
            Debug.Log(transform.rotation.eulerAngles.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale);
    }

    private void SetEffect()
    {
        manager.vCamera.m_Lens.OrthographicSize = Map(rb.velocity.x, -maxSpeed, maxSpeed, 6.3f, 9.3f);
    }


    public void Death()
    {
        isDeath = true;

        manager.depthOfField.focusDistance.value = 0;
    }
    
    private float accel(float a = 1)
    {
        return (rb.velocity.x + (a * accelleration * Time.deltaTime));
    }

    private float brake(float a = 5)
    {
        return rb.velocity.x - Time.deltaTime * a;
    }
}
