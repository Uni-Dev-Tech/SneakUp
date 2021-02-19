using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnPlace : MonoBehaviour
{
    public GameObject objectToMove;
    public Animator animator;
    private bool watchOver = true;
    public ParticleSystem angry;

    private void Update()
    {
        if (watchOver)
        {
            LineOfSight();
        }
    }

    private void LineOfSight()
    {
        RaycastHit[] raycastHits = new RaycastHit[10];
        Ray[] rays = new Ray[10];
        float rayDirection = -0.5f;

        for (int i = 0; i < rays.Length; i++)
        {
            rays[i] = new Ray(objectToMove.transform.position, objectToMove.transform.forward + (objectToMove.transform.right * rayDirection));
            rayDirection += 0.1f;

            if (Physics.Raycast(rays[i], out raycastHits[i], 5f))
            {
                if (raycastHits[i].transform.CompareTag("Player"))
                {
                    StopGame();
                    MoveToPlayer(objectToMove.transform.position, raycastHits[i].transform.position);
                }
            }
        }
    }

    public void StopGame()
    {
        PlayerController.instance.Catched();
        watchOver = false;
        objectToMove.transform.DOKill();
    }

    public void MoveToPlayer(Vector3 policeManPosition, Vector3 playerPosition)
    {
        animator.SetTrigger("Catched");
        Vector3 movePoint = policeManPosition + ((playerPosition - policeManPosition) / 2);
        angry.Play();
        objectToMove.transform.DOMove(movePoint, 1f).OnComplete(Arrested);
        objectToMove.transform.DOLookAt(playerPosition, 1f);
    }

    private void Arrested()
    {
        animator.SetTrigger("Arrested");
        UIWinLose.instance.Lose();
    }
}
