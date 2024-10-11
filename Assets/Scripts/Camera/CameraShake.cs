using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float _duration = 3f;
    private float _magnitude = 100f;

    public void ShakeCamera()
    {
        Vector2 initialPosition = transform.position;
        float timer = 0f;

        while (timer < _duration)
        {
            float xMovement = Random.Range(-_magnitude, _magnitude + Mathf.Epsilon);
            float yMovement = Random.Range(-_magnitude, _magnitude + Mathf.Epsilon);

            transform.position = new Vector2(xMovement, yMovement);
            timer += Time.deltaTime;

            Debug.Log("Shake Camera");
        }

        transform.position = initialPosition;
    }
}
