using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    private static CameraShake instance;

    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No Camerashake instance! Attach to camera transform");
            }
            return instance;
        }
    }

    const float shakeScale = 1f; // Change this value to fit the scale at which your game is running

    Vector3 lastShakeOffset;
    float xShakeCounter, yShakeCounter, xShakeAmount,yShakeAmount,xShakeSpeed,yShakeSpeed;
    float vibrateAmount;


    void Awake()
    {
        instance = this;
        xShakeCounter = Random.value * 100f;
        yShakeCounter = Random.value * 100f;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position -= lastShakeOffset;


        xShakeAmount -= Time.deltaTime;
        if (xShakeAmount < 0f)
            xShakeAmount = 0f;
        yShakeAmount -= Time.deltaTime;
        if (yShakeAmount < 0f)
            yShakeAmount = 0f;
        xShakeCounter += xShakeSpeed * Time.deltaTime * 6f;
        yShakeCounter += yShakeSpeed * Time.deltaTime * 6f;
        xShakeSpeed = Mathf.Lerp(xShakeSpeed, 1f, Time.deltaTime * 2f);
        yShakeSpeed = Mathf.Lerp(yShakeSpeed, 1f, Time.deltaTime * 2f);
        vibrateAmount = Mathf.Lerp(vibrateAmount, 0f, Time.deltaTime * 5f);


        lastShakeOffset = Vector3.zero;

        lastShakeOffset += transform.right * Mathf.Sin(xShakeCounter) * xShakeAmount * shakeScale;
        lastShakeOffset += transform.up * Mathf.Sin(yShakeCounter) * yShakeAmount * shakeScale;

        lastShakeOffset += Random.insideUnitSphere * vibrateAmount;

        transform.position += lastShakeOffset;

    }

    public void Shake(float m, float speedM)
    {
        if (xShakeAmount < m)
            xShakeAmount = m * Random.Range(0.8f, 1.25f);
        if (yShakeAmount < m)
            yShakeAmount = m * Random.Range(0.8f, 1.25f);

        if (xShakeSpeed < speedM)
            xShakeSpeed = speedM * Random.Range(0.8f, 1.25f);
        if (yShakeSpeed < speedM)
            yShakeSpeed = speedM * Random.Range(0.8f, 1.25f);

    }

    public void Shake(float mX, float speedMX, float mY, float speedMY)
    {
        if (xShakeAmount < mX)
            xShakeAmount = mX * Random.Range(0.8f, 1.25f);
        if (yShakeAmount < mY)
            yShakeAmount = mY * Random.Range(0.8f, 1.25f);

        if (xShakeSpeed < speedMX)
            xShakeSpeed = speedMX * Random.Range(0.8f, 1.25f);
        if (yShakeSpeed < speedMY)
            yShakeSpeed = speedMY * Random.Range(0.8f, 1.25f);

    }

    public void Vibrate(float amount)
    {
        if (vibrateAmount < amount)
            vibrateAmount = amount;
    }



}
