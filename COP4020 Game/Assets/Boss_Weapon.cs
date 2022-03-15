using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Weapon : MonoBehaviour
{
    public int attackDamage = 10;

    public Vector3 attackOffest;
    public float attackRange = 2f;
    public LayerMask attackMask;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffest.x;
        pos += transform.up * attackOffest.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if(colInfo != null)
        {
            //colInfo.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
}
