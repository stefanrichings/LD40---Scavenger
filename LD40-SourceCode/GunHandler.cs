using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject hitSpider;
    public Camera FPSCamera;
    vp_FPPlayerEventHandler player;

    void Awake ()
    {
        player = transform.GetComponent<vp_FPPlayerEventHandler>();
    }

    void OnEnable ()
    {
        if (player != null)
        {
            player.Register(this);
        }
    }

    void OnDisable ()
    {
        if (player != null)
            player.Unregister(this);
    }

    void OnMessage_CheckForHit()
    {
        RaycastHit hit;
        Debug.DrawRay(FPSCamera.transform.position, FPSCamera.transform.forward * 10, Color.red, 10f);
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hit))
        {
            if (hit.collider.GetComponent<Spider>() != null)
            {
                Spider s = hit.collider.GetComponent<Spider>();
                s.Hit();
                GameObject blood = Instantiate(hitSpider, hit.point, Quaternion.identity);
                StartCoroutine(DestroyHit(blood));
            }
        }
    }

    IEnumerator DestroyHit(GameObject g)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(g);
    }
}
