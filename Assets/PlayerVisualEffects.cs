using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.VFX;

public class PlayerVisualEffects : MonoBehaviour
{
    public float detectRadiusStart, viewOffset;
    public LayerMask enemyLayer;
    public LayerMask enemyAndObstacleLayer;
    Collider[] cols;

    public bool doingChaseEffect;

    public AudioSource visionSoundSource;
    public AudioClip coilSound, outOfVisionSound, normalEnemyVisionSound;

    private EnemyVisionInfo enemySeen;

    public float lostVisionTime;
    float currentLostvisionTime;

    public float effectCooldown;
    float lastTimeUsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        cols = new Collider[16];
    }

    // Update is called once per frame
    void Update()
    {
        //if (!doingChaseEffect && lastTimeUsed + effectCooldown <= Time.time)
        //{
        //    var hits = Physics.OverlapSphereNonAlloc(transform.position, detectRadiusStart, cols, enemyLayer);
        //    if (hits > 0)
        //    {
        //        foreach (Collider col in cols)
        //        {
        //            if (col == null) continue;
        //            if (col.TryGetComponent(out EnemyVisionInfo visionInfo) && visionInfo.CanSeePlayer && !visionInfo.effectRecently){
        //                Vector3 viewPos = Camera.main.WorldToViewportPoint(col.transform.position);

        //                Physics.Raycast(transform.position, col.transform.position - transform.position, out RaycastHit hit, Mathf.Infinity, enemyAndObstacleLayer);
        //                if (hit.transform.CompareTag("Enemy"))
        //                {

        //                    if (viewPos.x >= viewOffset && viewPos.x <= 1 - viewOffset && viewPos.y >= viewOffset && viewPos.y <= 1 - viewOffset && viewPos.z > 0)
        //                    {
        //                        // Your object is in the range of the camera, you can apply your behaviour
        //                        BeginChaseEffect(visionInfo);
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //if (enemySeen != null)
        //{
        //    if (!enemySeen.CanSeePlayer)
        //    {
        //        currentLostvisionTime += Time.deltaTime;
        //        if (currentLostvisionTime >= lostVisionTime) EndChaseEffect();
        //    }
        //    else currentLostvisionTime = 0;
        //}
    }

    public void BeginChaseEffect(EnemyVisionInfo visionInfo)
    {
        var cooldownTime = lastTimeUsed == 0 ? 0 : lastTimeUsed + effectCooldown;
        if (doingChaseEffect || cooldownTime > Time.time) return;

        doingChaseEffect = true;
        enemySeen = visionInfo;
        visionInfo.StartVisionCooldown(effectCooldown);

        switch (visionInfo.type)
        {
            case EnemyType.wanderer:
                PlaySoundEffect(normalEnemyVisionSound);
                break;
            case EnemyType.coil:
                PlaySoundEffect(coilSound);
                break;
        }

        PostProcessingHandler.Instance.SetChromaticAberration(1f, 0.5f);
        PostProcessingHandler.Instance.SetLensDistortion(1f, -0.15f);
        PostProcessingHandler.Instance.SetVignette(0.2f, 1f);
        Camera.main.GetComponentInParent<CameraController>().IncrementFov(15);
    }

    public void EndChaseEffect(EnemyVisionInfo visionInfo)
    {
        if (visionInfo != enemySeen) return;

        doingChaseEffect = false;
        lastTimeUsed = Time.time;
        enemySeen = null;
        PlaySoundEffect(outOfVisionSound, 0.5f);
        PostProcessingHandler.Instance.SetChromaticAberration(2f, 0);
        PostProcessingHandler.Instance.SetLensDistortion(2f, 0);
        PostProcessingHandler.Instance.ResetVignette(1f);
        Camera.main.GetComponentInParent<CameraController>().IncrementFov(-15);
    }

    void PlaySoundEffect(AudioClip clip, float volume = 0.7f)
    {
        visionSoundSource.PlayOneShot(clip, volume);
    }
}
