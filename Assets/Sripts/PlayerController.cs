using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameObject playerBody;
    public float speedPlayerMotion;
    public float rotSens;
    private float motX, motY;

    [HideInInspector]
    public bool moveActive = true;

    public Animator playerAnimator;
    private bool animInMove = false;

    private Vector3 motionDirection;
    private Vector3 mousePosition;

    static public PlayerController instance;

    private void Start()
    {
        if(PlayerController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        PlayerController.instance = this;
    }

    void Update()
    {
        float dirX = 0;
        float dirY = 0;

        if (Input.GetMouseButtonDown(0))
        {
            motX = Input.mousePosition.x;
            motY = Input.mousePosition.y;

            mousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            dirX = -(motX - Input.mousePosition.x) * 1000 / Screen.width;
            dirY = -(motY - Input.mousePosition.y) * 1000 / Screen.height;
        }

        if (Input.GetMouseButtonUp(0))
        {
            motX = 0;
            motY = 0;
        }

        motionDirection = new Vector3(dirX, 0, dirY).normalized;
        if (moveActive)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 lookDirection = new Vector3(dirX * rotSens, playerBody.transform.position.y, dirY * rotSens);

                if (Input.mousePosition != mousePosition)
                {
                    playerBody.transform.DOLookAt(lookDirection, 0.5f);

                    if (motX != 0 || motY != 0)
                        if(!animInMove)
                        {
                            animInMove = true;
                            playerAnimator.SetTrigger("Move");
                        }
                }
            }
            if (motX == 0 || motY == 0)
            {
                if(animInMove)
                {
                    animInMove = false;
                    playerAnimator.SetTrigger("Stay");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Catched();
    }
    private void FixedUpdate()
    {
        if(moveActive)
        {
            playerRb.velocity = motionDirection * speedPlayerMotion * Time.fixedDeltaTime;
        }
    }

    public void Catched()
    {
        playerAnimator.SetTrigger("Catched");
        moveActive = false;
        playerRb.velocity = Vector3.zero;
    }

    public void Finish()
    {
        moveActive = false;
        playerRb.velocity = Vector3.zero;
        playerAnimator.SetTrigger("Move");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent<PointToPoint>(out PointToPoint pointToPoint))
            {
                pointToPoint.StopGame();
                pointToPoint.MoveToPlayer(collision.gameObject.transform.position, transform.position);
            }
            else if(collision.gameObject.TryGetComponent<OnPlace>(out OnPlace onPlace))
            {
                onPlace.StopGame();
                onPlace.MoveToPlayer(collision.gameObject.transform.position, transform.position);
            }
            else if(collision.gameObject.transform.parent.TryGetComponent<OneByOne>(out OneByOne oneByOne))
            {
                oneByOne.StopGame(collision.gameObject);
                oneByOne.MoveToPlayer(collision.gameObject.transform.position, transform.position, collision.gameObject);
            }
        }
    }
}
