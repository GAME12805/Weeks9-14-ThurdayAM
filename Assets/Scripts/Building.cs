using Cinemachine;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Transform scaffold;
    public Transform bricks;
    public AnimationCurve scaffoldCurve;
    public float t;
    public ParticleSystem smoke;
    public CinemachineImpulseSource impulse;
    public AnimationCurve impulseCurve;
    public AudioSource audioSource;

    public DestructionManager dm;
    public void DestoryMe(DestructionManager manager)
    {
        dm = manager;
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        smoke.Play();
        
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            Vector3 rot = scaffold.eulerAngles;
            rot.z += -scaffoldCurve.Evaluate(t);
            scaffold.eulerAngles = rot;
            impulse.GenerateImpulse(impulseCurve.Evaluate(t));
            yield return null;
        }
        audioSource.Play();
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            bricks.localScale = Vector3.Lerp(Vector3.one, new Vector3(1, 0, 1), scaffoldCurve.Evaluate(t));
            yield return null;
        }
        scaffold.gameObject.SetActive(false);
        smoke.Stop();
        Destroy(gameObject, 5);
        dm.RemoveBuilding(this);
    }
}
