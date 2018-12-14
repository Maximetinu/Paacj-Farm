using UnityEngine;

public class AudioRangeDrawer : MonoBehaviour
{
	public AudioSource target;
	float initialScale;

	float updateStep = 0.1f;
	int sampleDataLength = 1024;

	float currentUpdateTime = 0f;

	float clipLoudness;
	float[] clipSampleData;

	void Start()
	{
		initialScale = transform.localScale.x;
		transform.localScale = Vector3.zero;

		clipSampleData = new float[sampleDataLength];
	}

	void Update()
	{

		// Update time ?
		currentUpdateTime += Time.deltaTime;
		if (currentUpdateTime >= updateStep)
		{
			// Compute loudness
			currentUpdateTime = 0f;
			target.clip.GetData(clipSampleData, target.timeSamples);
			clipLoudness = 0f;
			foreach (var sample in clipSampleData)
			{
				clipLoudness += Mathf.Abs(sample);
			}
			clipLoudness /= sampleDataLength;

			// Apply scale based on loudness
			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * initialScale, clipLoudness);
		}
	}
}
