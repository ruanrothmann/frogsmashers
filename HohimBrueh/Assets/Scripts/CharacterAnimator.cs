using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    enum AnimState
    {
        Idle,
        Running,
        Jumping,
        Skidding,
        BouncedInAir,
        ChargeAttack,
        Attacking,
        AttackRecover,
        WallSlide,
        Tongue
    }

    enum AttackDirection
    {
        Forward,
        Up,
        DiagonalUp,
        Down,
        DownForward
    }

    AttackDirection DetermineAttackDirection()
    {
        if (character.attackDir.y == 1f && character.attackDir.x == 0f)
        {
            return AttackDirection.Up;
        }
        else if (character.attackDir.y > 0.45f && Mathf.Abs(character.attackDir.x) > 0.45f)
        {
            return AttackDirection.DiagonalUp;
        }
        else if (character.attackDir.y < -0.45f && Mathf.Abs(character.attackDir.x) > 0.45f)
        {
            return AttackDirection.DownForward;
        }
        else if (character.attackDir.y == -1f)
        {
            return AttackDirection.Down;
        }
        else
        {
            return AttackDirection.Forward;
        }
    }

    //Vector3 defaultOffset;
    //Vector3 jumpFromPosition;
    Vector3 spriteDefaultOffset;

    public Character character;

    public SpriteRenderer rend;

    public LineRenderer tongueLine;

    public SpriteRenderer tongueTip;
    float particleCounter;
    float lastSkidXPos;
    public float skidEffectDistance;

    public Sprite[] idle;
    public Sprite[] run;
    public Sprite[] jumpLaunch;
    public Sprite[] jumpUp;
    public Sprite[] jumpDown;
    public Sprite skidLand;
    public Sprite[] skid;
    public Sprite skidRecover;
    public Sprite[] somersault;
    public Sprite[] bouncedFlying;
    public Sprite[] bouncedFlyingComet;
    public Sprite[] bouncedFlyingRecovered;
    public Sprite[] bouncedFlyingSpinning;
    public Sprite[] bouncedFlyingRotationg;

    public Sprite[] attackCharge;
    public Sprite[] attack;
    public Sprite[] attackRecover;

    public Sprite[] attachChargeUp;
    public Sprite[] attackUp;
    public Sprite[] attackRecoverUp;

    public Sprite[] attackChargeDiagUp;
    public Sprite[] attackDiagUp;
    public Sprite[] attackRecoverDiagUp;

    public Sprite[] attackChargeDown;
    public Sprite[] attackDown;
    public Sprite[] attackRecoverDown;

    public Sprite[] attackChargeDownForward;
    public Sprite[] attackDownForward;
    public Sprite[] attackRecoverDownForward;

    public Sprite[] impact;

    public Sprite[] wallSlide;
    public Sprite[] wallSlideJumpLaunch;
    public Sprite[] tongue;
    public Sprite[] tongueDownMovingUp;
    public Sprite[] tongueDownMovingDown;
    public Sprite[] tongueRetractStunned;
    public Sprite[] tongueBurp;
    public Sprite[] blush;

    public Sprite[] win;

    public AudioClip[] flight;

    public AudioSource flightAudioSource;
    public float flightHeightPitchMod;
    public bool modFlightVolume;
    public float flightVelocityVolumeMod;

    public float smokeRingDistance;
    public float lineVelocityScale;
    Vector2 lastSmokeRingPos;

    int frame;
    float frameCounter;

    AnimState animState;

    float trailCounter;
    float trailFaderCounter;
    public float trailDelay;

    int variationRandomizer;

    float transitionTime;



    float t
    {
        get
        {
            return character.t;
        }
    }


    void Start()
    {
        if (character.player != null)
        {
            //rend.color = Color.Lerp(character.player.color, Color.white, 0.5f);
            rend.color = character.player.color;
            spriteDefaultOffset = rend.transform.localPosition;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {


        var newAnimState = DetermineAnimState();
        rend.transform.localRotation = Quaternion.identity;
        rend.transform.localPosition = spriteDefaultOffset;
        rend.transform.localScale = new Vector3(character.FacingDirection, 1f, 1f);


        if (animState != newAnimState)
        {
            frame = 0;
            frameCounter = 0f;

            if (animState == AnimState.Skidding)
            {
                transitionTime = 0.05f;
                rend.sprite = skidRecover;
            }
            else if (newAnimState == AnimState.Jumping && animState == AnimState.WallSlide)
            {
                transitionTime = 0.05f;
                rend.sprite = wallSlideJumpLaunch[0];
            }
            variationRandomizer = Random.Range(0, 10);

            if (newAnimState != AnimState.Tongue)
            {
                tongueLine.enabled = false;
                tongueTip.gameObject.SetActive(false);
            }


        }

        animState = newAnimState;


        if (transitionTime > 0f)
        {
            if (animState != AnimState.BouncedInAir)
            {
                transitionTime -= t;
                return;
            }
            else transitionTime = 0f;
        }


        switch (animState)
        {
            case AnimState.Idle:
                AnimateIdle();
                break;
            case AnimState.Running:
                AnimateRun();
                break;
            case AnimState.Jumping:
                AnimateJumping();
                break;
            case AnimState.Skidding:
                AnimateSkid();
                break;
            case AnimState.BouncedInAir:
                AnimateBouncedFlying();
                break;
            case AnimState.ChargeAttack:
                AnimateChargeAttack();
                break;
            case AnimState.Attacking:
                AnimateAttack();
                break;
            case AnimState.AttackRecover:
                AnimateAttackRecover();
                break;
            case AnimState.WallSlide:
                AnimateWallSlide();
                break;
            case AnimState.Tongue:
                AnimateTongue();
                break;
            default:
                break;
        }

        if (character.IngestedFly && (character.state != CharacterState.Tounge || character.tongueState != TongueState.HitFlyBurping))
            RunTrailSilhouettePoweredUp();
        else
            rend.color = character.player.color;

        RunFlightAudio();

    }


    void AnimateRun()
    {
        int frameBefore = frame;
        RunAnimation(run, 0.04f);
        if (frame != frameBefore && frame % 2 == 1)
        {
            FreeLives.SoundController.PlaySoundEffect("Footstep", 0.1f, transform.position);
            EffectsController.CreateDustPuff(transform.position, character.FacingDirection);
        }
    }


    void AnimateIdle()
    {

        frameCounter += t;
        if (GameController.State == GameState.RoundFinished && character.IsWinningPlayer())
        {
            if (frameCounter < 0.3f)
            {
                rend.sprite = idle[0];
            }
            else if (frameCounter < 0.4f)
            {
                rend.sprite = win[0];
            }
            else
            {
                if (frameCounter % 2f < 0.9f)
                {
                    rend.sprite = win[3];
                }
                else if (frameCounter % 2f < 1f)
                {
                    rend.sprite = win[2];
                }
                else if (frameCounter % 2f < 1.9f)
                {
                    rend.sprite = win[1];
                }
                else
                {
                    rend.sprite = win[2];
                }
            }
        }
        else
        {
            if (frameCounter < 1f)
            {
                rend.sprite = idle[0];
            }
            else
            {

                if (frameCounter % 3f < 2.95f)
                {
                    rend.sprite = idle[1];
                }
                else
                {
                    rend.sprite = idle[2];
                }

            }
        }

    }

    void AnimateChargeAttack()
    {
        var ad = DetermineAttackDirection();

        if (ad == AttackDirection.Up)
            RunAnimation(attachChargeUp, Mathf.Lerp(0.2f, 0.03f, character.attackChargeM));
        else if (ad == AttackDirection.DiagonalUp)
        {
            RunAnimation(attackChargeDiagUp, Mathf.Lerp(0.2f, 0.03f, character.attackChargeM));
        }
        else if (ad == AttackDirection.Down)
        {
            RunAnimation(attackChargeDown, Mathf.Lerp(0.2f, 0.03f, character.attackChargeM));
        }
        else if (ad == AttackDirection.DownForward)
        {
            RunAnimation(attackChargeDownForward, Mathf.Lerp(0.2f, 0.03f, character.attackChargeM));
        }
        else
        {
            RunAnimation(attackCharge, Mathf.Lerp(0.2f, 0.03f, character.attackChargeM));
        }
    }
    bool wasBurp;
    void AnimateTongue()
    {
        if (character.tongueState == TongueState.HitFlyBurping)
        {
            if (!wasBurp)
            {
                frame = 0;
                frameCounter = 0f;
                wasBurp = true;
            }

            RunAnimation(tongueBurp, 0.05f,true);
        }
        else
        if (character.tongueState == TongueState.HitEnemyTongueStunned)
        {
            wasBurp = false;
            tongueTip.gameObject.SetActive(false);
            tongueLine.enabled = false;
            RunAnimation(blush, 0.05f);
        }
        else if (!character.OnGround && character.tongueDir.y < 0)
        {
            wasBurp = false;
            if (character.velocity.y >= 0)
            {
                RunAnimation(tongueDownMovingUp, 0.1f, true);
            }
            else
            {
                RunAnimation(tongueDownMovingDown, 0.1f, true);
            }
            tongueLine.enabled = true;
            tongueTip.gameObject.SetActive(true);
            tongueLine.SetPosition(1, Vector3.up * 1.5f + (Vector3)character.tongueDir * character.tongueDistance + Vector3.forward * 1.1f);
            tongueTip.transform.localPosition = (Vector3)(character.tongueOrigin + character.tongueDir * character.tongueDistance) + Vector3.forward;
            tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(Vector2.right, character.tongueDir));

            if (character.tongueDir.y < 0f)
                tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, -Vector2.Angle(Vector2.right, character.tongueDir));
            else
                tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(Vector2.right, character.tongueDir));
        }
        else
        {
            wasBurp = false;
            if (character.tongueState == TongueState.RetractingHitEnemyTongue)
                RunAnimation(tongueRetractStunned, 0.05f);
            else
                RunAnimation(tongue, 0.1f, true);
            tongueLine.enabled = true;
            tongueTip.gameObject.SetActive(true);
            tongueLine.SetPosition(1, Vector3.up * 1.5f + (Vector3)character.tongueDir * character.tongueDistance + Vector3.forward * 1.1f);
            tongueTip.transform.localPosition = (Vector3)(character.tongueOrigin + character.tongueDir * character.tongueDistance) + Vector3.forward;
            tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(Vector2.right, character.tongueDir));

            if (character.tongueDir.y < 0f)
                tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, -Vector2.Angle(Vector2.right, character.tongueDir));
            else
                tongueTip.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.Angle(Vector2.right, character.tongueDir));
        }

        if (character.wasBouncingBeforeTongue)
            RunTrailSilhouette();

    }

    void AnimateAttack()
    {
        var ad = DetermineAttackDirection();

        if (ad == AttackDirection.Up)
            RunAnimation(attackUp, 0.05f, true);
        else if (ad == AttackDirection.DiagonalUp)
        {
            RunAnimation(attackDiagUp, 0.05f, true);
        }
        else if (ad == AttackDirection.Down)
        {
            RunAnimation(attackDown, 0.05f, true);
        }
        else if (ad == AttackDirection.DownForward)
        {
            RunAnimation(attackDownForward, 0.05f, true);
        }
        else
            rend.sprite = attack[0];
    }

    void AnimateAttackRecover()
    {
        var ad = DetermineAttackDirection();

        if (ad == AttackDirection.Up)
            RunAnimation(attackRecoverUp, 0.05f, true);
        else if (ad == AttackDirection.DiagonalUp)
        {
            RunAnimation(attackRecoverDiagUp, 0.05f, true);
        }
        else if (ad == AttackDirection.Down)
        {
            RunAnimation(attackRecoverDown, 0.05f, true);
        }
        else if (ad == AttackDirection.DownForward)
        {
            RunAnimation(attackRecoverDownForward, 0.05f, true);
        }
        else
        {
            RunAnimation(attackRecover, 0.05f, true);
        }
    }

    bool havePlayedLaunchSound;


    void AnimateBouncedFlying()
    {

        if (character.TimeBumpActive && character.timeBumpTimeScale == 0)
        {
            RunAnimation(impact, 0.05f, false, true);
            havePlayedLaunchSound = false;
        }
        else
        if (!character.hasBounceDodged)
        {
            if (!havePlayedLaunchSound)
            {
                int hits = Mathf.Clamp(character.hitsTaken, 0, 5);
                int level = 0;
                if (hits >= 5)
                    level = 3;
                else if (hits >= 3)
                    level = 2;
                else if (hits >= 1)
                    level = 1;
                havePlayedLaunchSound = true;
                if (level > 2)
                {
                    FreeLives.SoundController.PlaySoundEffect("Launch" + level.ToString(), 0.5f, transform.position);
                    FreeLives.SoundController.PlaySoundEffect("LaunchVoice" + level.ToString(), 0.5f, transform.position);
                }
            }

            if (!character.canBounceDodge)
            {



                float a = Vector3.Angle(Vector3.up, character.velocity);
                if (character.velocity.x > 0f)
                    a = 360f - a;
                rend.transform.localRotation = Quaternion.Euler(0, 0, a);
                var p = spriteDefaultOffset;
                p.y = 1f;
                rend.transform.localPosition = p;
                //if (variationRandomizer % 2 == 0)
                //RunAnimation(bouncedFlying, 0.05f);
                //else
                if (character.hitsTaken > 4)
                    RunAnimation(bouncedFlyingComet, 1f / 25f);
                else
                if (character.hitsTaken > 2)
                    RunAnimation(bouncedFlying, 1f / 25f);
                else
                    RunAnimation(bouncedFlyingSpinning, 1f / 25f);

                if (character.hitsTaken > 2)
                {
                    particleCounter += t;
                    if (particleCounter > trailDelay)
                    {
                        particleCounter -= trailDelay;
                        EffectsController.CreateStarParticles(character.Center, character.velocity * 0.1f, 0.5f, 1, new Color[] { Color.white, Color.black, character.player.color });
                    }
                }
            }
            else
            {

                float a = Vector3.Angle(Vector3.up, character.velocity);
                if (character.velocity.x > 0f)
                    a = 360f - a;
                rend.transform.localRotation = Quaternion.Euler(0, 0, a);// + 30f * character.FacingDirection);
                RunAnimation(bouncedFlyingRecovered, 0.05f);
            }
        }
        else
        {
            //if (flightAudioSource.isPlaying)
            //    flightAudioSource.Stop();
            RunAnimation(somersault, 0.05f);

        }

        if (character.hitsTaken > 0)
        {


            RunTrailSilhouette();
            if (!character.hasBounceDodged)
            {
                if (Vector2.Distance(lastSmokeRingPos, character.Center) > smokeRingDistance && character.velocity.magnitude > character.maxRunSpeed && character.hitsTaken > 2)
                {
                    lastSmokeRingPos = character.Center;
                    EffectsController.CreateSmokeRing(character.Center, rend.transform.rotation, character.player.color);
                }

                trailCounter -= t;
                if (trailCounter < 0f)
                {
                    trailCounter += trailDelay;

                    if (character.lastHitByPlayer != null)
                    {





                        for (int i = 0; i < character.hitsTaken; i++)
                        {
                            Color col = Color.white;
                            Color lerpFrom = character.player.color;// Random.value < character.hitsTaken * 0.02f ? character.lastHitByPlayer.color : character.player.color;
                            if (Random.value < 0.5f)
                                col = Color.Lerp(lerpFrom, Color.black, Random.value * 0.3f);
                            else
                                col = Color.Lerp(lerpFrom, Color.white, Random.value * 0.8f);
                            //if (Random.value < 0.2f)
                            if (character.velocity.magnitude > character.maxRunSpeed)
                                EffectsController.CreateLineParticle(character.Center + (Vector3)Random.insideUnitCircle * 0.5f + Vector3.forward, rend.transform.rotation, col, character.velocity * 0f, 0.3f + character.velocity.magnitude * lineVelocityScale, 0.25f + 0.5f * character.velocity.magnitude / character.maxRunSpeed);
                        }
                        //EffectsController.CreateSmokePuff(character.Center + (Vector3)Random.insideUnitCircle * 0.25f + Vector3.forward, col);
                    }
                }
            }

        }
    }

    void RunTrailSilhouette()
    {
        trailFaderCounter -= Time.deltaTime;
        if (trailFaderCounter <= 0f)
        {
            trailNumber++;
            trailFaderCounter += trailDelay * 2f;
            EffectsController.CreateTrailFader(this, 0.1f + character.hitsTaken * 0.15f, Color.Lerp(character.lastHitByPlayer.color, Color.white, Mathf.PingPong(trailNumber * 0.2f, 1f)));
        }
    }

    void RunTrailSilhouettePoweredUp()
    {
        trailFaderCounter -= Time.deltaTime;
        if (trailFaderCounter <= 0f)
        {
            trailNumber++;
            trailFaderCounter += trailDelay * 7f;
            if (trailNumber % 3 == 0)
                rend.color = Color.white;
            else if (trailNumber % 3 == 1)
                rend.color = Color.black;
            else
            {
                Vector3 col = Random.onUnitSphere;
                rend.color = character.player.color;// new Color(Mathf.Abs(col.x), Mathf.Abs(col.y), Mathf.Abs(col.z));
            }

            //EffectsController.CreateTrailFader(this, 0.05f, new Color(Random.value, Random.value, Random.value), true);
        }
    }

    float flightOrigin;
    void RunFlightAudio()
    {
        if (character.state == CharacterState.Bouncing)
        {
            if (character.TimeBumpActive && character.timeBumpTimeScale == 0)
            {
                if (flightAudioSource.isPlaying)
                {
                    flightAudioSource.Stop();
                }
                flightOrigin = character.transform.position.y;
            }
            else
            if (!character.hasBounceDodged)
            {
                flightAudioSource.volume = 0.15f;
                if (!flightAudioSource.isPlaying)
                {
                    if (character.hitsTaken > 4)
                        flightAudioSource.clip = flight[2];
                    else if (character.hitsTaken > 2)
                        flightAudioSource.clip = flight[1];
                    else if (character.hitsTaken > 0)
                        flightAudioSource.clip = flight[0];
                    flightAudioSource.Play();
                }
                flightAudioSource.pitch = 1f + (character.transform.position.y - flightOrigin) * flightHeightPitchMod;
                if (modFlightVolume)
                    flightAudioSource.volume = character.velocity.magnitude * flightVelocityVolumeMod;

            }
            else
            {
                if (flightAudioSource.isPlaying)
                    flightAudioSource.Stop();
            }
        }
        else
        {
            flightAudioSource.volume = Mathf.MoveTowards(flightAudioSource.volume, 0f, Time.deltaTime * 2f);
        }
    }
    int trailNumber;
    void AnimateJumping()
    {
        if (character.velocity.y < character.gravityGraceThreshold && character.input.aButton && character.gravityGraceTimeLeft > 0f)
        {
            float m = 1f - character.gravityGraceTimeLeft / character.gravityGraceTime;
            rend.sprite = somersault[(int)(m * somersault.Length)];
        }
        else
        if (character.Velocity.y > 0f)
        {
            if (frame < jumpLaunch.Length)
            {
                //transform.position = new Vector3(transform.position.x, jumpFromPosition.y, transform.position.z);
                rend.sprite = jumpLaunch[frame];
                frameCounter += t;
                if (frameCounter > 0.05f)
                {
                    frame++;
                    frameCounter = 0f;
                }
            }
            else
            {
                //transform.localPosition = defaultOffset;
                RunAnimation(jumpUp, 0.05f);
            }
        }
        else
        {
            RunAnimation(jumpDown, 0.05f);
        }
    }

    void AnimateSkid()
    {
        if (Mathf.Abs(transform.position.x - lastSkidXPos) > skidEffectDistance && (Mathf.Abs(character.velocity.x) > character.maxRunSpeed))
        {
            lastSkidXPos = transform.position.x;
            EffectsController.CreateJumpPuffSkew(transform.position, character.FacingDirection);
        }

        if (frame == 0)
        {
            rend.sprite = skidLand;
            frameCounter += t;
            if (frameCounter > 0.075f)
            {
                frame++;
                frameCounter = 0f;
            }
        }
        else
        {

            RunAnimation(skid, 0.05f);
        }

        if (character.hitsTaken > 0)
            RunTrailSilhouette();
    }

    void RunAnimation(Sprite[] frames, float frameDelay, bool clamp = false, bool ignoreCharacterTimescale = false)
    {
        if (ignoreCharacterTimescale)
            frameCounter += Time.deltaTime;
        else
            frameCounter += t;

        if (frameCounter > frameDelay || frame < 0)
        {
            frame++;
            frameCounter -= frameDelay;
        }
        if (clamp)
            rend.sprite = frames[Mathf.Clamp(frame, 0, frames.Length - 1)];
        else
            rend.sprite = frames[frame % frames.Length];
    }

    void AnimateWallSlide()
    {
        RunAnimation(wallSlide, 1f / 25f);
    }


    AnimState DetermineAnimState()
    {

        if (character.state == CharacterState.Bouncing)
        {
            if (character.OnGround)
            {
                return AnimState.Skidding;
            }
            else
            {
                return AnimState.BouncedInAir;
            }
        }
        else if (character.state == CharacterState.Attacking)
        {
            if (character.attackState == AttackState.Charging)
            {
                return AnimState.ChargeAttack;
            }
            else if (character.attackState == AttackState.Attacking)
            {
                return AnimState.Attacking;
            }
            else if (character.attackState == AttackState.Recovering)
            {
                return AnimState.AttackRecover;
            }
        }
        else if (character.state == CharacterState.Tounge)
        {
            return AnimState.Tongue;
        }
        else if (character.OnGround && Mathf.Abs(character.Velocity.x) > 0f)
        {
            return AnimState.Running;
        }
        else if (!character.OnGround)
        {
            if (character.WallSliding)
            {
                return AnimState.WallSlide;
            }
            else
                return AnimState.Jumping;
        }

        return AnimState.Idle;
    }
}
