using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector2 velocity;
    private Transform player;
    private float shakeTimer;
    private float shakeAmount;

    public float smoothTimeX;
    public float smoothTimeY;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer >= 0f)
        {
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);
        }

        shakeTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);
    }

    public void ShakeCamera(float Timer, float Amount)
    {
        shakeTimer = Timer;
        shakeAmount = Amount;
    }
}
    