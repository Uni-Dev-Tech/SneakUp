using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Finish : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRb;
    public CapsuleCollider playerCapsuleCol;
    public Vector3[] pathToExit;

    public GameObject door;

    public Camera currentCamera;
    public Transform cameraFinalPoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController.instance.Finish();
            CameraFollow.instance.cameraFollowActive = false;
            CameraFollow.instance.cameraLookAtPlayer = true;
            playerCapsuleCol.enabled = false;
            playerRb.isKinematic = true;

            var seq = DOTween.Sequence();
            seq.Append(player.transform.DOMove(transform.position, 1f));
            seq.Join(player.transform.DORotate(Vector3.zero, 1f));
            seq.Append(player.transform.DOPath(pathToExit, 3f).SetEase(Ease.Linear));
            seq.Join(currentCamera.transform.DOMove(cameraFinalPoint.position, 1f));
            seq.Append(door.transform.DORotate(Vector3.zero, 1f).OnComplete(UIWinLose.instance.Win));
        }
    }
}
