using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DemoLevelLoader : MonoBehaviour
{
    public float openRange = 3f;
    public Transform EntranceRange;
    public LayerMask playerLayer;
    [SerializeField]public string scene;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] open = Physics2D.OverlapCircleAll(EntranceRange.position, openRange, playerLayer);


        if (open.Length != 0 && Input.GetKeyDown(KeyCode.E))
        {
            Loader.Load(scene);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(EntranceRange.position, openRange);
    }
}
