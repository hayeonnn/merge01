using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour
{
    public float jumpPower;
    public int itemCount;
    public GameManager manager;
    bool isJump;
    Rigidbody rigid;

    private float moveSpeed = 20.0f;    //이동속도(z축)
    private float rotateSpeed = 300.0f;  //회전속도

    void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
    }

    public GameObject targetPosition;
    //targetPosition = GameObject.FindWithTag("Item");
    //Vector3 target = new Vector3(Component.tag.Item);
    void Update()
    {
       //z축이동
        //transform.position += Vector3.MoveTowards(transform.position, Vector3, 0.1f) * moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(gameObject.transform.position, GameObject.FindWithTag("Item").transform.position, 0.01f);

        //오브젝트 회전(x축)
        transform.Rotate(Vector3.right * rotateSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rigid.AddForce(new Vector3(h, 0, v), ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision) { 
        if(collision.gameObject.tag == "Floor")
            isJump = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            itemCount++;
            GetComponent<AudioSource>().Play();
            //other.gameObject.SetActive(false);
            manager.GetItem(itemCount);
            other.gameObject.tag = "ok";
        }
        else if (other.tag == "Finish")
        {
            if (itemCount == manager.totalItemCount)
            {                
                //Game Clear!
                if (manager.stage == 1)
                    SceneManager.LoadScene(0);
                else
                    SceneManager.LoadScene(manager.stage + 1);
            }
            else {
                //Restart..
                SceneManager.LoadScene(manager.stage);
            }
            //GameObject.FindGameObjectWithTag // find는 cpu많이잡아먹기때문에 잘 안씀
        }
    }
}
