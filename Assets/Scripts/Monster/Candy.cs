using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private Monster candyOwner;
    private Transform player;
    private Vector3 direction;

    private bool isFollowing = false;
    private IEnumerator followPlayerCo;

    public void StartFollowingPlayer(Transform player)
    {
        Debug.Log("[Candy] Start Following Player.");
        this.player = player;
        isFollowing = true;
        StartCoroutine(followPlayerCo = FollowPlayer());
    }
    private IEnumerator FollowPlayer()
    {
        while (isFollowing == true)
        {
            direction = player.position - transform.position;
            transform.Translate(direction);
            yield return new WaitForEndOfFrame();
            Debug.Log($"[Candy] Distance from Player : {direction.magnitude}");
            if (direction.magnitude < 0.5f)
            {
                isFollowing = false;
                break;
            }
        }
        DisableCandy();
    }
    private void DisableCandy()
    {
        Debug.Log($"[Candy] Returning to Monster : {candyOwner.name}");
        if (followPlayerCo != null)
        {
            StopCoroutine(followPlayerCo);
            followPlayerCo = null;
        }
        if (candyOwner != null)
            candyOwner.RecoverCandy(gameObject);
        gameObject.SetActive(false);
    }
}
