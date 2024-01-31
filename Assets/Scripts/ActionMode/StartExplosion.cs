using UnityEngine;

public class StartExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        var pos = transform.position;
        pos.y -= 3;

        rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);

    }
}
