using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeLives;
public class IntroAnimController : MonoBehaviour
{

    public Confetti[] characterParticlePrefabs;
    public float characterParticleLaunchVelocity;

    public AudioSource musicAudio;

    public bool cameraPanning;

    public float cameraPanDelay;

    public SoundHolder soundHolder;

    public float panTo;
    public float panTime;
    public AnimationCurve cameraPosCurve;
    float cameraPosM;

    public Animator animator;

    public GameObject fly;

    public Transform backgroundTransform;

    public float shakeMaxSpeed, shakeMaxAmount;

    float shakeXCounter, shakeYCounter;
    float shakeXSpeed, shakeYSpeed;
    float shakeXAmount, shakeYAmount;

    bool stopPlayingTapSound;

    public float shakeSpeedDecay, shakeAmountDecay;

    public void PlaySound(string name)
    {
        if (!stopPlayingTapSound)
            SoundController.PlaySoundEffect(soundHolder, name);
    }

   
    public void StartCameraPan()
    {   
        cameraPanning = true;
    }

    public void StartMusic()
    {
        musicAudio.Play();
    }

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        input = new InputState();
    }
    InputState input;

    float shakeDelay;
    Vector3 shakeOffset = Vector3.zero;
    // Update is called once per frame
    void Update()
    {
        if (cameraPanning && (cameraPanDelay -= Time.deltaTime) < 0f)
        {
            var p = Camera.main.transform.position;
            cameraPosM += 1 / panTime * Time.deltaTime;
            cameraPosM = Mathf.Clamp01(cameraPosM);
            p.y = cameraPosCurve.Evaluate(cameraPosM) * panTo;
            Camera.main.transform.position = p;
        }

        InputReader.GetInput(input);
        if (cameraPosM == 1f)
        {
            if (input.start && !input.wasStart)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("JoinScreen");
            }
        }
        else
        {
            if (input.start && !input.wasStart)
            {
                animator.CrossFade(animator.GetCurrentAnimatorStateInfo(-1).shortNameHash, 0f, 0, 0.95f);
                cameraPanning = true;
                cameraPanDelay = 0f;
                cameraPosM = 1f;
            }
        }
        if (cameraPosM > 0.55f)
        {
            shakeDelay -= Time.deltaTime;
            if (shakeDelay < 0f)
            {
                ShakeBackground();
                shakeDelay = Random.Range(0.2f, 1f);
            }
        }
        //if (cameraPosM  > 0.5f)
        {
            backgroundTransform.position -= shakeOffset;
            shakeXCounter += Time.deltaTime * shakeXSpeed;
            shakeYCounter += Time.deltaTime * shakeYSpeed;
            shakeOffset.x = Mathf.Sin(shakeXCounter) * shakeXAmount;
            shakeOffset.y = Mathf.Sin(shakeYCounter) * shakeYAmount;
            backgroundTransform.position += shakeOffset;

            shakeXAmount = Mathf.Lerp(shakeXAmount, 0f, shakeAmountDecay * Time.deltaTime);
            shakeYAmount = Mathf.Lerp(shakeYAmount, 0f, shakeAmountDecay * Time.deltaTime);

            shakeXSpeed = Mathf.Lerp(shakeXSpeed, 0f, shakeSpeedDecay * Time.deltaTime);
            shakeYSpeed = Mathf.Lerp(shakeYSpeed, 0f, shakeSpeedDecay * Time.deltaTime);

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ShakeBackground();
        }
    }

    void ShakeBackground()
    {
        stopPlayingTapSound = true;

        int level = Random.Range(1, 4);

        SoundController.PlaySoundEffect("BatHit" + level.ToString(), Random.Range(0.2f, 0.5f), Camera.main.transform.position + Vector3.right * Random.Range(-5f, 5f));
        if (Random.value < 0.5f)
        {
            SoundController.PlaySoundEffect("BatHitVoice" + level.ToString(), Random.Range(0.2f, 0.5f), Camera.main.transform.position + Vector3.right * Random.Range(-5f, 5f));
        }
        else if (Random.value < 0.25f)
        {
            SoundController.PlaySoundEffect("LaunchVoice3", Random.Range(0.2f, 0.5f), Camera.main.transform.position + Vector3.right * Random.Range(-5f, 5f));
        }

        if (Random.value < 0.5f)
        {
            shakeXSpeed = Mathf.Clamp(shakeXSpeed + Random.Range(0f, shakeMaxSpeed), 0f, shakeMaxSpeed);
            shakeYSpeed = Mathf.Clamp(shakeYSpeed + Random.Range(0f, shakeMaxSpeed), 0f, shakeMaxSpeed);
            shakeXAmount = Mathf.Clamp(shakeXAmount + Random.Range(0f, shakeMaxAmount), 0f, shakeMaxAmount);
            shakeYAmount = Mathf.Clamp(shakeYAmount + Random.Range(0f, shakeMaxAmount), 0f, shakeMaxAmount);
        }
        if (Random.value < 0.5f)
        {
            var cf = Instantiate(characterParticlePrefabs[Random.Range(0, characterParticlePrefabs.Length)], new Vector3(Random.Range(-5f, 5f), -2f, 0f), Quaternion.identity) as Confetti;
            cf.velocity = new Vector3(0f, characterParticleLaunchVelocity, 0f);

            cf.GetComponent<SpriteRenderer>().color = EffectsController.GetRandomColor();

            cf.velocity = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f)) * cf.velocity;
        }
    }

}
