using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavAgent : MonoBehaviour
{
    public Rigidbody body;
    Vector3 velocity;
    // Start is called before the first frame update
    public NavMeshAgent navaaaa;
    public Transform target;
    public Transform Me;
    public Animator anim;
    public Weapon equipWeapon;
    public Transform groundCheck;
    public float gorundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    bool hit = false;
    bool isFireReady = true;
    bool isGrounded;
    float fireDelay;
    Vector3 rese;
    
    
    // Update is called once per frame
    void Update()
    {
        
         if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            
            
        }
       
        navaaaa.SetDestination(target.position);

        isGrounded = Physics.CheckSphere(groundCheck.position, gorundDistance, groundMask);

        float chek = Mathf.Abs(target.position.x - Me.position.x) + Mathf.Abs(target.position.z - Me.position.z);

        
        //Debug.Log(chek);
        if(chek < 2.9)
        {
            navaaaa.isStopped = true;
        }
        else if(chek >2.8)
        {
            navaaaa.isStopped = false;
        }

        if(hit)
        {
            rese = rese.normalized;
            rese += Vector3.up;
            Useaaa();
            body.AddForce(rese * 1000, ForceMode.Force);
            hit = false;
        }

        velocity.y += gravity * Time.deltaTime;
        body.AddForce(velocity,ForceMode.Force);
        Attack(chek);
        anim.SetFloat("idle", chek);
        anim.SetBool("jump", isGrounded);
        
    }
    void FixedUpdate()
    {
        Frezen();
    }
    void Frezen()
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
    }
    void Attack(float chek)
    {
        // 공격할 조건만 플레이어에 두고, 공격로직은 무기에 위임한다.
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(chek < 2.8 && isFireReady && !hit) {
            equipWeapon.Use();
            anim.SetTrigger("attack");
            navaaaa.isStopped = false;
            fireDelay = 0;
        }
    
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.layer == 3 && other.gameObject.tag == "Player")
        {
            
            rese = Me.transform.position - target.transform.position;
            hit = true;
            navaaaa.enabled = false;
            
        }
        

        
    }
    public void Useaaa()
    {
        StopCoroutine("KnockBack");
        StartCoroutine("KnockBack");
    }
    IEnumerator KnockBack()
    {
        
        yield return new WaitForSeconds(0.1f);
        navaaaa.enabled = false;
        yield return new WaitForSeconds(2f);
        navaaaa.enabled = true;
    }
}
