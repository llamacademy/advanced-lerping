using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class LerpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Rect;
    [SerializeField]
    private GameObject SecondRect;
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform CenterTarget;
    [SerializeField]
    private Transform EndPoint;
    [SerializeField]
    private TextMeshPro[] Texts;
    [SerializeField]
    private MinMaxCurve Curve;
    [SerializeField]
    [Range(0, 100)]
    private float LerpSpeed = 1f;
    [Range(0, 100)]
    [SerializeField]
    private float MoveSpeed = 2f;

    private Coroutine LerpCoroutine;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 225, 30), "Vector3 Lerp Fixed Time"))
        {
            if (LerpCoroutine != null)
            {
                StopCoroutine(LerpCoroutine);
            }

            LerpCoroutine = StartCoroutine(LerpRectFixedTime());
        }
        if (GUI.Button(new Rect(10, 50, 225, 30), "Vector3 Lerp \"Fixed\" Speed"))
        {
            if (LerpCoroutine != null)
            {
                StopCoroutine(LerpCoroutine);
            }

            LerpCoroutine = StartCoroutine(LerpRectFixedSpeed());
        }
        if (GUI.Button(new Rect(10, 90, 225, 30), "Quaternion Lerp/Slerp Fixed Time"))
        {
            if (LerpCoroutine != null)
            {
                StopCoroutine(LerpCoroutine);
            }

            LerpCoroutine = StartCoroutine(LerpRotationFixedTime());
        }
        if (GUI.Button(new Rect(10, 130, 225, 30), "Lerp Color"))
        {
            if (LerpCoroutine != null)
            {
                StopCoroutine(LerpCoroutine);
            }

            LerpCoroutine = StartCoroutine(LerpColor());
        }
        if (GUI.Button(new Rect(10, 170, 225, 30), "Vector3 Slerp"))
        {
            if (LerpCoroutine != null)
            {
                StopCoroutine(LerpCoroutine);
            }

            LerpCoroutine = StartCoroutine(SlerpRectFixedTime());
        }
    }

    private void DisableSecondaryAndTexts()
    {
        foreach (TextMeshPro text in Texts)
        {
            text.gameObject.SetActive(false);
        }
        SecondRect.gameObject.SetActive(false);
    }

    private IEnumerator LerpRectFixedTime()
    {
        DisableSecondaryAndTexts();
        Rect.transform.rotation = Quaternion.identity;

        while (true)
        {
            float time = 0;
            float random = Random.value;
            while (time < 1)
            {
                Rect.transform.position = Vector3.Lerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(time, random)
                );
                time += Time.deltaTime * LerpSpeed;
                yield return null;
            }

            random = Random.value;
            while (time > 0)
            {
                Rect.transform.position = Vector3.Lerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(time, random)
                );
                time -= Time.deltaTime * LerpSpeed;
                yield return null;
            }
        }
    }

    private IEnumerator SlerpRectFixedTime()
    {
        DisableSecondaryAndTexts();
        Rect.transform.rotation = Quaternion.identity;

        while (true)
        {
            float time = 0;
            float random = Random.value;
            while (time < 1)
            {
                Rect.transform.position = Vector3.Slerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(time, random)
                );
                time += Time.deltaTime * LerpSpeed;
                yield return null;
            }

            random = Random.value;
            while (time > 0)
            {
                Rect.transform.position = Vector3.Slerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(time, random)
                );
                time -= Time.deltaTime * LerpSpeed;
                yield return null;
            }
        }
    }

    private IEnumerator LerpRectFixedSpeed()
    {
        DisableSecondaryAndTexts();
        Rect.transform.rotation = Quaternion.identity;

        while (true)
        {
            float distance = Vector3.Distance(
                StartPoint.position, 
                EndPoint.position
            );
            float remainingDistance = distance;
            float random = Random.value;
            while (remainingDistance > 0)
            {
                Rect.transform.position = Vector3.Lerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(1 - (remainingDistance / distance), random)
                );
                remainingDistance -= MoveSpeed * Time.deltaTime;
                yield return null;
            }

            remainingDistance = distance;
            random = Random.value;
            while (remainingDistance > 0)
            {
                Rect.transform.position = Vector3.Lerp(
                    StartPoint.position, 
                    EndPoint.position, 
                    Curve.Evaluate(remainingDistance / distance, random)
                );
                remainingDistance -= MoveSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    private IEnumerator LerpRotationFixedTime()
    {
        SecondRect.gameObject.SetActive(true);
        Rect.transform.rotation = Quaternion.identity;
        SecondRect.transform.rotation = Quaternion.identity;
        Rect.transform.position = StartPoint.position;
        SecondRect.transform.position = EndPoint.position;

        foreach (TextMeshPro text in Texts)
        {
            text.gameObject.SetActive(true);
        }

        while (true)
        {
            float time = 0;
            float random = Random.value;
            while (time < 1)
            {
                Rect.transform.rotation = Quaternion.Lerp(
                    Quaternion.identity, 
                    Quaternion.Euler(0, 180, 0), 
                    Curve.Evaluate(time, random)
                );
                SecondRect.transform.rotation = Quaternion.Slerp(
                    Quaternion.identity, 
                    Quaternion.Euler(0, 180, 0), 
                    Curve.Evaluate(time, random)
                );
                time += Time.deltaTime * LerpSpeed;
                yield return null;
            }

            time = 0;
            random = Random.value;
            while (time < 1)
            {
                Rect.transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(0, 180, 0), 
                    Quaternion.Euler(0, 360, 0), 
                    Curve.Evaluate(time, random)
                );
                SecondRect.transform.rotation = Quaternion.Slerp(
                    Quaternion.Euler(0, 180, 0), 
                    Quaternion.Euler(0, 360, 0), 
                    Curve.Evaluate(time, random)
                );
                time += Time.deltaTime * LerpSpeed;
                yield return null;
            }
        }
    }

    
    private IEnumerator LerpColor()
    {
        DisableSecondaryAndTexts();
        Rect.transform.position = CenterTarget.position;
        Rect.transform.rotation = Quaternion.identity;

        Mesh mesh = Rect.GetComponent<MeshFilter>().mesh;

        Color[] startColors = new Color[mesh.colors.Length];
        Color[] targetColors = new Color[mesh.colors.Length];
        List<Color> currentColors = new List<Color>();
        for (int i = 0; i < mesh.colors.Length; i++)
        {
            startColors[i] = mesh.colors[i];
            targetColors[i] = Color.black;
        }

        while (true)
        {
            currentColors.Clear();
            mesh.GetColors(currentColors);
            float time = 0;
            float random = Random.value;
            while (time < 1)
            {
                for (int i = 0; i < currentColors.Count; i++)
                {
                    currentColors[i] = Color.Lerp(
                        startColors[i], 
                        targetColors[i], 
                        Curve.Evaluate(time, random)
                    );
                }

                mesh.SetColors(currentColors);

                time += Time.deltaTime * LerpSpeed;
                yield return null;
            }

            time = 1;
            random = Random.value;
            while (time > 0)
            {
                for (int i = 0; i < currentColors.Count; i++)
                {
                    currentColors[i] = Color.Lerp(
                        startColors[i],
                        targetColors[i],
                        Curve.Evaluate(time, random)
                    );
                }

                mesh.SetColors(currentColors);

                time -= Time.deltaTime * LerpSpeed;
                yield return null;
            }
        }
    }
}
