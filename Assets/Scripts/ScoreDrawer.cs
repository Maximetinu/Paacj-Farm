using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ScoreDrawer : MonoBehaviour
{
    public Font scoreFont;
    public float fadeDuration = 1f;
    public float verticalDisplacement = 1f;

    private static ScoreDrawer instance;

    public static ScoreDrawer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreDrawer>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ScoreDrawer).Name;
                    instance = obj.AddComponent<ScoreDrawer>();
                }
            }
            return instance;
        }
    }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as ScoreDrawer;
            //DontDestroyOnLoad(this.gameObject); // Do not make it persistent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DrawScore(int scoreGained, Vector3 worldPosition)
    {
        GameObject go;
        Text goText;
        Vector3 worldPositionToFade;
        float verticalDisplacementResponsive;

        go = new GameObject("+" + scoreGained);
        goText = go.AddComponent<Text>();

        goText.text = "+" + scoreGained;
        goText.font = scoreFont;
        goText.fontSize = 50;
        goText.alignment = TextAnchor.MiddleCenter;

        go.transform.SetParent(Instance.transform);
        go.transform.localScale = Vector3.one;

        go.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
        worldPositionToFade = Camera.main.WorldToScreenPoint(worldPosition + Vector3.up * verticalDisplacement);

        verticalDisplacementResponsive = worldPositionToFade.y - go.transform.position.y;


        StartCoroutine(FadeOutAndDestroy(goText, fadeDuration, verticalDisplacementResponsive));
    }

    IEnumerator FadeOutAndDestroy(Text text, float duration, float verticalDisplacement)
    {
        float start = Time.time;

        Color startColor = text.color;
        Color endColor = startColor;
        endColor.a = 0f;

        Vector3 startPosition = text.transform.position;
        Vector3 endPosition = startPosition + verticalDisplacement * Vector3.up;

        float elapsed = 0;

        while (elapsed < duration)
        {
            // calculate how far through we are
            elapsed = Time.time - start;
            float normalisedTime = Mathf.Clamp(elapsed / duration, 0, 1);
            text.color = Color.Lerp(startColor, endColor, normalisedTime);
            text.transform.position = Vector3.Lerp(startPosition, endPosition, normalisedTime);
            // wait for the next frame
            yield return null;
        }
        Destroy(text.gameObject);
    }

}
