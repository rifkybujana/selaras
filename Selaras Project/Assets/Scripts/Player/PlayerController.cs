using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.Clamp((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min, out_min, out_max);
    }

    [HideInInspector]
    public bool isGrounded()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask);
    }

    [HideInInspector]
    public Transform GroundPos()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask).GetComponent<Transform>();
    }

    [HideInInspector]
    public BuoyancyEffector2D GroundBuoyancy()
    {
        return Physics2D.OverlapBox(transform.position - new Vector3(GroundCheckPos.x, -GroundCheckPos.y, 0), GroundCheckScale, 0, GroundLayerMask).GetComponent<BuoyancyEffector2D>();
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                if (playerInput.buttonHoldTime >= playerInput.buttonHoldMin)
                {
                    rb.velocity = new Vector2(rb.velocity.x - (Time.deltaTime * speedThreshold / 2), rb.velocity.y);
                }

                playerInput.buttonHoldTime += Time.deltaTime;
            }

            //jika player berhenti menekan mouse kiri
            if (Input.GetMouseButtonUp(0))
            {
                if (playerInput.buttonHoldTime < playerInput.buttonHoldMin)
                {
                    rb.AddForce(Vector2.right * accelleration );
                }

                playerInput.buttonHoldTime = 0;
            }
        }

        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0.5f, Mathf.Infinity), rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rb.velocity.y < -speedThreshold)
        {
            manager.isDeath = true;
        }

        manager.pGen = collision.gameObject.GetComponent<ProceduralGenerator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.velocity.y < -speedThreshold)
        {
            manager.isDeath = true;
        }

        manager.pGen = collision.gameObject.GetComponent<ProceduralGenerator>();
    }

    private void CheckDeath()
    {
        if ((isGrounded() && transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270))
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
