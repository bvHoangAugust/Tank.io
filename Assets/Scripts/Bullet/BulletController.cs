using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviourPunCallbacks
{
    public int damageBullet;

    Animator anim;

    Rigidbody2D mybody;

    RaycastHit2D hit;
    bool checkRaycast;
    void Awake()
    {
        checkRaycast = true;
        mybody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        DetectCollistion();
    }

    public void SetDame(int tankDamage)
    {
        damageBullet = tankDamage;
    }
    public void MoveBullet(float tankBulletSpeed)
    {
        mybody.velocity = transform.up * tankBulletSpeed;
    }

    void DetectCollistion()
    {
        if (checkRaycast)
        {
            hit = Physics2D.Raycast(this.transform.position, Vector3.zero);
            if (hit.collider != null)
            {
                checkRaycast = false;
                mybody.velocity = Vector2.zero;

                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PhotonView>().RPC("Hit", RpcTarget.All, damageBullet);
                }
                ShowEndAnimation();
            }
        }
    }

    public void ShowEndAnimation()
    {
        if (Random.value <= 0.5f)
        {
            anim.Play("BulletExplosion");
        }
        else
        {
            anim.Play("BulletExplosionSmoke");
        }
    }
    public void Destroy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
