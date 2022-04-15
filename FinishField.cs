using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishField : MonoBehaviour
{
    public static FinishField Instance;
    private PlayerController playerController;

    private Rigidbody rigidBody;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject shadow;

    public bool isFinished = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        playerController = PlayerController.Instance;
        rigidBody = player.gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isFinished)
            {
                playerController.CurrentGameState = GameState.Dead;
                rigidBody.velocity = Vector3.zero;

                shadow.SetActive(false);

                player.transform.DOJump(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z), 7f, 1, 3f, false);
                player.transform.DORotate(new Vector3(-360, 0, 0), 2f, RotateMode.LocalAxisAdd);
                isFinished = false;
            }
        }
    }
}