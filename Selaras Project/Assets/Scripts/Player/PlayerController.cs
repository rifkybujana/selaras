using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = new PlayerInput();

    [Space(10)] [Header("Player Movement and Controll")]

    [Tooltip("Kecepatan perubahan dari kecepatan player")]
    [Range(1, 20)] public float accelleration = 10;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float maxSpeed = 30;

    [Space(5)]
    [Header("Ground Check")]
    [SerializeField] private LayerMask GroundLayerMask;

    [SerializeField] private Vector2 GroundCheckScale = new Vector2(1, 1);
    [SerializeField] private Vector2 GroundCheckPos = new Vector2(0, 0);


    #region Private Variable

    private Rigidbody2D rb;

    private float speed;

    [HideInInspector] public float flow;

    [HideInInspector]
    public bool isGrounded()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask);
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        flow = 1;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale);
    }

    /// <summary>
    /// Mendapatkan Input dari player, dan mengendalikan player berdasarkan input
    /// </summary>
    private void GetInput()
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
            if(playerInput.buttonHoldTime < playerInput.buttonHoldMin)
            {
                speed = Mathf.Clamp(accel(),0, maxSpeed);
            }

            playerInput.buttonHoldTime = 0;

            playerInput.inputTimer = 0;
        }

        if(playerInput.inputTimer < playerInput.MaxInputTime)
        {
            rb.velocity = new Vector2(Mathf.Clamp(speed, 0, maxSpeed), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x - Time.deltaTime, 0, maxSpeed), rb.velocity.y);

            speed = rb.velocity.x;
        }

        playerInput.inputTimer += Time.deltaTime;
    }

    private float accel(float a = 1)
    {
        return (rb.velocity.x + (a * accelleration * Time.deltaTime)) * flow;
    }

    private float brake(float a = 5)
    {
        return rb.velocity.x - Time.deltaTime * a;
    }
}
