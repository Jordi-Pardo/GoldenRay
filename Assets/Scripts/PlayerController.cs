using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private float resetSpeed;
    public float dashSpeed;
    public float dashTime;

    private PhotonView view;
    private Animator anim;
    private Health healthScript;

    private int runningHash = Animator.StringToHash("isRunning");

    private LineRenderer rend;

    public float minX, maxX, minY, maxY;

    public TMPro.TextMeshProUGUI nameDisplay;

    public GameObject bulletObj;


    private void Start()
    {
        resetSpeed = speed;
        view = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        healthScript = FindObjectOfType<Health>();
        rend = FindObjectOfType<LineRenderer>();

        if (view.IsMine)
        {
            nameDisplay.text = PhotonNetwork.NickName;
        }
        else
        {
            nameDisplay.text = view.Owner.NickName;
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {

            if (Input.GetMouseButtonDown(0))
            {
                view.RPC("FireRPC", RpcTarget.All, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
                //GameObject bullet = Instantiate(bulletObj, transform.position, Quaternion.identity);
                //bullet.GetComponent<BulletScript>().Init(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }

            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 moveAmount = moveInput.normalized * speed * Time.deltaTime;
            transform.position += (Vector3)moveAmount;

            Wrap();

            if(Input.GetKeyDown(KeyCode.Space) &&moveInput != Vector2.zero)
            {
                StartCoroutine(Dash());
            }


            if(moveInput == Vector2.zero)
            {
                anim.SetBool(runningHash, false);
            }
            else
            {
                anim.SetBool(runningHash, true);
            }

            rend.SetPosition(0, transform.position);
        }
        else
        {
            rend.SetPosition(1, transform.position);
        }
    }

    private void Wrap()
    {
        if(transform.position.x < minX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
        if(transform.position.x > maxX)
        {
            transform.position = new Vector2(minX, transform.position.y);
        }
        if(transform.position.y < minY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }
        if(transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, minY);
        }
    }

    private IEnumerator Dash()
    {
        speed = dashSpeed;
        yield return new WaitForSeconds(dashTime);
        speed = resetSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (view.IsMine)
        {
            if (collision.CompareTag("Enemy"))
            {
                healthScript.TakeDamage();
            }
        }
    }

    [PunRPC]
    public void FireRPC(Vector3 direction)
    {
        GameObject bullet=  Instantiate(bulletObj, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().Init(direction);

    }
}
