using DG.Tweening;
using UnityEngine;
public enum GameState
{
    Playing,
    Dead
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private FinishField finishField;
    private GroundController groundController;
    private GameState currentGameState;
    public GameState CurrentGameState { get => currentGameState; set => currentGameState = value; }

    [SerializeField] private Camera OrthoG;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject playerMirror;
    [SerializeField] private GameObject shadow;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int speed;

    private GameObject turnPoint;
    private Rigidbody rb;
    private RaycastHit hit;
    private Vector3 firstPosition;
    private Vector3 mousePosition;
    private Vector3 difference;
    private bool feverState;
    private float biggestY = 3f;
    private float smallestY = 0.1f;
    private float sensivity = 8f;
    private float feverDuration;
    private int combo = 0;

    private void Start()
    {
        finishField = FinishField.Instance;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        rb = GetComponent<Rigidbody>();
        groundController = GroundController.Instance;
    }
    private void FixedUpdate()
    {
        if (currentGameState == GameState.Playing)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, speed);
        }
    }
    
    private void Update()
    {
        if (finishField.isFinished)
        {
            if (Input.GetAxis("Mouse Y") > 0)   //Debug.Log("Move UP");
            {
                if (transform.localScale.y < biggestY)
                {
                    transform.localScale += new Vector3(-.03f, .03f, 0);
                    playerMirror.transform.localScale = transform.localScale;
                }
            }
            if (Input.GetAxis("Mouse Y") < 0)   //Debug.Log("Move DOWN");
            {
                if (transform.localScale.y > smallestY)
                {
                    transform.localScale += new Vector3(.03f, -.03f, 0);
                    playerMirror.transform.localScale = transform.localScale;
                }
            }
        }

        firstPosition = Vector3.Lerp(firstPosition, mousePosition, .1f);
        if (Input.GetMouseButtonDown(0))
            MouseDown(Input.mousePosition);

        else if (Input.GetMouseButtonUp(0))
            MouseUp();

        else if (Input.GetMouseButton(0))
            MouseHold(Input.mousePosition);


        if (Physics.Raycast(transform.position, transform.position + Vector3.forward, out hit, 111f, mask)) //Debug.Log(hit.distance);
        {
            shadow.transform.localScale = new Vector3(shadow.transform.localScale.x, shadow.transform.localScale.y, hit.distance * 2);
        }
        if (feverState)
        {
            feverDuration += Time.deltaTime;
            if (feverDuration >= 3)
            {
                combo = 0;
                feverDuration = 0f;
                speed = 10;
                groundController.DoorForDefaultColor();
                feverState = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)  //ÇARPMA DURUMU
    {
        if (collision.gameObject.CompareTag("barrier"))
        {
            collision.gameObject.AddComponent<Rigidbody>();

            if (combo >= 3)
                collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z + speed * 10 * combo));
            else
                collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z + speed));


            collision.transform.parent.GetChild(3).GetComponent<Collider>().enabled = false;
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject, 2f);
            combo = 0;
            if (!feverState)
                speed = 10;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("correct"))
        {
            CanvasController.Instance.ScoreCount();
            playerMirror.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + hit.distance);

            combo++;
            speed += 5;

            if (combo >= 5)
            {
                feverState = true;
                GroundController.Instance.DoorForGreenColor();
            }
        }
        if (other.gameObject.CompareTag("bridge fall"))
        {
            Debug.Log("Player köprüyü geçemedi.\nGame Over !!!");
        }
        if (other.gameObject.CompareTag("right turn"))
        {
            ground.transform.parent = other.transform;
            turnPoint = GameObject.FindWithTag("left turn");
            turnPoint.transform.parent = other.transform;

            other.transform.DORotate(new Vector3(0, -90, 0), 0.2f);
            Invoke("DoGroundNull", 1f);

        }
        if (other.gameObject.CompareTag("left turn"))
        {
            ground.transform.parent = other.transform;

            other.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
            Invoke("DoGroundNull", 1f);
        }
    }

    //End Of FunctionS.
    private void DoGroundNull()
    {
        ground.transform.parent = null;
        turnPoint.transform.parent = null;
    }
    private void MouseDown(Vector3 inputPosition)
    {
        mousePosition = OrthoG.ScreenToWorldPoint(inputPosition);
        firstPosition = mousePosition;
    }
    private void MouseHold(Vector3 inputPosition)
    {
        mousePosition = OrthoG.ScreenToWorldPoint(inputPosition);
        difference = mousePosition - firstPosition;
        difference *= sensivity;
    }
    private void MouseUp()
    {
        difference = Vector3.zero;
    }
}