using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 target;
    Vector2 start;
    float duration;
    float height;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    AudioSource source;

    public void SetParameters(Vector2 target, float speed, float maxDistance, AudioSource source)
    {
        this.target = target;
        start = transform.position;
        float distance = Vector2.Distance(start, target);
        duration = distance / speed;
        height = Mathf.Lerp(minHeight, maxHeight, distance/maxDistance);
        this.source = source;
        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        float time = 0;
        while (time < duration)
        {
            float percentageOfPath = time / duration;
            Vector2 peak = (start + target) / 2 + (Vector2)transform.up * height;
            Vector2 pointA = Vector2.Lerp(start, peak, percentageOfPath);
            Vector2 pointB = Vector2.Lerp(peak, target, percentageOfPath);
            transform.position = Vector2.Lerp(pointA, pointB, percentageOfPath);
            time += Time.deltaTime;
            yield return null;
        }
        if (source != null)
        {
            source.Play();
        }
        transform.position = target;
        Destroy(gameObject, 5);
        Yeet.SoundGenerated?.Invoke((Vector2)transform.position);
    }

    void DeleteMe()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameStateManager.ResetGame += DeleteMe;
    }

    private void OnDisable()
    {
        GameStateManager.ResetGame -= DeleteMe;
    }
}
