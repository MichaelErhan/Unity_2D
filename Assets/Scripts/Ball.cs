using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public float startSpeed = 40f;

    private Transform _arrow;

    private bool _ballMoving;

    private Transform _startPosition;

    private List<GameObject> _pins = new();

    private readonly Dictionary<GameObject, Transform> _pinsDefaultTransform = new();

    public int Point { get; set; }

    [SerializeField] private Animator cameraAnim;

    private TextMeshProUGUI feedBack;


    private void Start()
    {
        Application.targetFrameRate = 60;

        _arrow = GameObject.FindGameObjectWithTag("Arrow").transform;

        rb = GetComponent<Rigidbody>();

        _startPosition = transform;

        _pins = GameObject.FindGameObjectsWithTag("Pin").ToList();

        foreach (var pin in _pins)
        {
            _pinsDefaultTransform.Add(pin, pin.transform);
        }

        feedBack = GameObject.FindGameObjectWithTag("FeedBack").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_ballMoving)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Shoot());
        }

    }

    private IEnumerator Shoot()
    {
        cameraAnim.SetTrigger("Go");
        cameraAnim.SetFloat("CameraSpeed", _arrow.transform.localScale.z);
        _ballMoving = true;
        _arrow.gameObject.SetActive(false);
        rb.isKinematic = false;

        Vector3 forceVector = _arrow.forward * (startSpeed * _arrow.transform.localScale.z);

        Vector3 forcePosition = transform.position + (transform.right * 0.5f);

        rb.AddForceAtPosition(forceVector, forcePosition, ForceMode.Impulse);


        yield return new WaitForSecondsRealtime(7);

        _ballMoving = false;

        GenerateFeedBack();

        yield return new WaitForSecondsRealtime(4);

        ResetGame();
    }


    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GenerateFeedBack()
    {
        feedBack.text = Point switch
        {
            0 => "Бывает :(",
            > 0 and < 3 => "Для новичка неплохо!",
            >= 3 and < 6 => "Это было близко!",
            >= 6 and < 10 => "Отлично!",
            _ => "Превосходно!"
        };

        feedBack.GetComponent<Animator>().SetTrigger("Show");
    }
}
