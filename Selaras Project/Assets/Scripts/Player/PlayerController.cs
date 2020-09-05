using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = new PlayerInput();

    [Space(10)] [Header("Player Movement and Controll")]

    [Tooltip("Kecepatan perubahan dari kecepatan player")]
    [Range(1, 20)] [SerializeField] private float accelleration = 10;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float maxSpeed = 30;

    #region Private Variable

    private Rigidbody2D rb;

    private float speed;

    [HideInInspector] public float flow;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        flow = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProceduralGenerator pg = collision.gameObject.GetComponent<ProceduralGenerator>();

        if (pg == null) return;

        if (pg.meshType == ProceduralGenerator.MeshType.StreamDown)
        {
            flow = 10;
        }
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
                speed = Mathf.Clamp(brake(3), flow, maxSpeed);
            }

            playerInput.buttonHoldTime += Time.deltaTime;
        }

        //jika player berhenti menekan mouse kiri
        if (Input.GetMouseButtonUp(0))
        {
            if(playerInput.buttonHoldTime < playerInput.buttonHoldMin)
            {
                speed = Mathf.Clamp(accel(2), flow, maxSpeed);
            }

            playerInput.buttonHoldTime = 0;
        }

        rb.velocity = new Vector2(Mathf.Clamp(speed, flow, maxSpeed), rb.velocity.y);
    }

    private float accel(float a = 1)
    {
        return rb.velocity.x + (a * accelleration * Time.deltaTime);
    }

    private float brake(float a = 1)
    {
        return rb.velocity.x - (a * accelleration * Time.deltaTime);
    }
}
