using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 target;
    Vector2 start;
    float speed;
    float duration;
    float height = 5;
    //AudioSource source;

    public void SetParameters(Vector2 target, float speed)//, AudioSource source)
    {
        this.speed = speed;
        this.target = target;
        //this.source = source;
    }

    void Start()
    {
        start = transform.position;
        duration = Vector2.Distance(start, target) / speed;
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        yield return new WaitUntil(() => duration != 0);
        float time = 0;
        while (time < duration)
        {
            float percentageOfPath = time / duration;
            Vector2 peak = (start + target) / 2 + Vector2.up * height;
            Vector2 pointA = Vector2.Lerp(start, peak, percentageOfPath);
            Vector2 pointB = Vector2.Lerp(peak, target, percentageOfPath);
            transform.position = Vector2.Lerp(pointA, pointB, percentageOfPath);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        Destroy(gameObject, 5);
        Yeet.SoundGenerated?.Invoke((Vector2)transform.position);
    }
}
