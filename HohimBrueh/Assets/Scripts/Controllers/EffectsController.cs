using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{

    public HitEffect hitEffectPrefab, hitStarPrefab, hitStarPowerHitPrefab;

    public GameObject directionalHitEffect;

    public HitParticle hitParticlePrefab;

    public TrailFader faderTrailPrefab;

    static EffectsController instance;

    public KnockedUpEffect knockedUpEffect;

    public GameObject tongueHitEffect;

    public SideScorePlum sideScorePlumPrefab;

    public LocalizedShake localizedShakePrefab;

    public GameObject jumpPuffSkew, jumpPuffStraight, bouncePuff, wallPuff, dustPuff, turnAroundPuff, smokeRing, lineParticle;

    public GameObject confetti;

    public GameObject shingEffect;

    public ParticleSystem spawnPuffParticleSystem;

    public SpriteRenderer[] smokePuff;

    public GameObject spawnPuff;
    Vector2 cameraShakeVelocity;

    float confettiDelay;

    public float cameraSpring;
    public float shakePower;

    float startWinZoomDelay = 1.5f;

    public Vector2 cameraWobbleSpeed, cameraWobbleAmount;
    float cameraWobbleXCounter, cameraWobbleYCounter;

    public static void CreateSmokeRing(Vector3 pos, Quaternion rot, Color col)
    {
        var sr = Instantiate(instance.smokeRing, pos - Vector3.forward * 3f, rot) as GameObject;
    }

    public static void CreateLineParticle(Vector3 pos, Quaternion rot, Color col, Vector2 velocity, float velocityScale, float lifeScale)
    {
        var sr = Instantiate(instance.lineParticle, pos, rot) as GameObject;
        sr.GetComponent<SpriteRenderer>().color = col;
        sr.GetComponent<Confetti>().velocity = velocity;

        sr.transform.localScale = new Vector3(1f, velocityScale, 1f);
        sr.GetComponent<SimpleAnim>().animSpeed *= lifeScale * Random.Range(0.9f, 1.2f);
    }

    public static void CreateSmokePuff(Vector3 pos, Color col)
    {
        var sp = Instantiate(instance.smokePuff[Random.Range(0, instance.smokePuff.Length)], pos, Quaternion.identity) as SpriteRenderer;
        sp.color = col;
    }

    public static void CreateSpawnEffects(Vector2 pos, Color col)
    {
        //var spp = Instantiate(instance.spawnPuffParticleSystem, (Vector3)pos - Vector3.forward, Quaternion.identity) as ParticleSystem;
        //spp.startColor = col;
        //Destroy(spp.gameObject, 5f);
        var sp = Instantiate(instance.spawnPuff, (Vector3)pos - Vector3.forward + Vector3.up * 0.2f, Quaternion.identity) as GameObject;
        sp.GetComponent<SpriteRenderer>().color = col;

        //sp.main.startColor = col;
    }

    public static void ShakeCamera(Vector2 shakeVector, float intensity)
    {
        instance.cameraShakeVelocity += shakeVector * (instance.shakePower * intensity + 10f);
    }


    void Update()
    {

        if (!GameController.HasInstance)
            return;

        cameraWobbleXCounter += Time.deltaTime;
        cameraWobbleYCounter += Time.deltaTime;

        Vector3 wobbleVector = new Vector3(Mathf.Sin(cameraWobbleXCounter * cameraWobbleSpeed.x) * cameraWobbleAmount.x, Mathf.Sin(cameraWobbleYCounter * cameraWobbleSpeed.y) * cameraWobbleAmount.y,0f);

        if (GameController.State == GameState.Playing || (startWinZoomDelay -= Time.deltaTime) > 0f)
        {
            var p = Camera.main.transform.position;
            p += (Vector3)cameraShakeVelocity * Time.deltaTime;
            if (p.x > 1f && cameraShakeVelocity.x > 0f)
            {
                cameraShakeVelocity.x *= -0.9f;
                p.x = 1f;
            }
            if (p.y > 1f && cameraShakeVelocity.y > 0f)
            {
                cameraShakeVelocity.y *= -0.9f;
                p.y = 1f;
            }
            if (p.x < -1f && cameraShakeVelocity.x < 0f)
            {
                cameraShakeVelocity.x *= -0.9f;
                p.x = -1f;
            }
            if (p.y < -1f && cameraShakeVelocity.y < 0f)
            {
                cameraShakeVelocity.y *= -0.9f;
                p.y = -1f;
            }



            if (Mathf.Sign(cameraShakeVelocity.x) == Mathf.Sign(p.x))
            {
                cameraShakeVelocity.x -= p.x * cameraSpring * Time.deltaTime;
            }
            if (Mathf.Sign(cameraShakeVelocity.y) == Mathf.Sign(p.y))
            {
                cameraShakeVelocity.y -= p.y * cameraSpring * Time.deltaTime;
            }

            Camera.main.transform.position = Vector3.Lerp(p, wobbleVector - Vector3.forward * 10f, Time.deltaTime * 3f);
        }
        else if (GameController.State == GameState.RoundFinished)
        {
            var ch = GameController.GetWinningPlayer().character;
            if (ch != null)
            {

                var p = Camera.main.transform.position;
                p = Vector3.Lerp(p, ch.Center, Time.deltaTime * 5f);
                p.z = Camera.main.transform.position.z;


                //float horzExtent = 
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 7.5f, Time.deltaTime * 2f);

                float vertExtent = Camera.main.orthographicSize;
                float horzExtent = vertExtent * Screen.width / Screen.height;
                p.y = Mathf.Clamp(p.y, -(18 - vertExtent), 18 - vertExtent);
                p.x = Mathf.Clamp(p.x, -(32 - horzExtent), 32 - horzExtent);
                Camera.main.transform.position = p;

                confettiDelay -= Time.deltaTime;
                if (confettiDelay < 0f)
                {
                    confettiDelay = 0.03f;
                    float y = p.y + Camera.main.orthographicSize * Random.Range(0.75f, 1.5f);
                    float x = Random.Range(p.x - horzExtent * Random.Range(0.5f, 1.5f), p.x + horzExtent);

                    var cf = Instantiate(confetti, new Vector3(x, y, -1f), Quaternion.identity);
                    cf.GetComponent<SpriteRenderer>().color = GetRandomColor();
                    cf.GetComponent<Confetti>().velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.5f, -2f), 0f);
                }

            }
            else
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 18f, Time.deltaTime * 1f);
                var p = Camera.main.transform.position;
                p = Vector3.Lerp(p, Vector3.zero, Time.deltaTime * 1f);
                p.z = Camera.main.transform.position.z;
                Camera.main.transform.position = p;
            }
        }
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    ShakeCamera(Vector2.right,shak);
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    ShakeCamera(Vector2.left);
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    ShakeCamera(Vector2.up);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    ShakeCamera(Vector2.down);
        //}

    }

    public enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }

    static Quaternion GetRotationForSide(Side side)
    {
        switch (side)
        {
            case Side.Top:
                return Quaternion.Euler(0f, 0f, 180f);

            case Side.Bottom:
                return Quaternion.Euler(0f, 0f, 0f);

            case Side.Left:
                return Quaternion.Euler(0f, 0f, -90f);

            case Side.Right:
                return Quaternion.Euler(0f, 0f, 90f);
            default:
                return Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public static void CreateShingEffect(Vector2 pos, Vector2 attackDirection)
    {
        float rot = 0f;
        if (attackDirection.x > 0f)
            rot = -Vector3.Angle(Vector3.up, attackDirection);
        else rot = Vector3.Angle(Vector3.up, attackDirection);
        var se = Instantiate(instance.shingEffect, (Vector3)pos - Vector3.forward, Quaternion.Euler(0f, 0f, rot));

    }

    static Vector3 GetOffsetForSide(EffectsController.Side side)
    {
        switch (side)
        {
            case Side.Top:
                return new Vector3(0, 2f, 0f);

            case Side.Bottom:
                return new Vector3(0f, 0f, 0f);

            case Side.Left:
                return new Vector3(-1f, 1f, 0f);

            case Side.Right:
                return new Vector3(1f, 1f, 0f);
            default:
                return new Vector3(0f, 0f, 0f);
        }
    }

    public static void CreateJumpPuffStraight(Vector2 pos, Side orientation)
    {
        var jp = Instantiate(instance.jumpPuffStraight, (Vector3)pos + GetOffsetForSide(orientation) + Vector3.forward, GetRotationForSide(orientation));
    }

    public static void CreateJumpPuffSkew(Vector2 pos, float direction)
    {
        var jp = Instantiate(instance.jumpPuffSkew, (Vector3)pos + Vector3.forward, Quaternion.identity);
        jp.transform.localScale = new Vector3(direction, 1f, 1f);
    }

    public static void CreateBouncePuff(Vector2 pos, Side orientation)
    {
        GameObject prefab = instance.bouncePuff;
        if (orientation == Side.Left || orientation == Side.Right)
        {
            prefab = instance.wallPuff;
        }

        var jp = Instantiate(prefab, (Vector3)pos + Vector3.forward + GetOffsetForSide(orientation), GetRotationForSide(orientation));

    }

    public static void CreateDustPuff(Vector2 pos, float direction)
    {
        var jp = Instantiate(instance.dustPuff, (Vector3)pos + Vector3.forward, Quaternion.identity);
        jp.transform.localScale = new Vector3(direction, 1f, 1f);
    }

    public Color[] randomColors;

    void Awake()
    {
        instance = this;
    }

    public static void CreateHitEffect(Vector2 pos, float life, bool powerHit)
    {
        var he = Instantiate(instance.hitEffectPrefab, (Vector3)pos + Vector3.forward * 2f, Quaternion.identity) as HitEffect;
        he.life = life;
        he.scale = 1f + life * 0.4f;

        he = Instantiate(instance.hitStarPrefab, (Vector3)pos + Vector3.forward * 3f, Quaternion.identity) as HitEffect;
        he.life = life;
        he.scale = 1f + life * 0.4f;

        if (powerHit)
        {
            he = Instantiate(instance.hitStarPowerHitPrefab, (Vector3)pos + Vector3.forward * 4f, Quaternion.identity) as HitEffect;
            he.life = life;
            he.scale = 1.5f + life * 0.4f;
        }
        //Instantiate(instance.directionalHitEffect, (Vector3)pos + Vector3.forward * 1f, Quaternion.identity);
    }

    public static void CreateLocalizedShake(Vector2 pos, Vector2 direction, float velocity, float life)
    {
        var ls = Instantiate(instance.localizedShakePrefab, (Vector3)pos - Vector3.forward * 9f, Quaternion.identity) as LocalizedShake;
        ls.life = 0.1f + life * 0.1f;
        ls.velocity = velocity;
        ls.Magnitude = direction;
    }


    public static void CreateTongueHitEffect(Vector2 pos, float life)
    {
        var he = Instantiate(instance.tongueHitEffect, (Vector3)pos - Vector3.forward, Quaternion.identity);

    }

    public static void CreateTrailFader(CharacterAnimator animator, float lifeM, Color color, bool grow = false)
    {
        var traile = Instantiate(instance.faderTrailPrefab, animator.rend.transform.position + Vector3.forward * 3.5f, animator.rend.transform.rotation) as TrailFader;
        traile.transform.localScale = animator.rend.transform.localScale;
        traile.spriteRenderer.material.color = color;
        traile.spriteRenderer.sprite = animator.rend.sprite;
        color.a = 0.5f;
        traile.spriteRenderer.color = color;
        traile.lifeM = 1f + lifeM;
        traile.transform.parent = animator.rend.transform;
        traile.transform.localPosition = Vector3.forward;
        traile.copySpriteRenderer = animator.rend;
        traile.grow = grow;

        if (grow)
            traile.transform.localPosition = traile.transform.localPosition + (Vector3)Random.insideUnitCircle * 0.25f;
    }

    internal static void CreateKnockedUpEffect(CharacterAnimator characterAnimator)
    {
        var kue = Instantiate(instance.knockedUpEffect, characterAnimator.transform.position - Vector3.forward * 5f, Quaternion.identity) as KnockedUpEffect;

        kue.spriteRenderer.color = characterAnimator.rend.color;

    }

    public static void SprayParticles(Vector2 pos, Vector2 direction, float force, int amount, float life, float angle)
    {
        for (int i = 0; i < amount; i++)
        {
            var hp = Instantiate(instance.hitParticlePrefab, pos + UnityEngine.Random.insideUnitCircle, Quaternion.identity) as HitParticle;
            float directionM = ((float)(i + 1f) / (float)(amount + 2f));
            //direction =
            hp.velocity = (Quaternion.Euler(0, 0f, Mathf.Lerp(-angle, angle, directionM)) * ((Vector3)direction) * force);// * Random.Range(0.75f,1.25f);
            hp.life = life * Random.Range(0.9f, 1.1f);
            Vector2 dir = Vector2.one;

        }
    }

    public static void CreateStarParticles(Vector2 pos, Vector2 velocity, float life, int amount, Color[] colors)
    {
        for (int i = 0; i < amount; i++)
        {
            var hp = Instantiate(instance.hitParticlePrefab, pos + UnityEngine.Random.insideUnitCircle, Quaternion.identity) as HitParticle;
            float directionM = ((float)i / (float)amount);
            //direction =
            hp.velocity = velocity * Random.Range(0.75f, 1.25f);
            hp.life = life * Random.Range(0.9f, 1.1f);
            hp.startSize = 0.15f;
            hp.endSize = 0f;
            if (colors != null)
            {
                hp.SetColors(colors);
            }

        }
    }

    public static Color GetRandomColor()
    {
        return instance.randomColors[Random.Range(0, instance.randomColors.Length)];
    }

    internal static void CreateSideScorePlum(Vector3 position, Side killedOnSide, int hitsTaken, Color playerColor)
    {
        Quaternion rotation = Quaternion.identity;
        position.x = Mathf.Clamp(position.x, -31f, 31f);
        position.y = Mathf.Clamp(position.y, -17f, 17f);



        int shakeIntensity = Mathf.Clamp(hitsTaken, 0, int.MaxValue);



        if (killedOnSide == Side.Left)
        {
            position.x = -32;
            ShakeCamera(Vector2.left, hitsTaken);
        }
        else if (killedOnSide == Side.Right)
        {
            position.x = 32f;
            rotation = Quaternion.Euler(0f, 0f, 180f);
            ShakeCamera(Vector2.right, hitsTaken);
        }
        else if (killedOnSide == Side.Top)
        {
            position.y = 18f;
            rotation = Quaternion.Euler(0f, 0f, -90f);
            ShakeCamera(Vector2.up, hitsTaken);
        }
        else if (killedOnSide == Side.Bottom)
        {
            position.y = -18f;
            rotation = Quaternion.Euler(0f, 0f, 90f);
            ShakeCamera(Vector2.down, hitsTaken);
        }

        position.z = -1f;

        var ssp = Instantiate(instance.sideScorePlumPrefab, position, rotation) as SideScorePlum;
        ssp.SetPoints(hitsTaken);
        ssp.transform.parent = Camera.main.transform;
        position.z = 11f;
        ssp.transform.localPosition = position;
        if (hitsTaken > 0)
            ssp.SetText("+" + hitsTaken, playerColor);
        else
        {
            if (Random.value < 0.2f)
                ssp.SetText("OOPS!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("DERP!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("FAIL!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("CRAP!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("SHIT!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("CRUD!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText(":(", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("DARN!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("BUTTS!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("FAIL!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("DRAT!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("OH DEAR!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("OH NO!", playerColor);
            else if (Random.value < 0.2f)
                ssp.SetText("FAREWELL, CRUEL WORLD!", playerColor);
            else
                ssp.SetText("~_~", playerColor);
            // ssp.SetText(hitsTaken.ToString(), playerColor);
        }
    }

    internal static void CreateTurnAroundPuff(Vector3 position, float dir)
    {
        var dp = Instantiate(instance.turnAroundPuff, position + Vector3.forward, Quaternion.identity);
        dp.transform.localScale = new Vector3(dir, 1f, 1f);
    }
}
