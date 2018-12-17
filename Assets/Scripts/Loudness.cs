using UnityEngine;

public class Loudness : MonoBehaviour
{
	public AudioSource target;
	public Animator anim;
	[Range(0.01f, 0.1f)]
	public float updateStep = 0.1f;
	public bool decreaseDelayed = false;
	public float decreaseDelayedSpeed = 1f;
	public int CurrentLoudnessLevel
	{
		get
		{
			if (clipLoudness <= 0.1f)
			{
				return 1;
			}
			else if (clipLoudness <= 0.3f)
			{
				return 2;
			}
			else if (clipLoudness <= 0.4f)
			{
				return 3;
			}
			else if (clipLoudness <= 0.5f)
			{
				return 4;
			}
			else
			{
				return 5;
			}
		}
	}
	float initialScale;

	int sampleDataLength = 1024;

	float currentUpdateTime = 0f;

	float clipLoudness;
	float clipLoudnessDelayed;

	float[] clipSampleData;

	//float maxAmplitude = 0f;

	void Start()
	{
		initialScale = transform.localScale.x;
		transform.localScale = Vector3.zero;

		clipSampleData = new float[sampleDataLength];

		//ComputeMaxAmplitude();
	}

	// Lazy inicialization through hash set? - Or maybe serialized precomputed data?
	//void ComputeMaxAmplitude()
	//{
	//	float[] samples = new float[target.clip.samples * target.clip.channels];
	//	target.clip.GetData(samples, 0);

	//	for (int i = 0; i < samples.Length; ++i)
	//	{
	//		if (samples[i] > maxAmplitude)
	//		{
	//			maxAmplitude = samples[i];
	//		}
	//	}
	//}

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

			if (clipLoudness > clipLoudnessDelayed)
			{
				clipLoudnessDelayed = clipLoudness;
			}

			// Apply feedback directly from loudness
			if (decreaseDelayed == false)
			{
				// Apply scale based on loudness
				transform.localScale = Vector3.Lerp(Vector3.zero, (Vector3.one * initialScale) /* / maxAmplitude */, clipLoudness);

				// Apply animation speed based on loudness
				anim.speed = Mathf.Lerp(0.025f, 8f, clipLoudness);
			}
		}

		if (decreaseDelayed == true)
		{

			if (clipLoudness < clipLoudnessDelayed)
			{
				clipLoudnessDelayed -= Time.deltaTime * decreaseDelayedSpeed;
			}
			// Apply scale based on loudness
			transform.localScale = Vector3.Lerp(Vector3.zero, (Vector3.one * initialScale) /* / maxAmplitude */, clipLoudnessDelayed);

			// Apply animation speed based on loudness
			anim.speed = Mathf.Lerp(0.025f, 8f, clipLoudnessDelayed);
		}
	}
}
