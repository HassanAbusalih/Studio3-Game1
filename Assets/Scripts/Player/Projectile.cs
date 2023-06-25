using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 target;
    float speed;
    //AudioSource source;

    public void SetParameters(Vector2 target, float speed)//, AudioSource source)
    {
        this.speed = speed;
        this.target = target;
        //this.source = source;
    }

    void Start()
    {
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        while (target != null && Vector2.Distance(target, transform.position) > 0.1f)
        {
            Vector3 direction = target - (Vector2)transform.position;
            transform.position += direction.normalized * speed * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject, 5);
        Yeet.SoundGenerated?.Invoke((Vector2)transform.position);
    }
}
