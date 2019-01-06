using UnityEngine;

public class Loudness : MonoBehaviour
{

    class ClipLoudnessData
    {
        AudioClip thisClip;

        public float maxScaleAmplitude;
        public float lowThreshold, mediumThreshold, highThreshold, perfectThreshold;
        public readonly int loudnessThresholdCount = 4;

        public ClipLoudnessData(AudioClip clip)
        {
            thisClip = clip;

            InitMaxScaleAmplitude();
            InitClipLoudnessThresholds();
        }

        void InitMaxScaleAmplitude()
        {
            const float sampleUmbral = 0.3f;

            float maxAmplitude, averageAmplitude;
            float[] samples;
            float samplesSum;
            int samplesAboveUmbralCount;

            samples = new float[thisClip.samples * thisClip.channels];
            samplesSum = 0f;
            samplesAboveUmbralCount = 0;
            maxAmplitude = 0f;

            thisClip.GetData(samples, 0);

            for (int i = 0; i < samples.Length; ++i)
            {
                // Compute max amplitude
                if (samples[i] > maxAmplitude)
                {
                    maxAmplitude = samples[i];
                }

                // Compute average amplitude
                if (samples[i] > sampleUmbral)
                {
                    samplesSum += samples[i];
                    samplesAboveUmbralCount++;
                }
            }

            if (samplesAboveUmbralCount > 0)
            {
                averageAmplitude = samplesSum / (samplesAboveUmbralCount);
                this.maxScaleAmplitude = averageAmplitude;
            }
            else
            {
                // averageAmplitude = sampleUmbral; // Not needed indeed. But if not, averageAmplitude is NaN
                this.maxScaleAmplitude = maxAmplitude;
            }
        }

        void InitClipLoudnessThresholds()
        {
            float loudnessThresholdSegment;

            loudnessThresholdSegment = this.maxScaleAmplitude / this.loudnessThresholdCount;

            this.lowThreshold = loudnessThresholdSegment * 0.5f;
            this.mediumThreshold = loudnessThresholdSegment * 1.5f;
            this.highThreshold = loudnessThresholdSegment * 2.5f;
            this.perfectThreshold = loudnessThresholdSegment * 3.5f;
        }
    }

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
            if (clipLoudness <= currentClipData.lowThreshold) // 0.1f
            {
                return 1;
            }
            else if (clipLoudness <= currentClipData.mediumThreshold) // 0.3f
            {
                return 2;
            }
            else if (clipLoudness <= currentClipData.highThreshold) // 0.4f
            {
                return 3;
            }
            else if (clipLoudness <= currentClipData.perfectThreshold) // 0.5f
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

    ClipLoudnessData currentClipData;

    void Start()
    {
        initialScale = transform.localScale.x;
        transform.localScale = Vector3.zero;

        clipSampleData = new float[sampleDataLength];

        // Init Clip Loudness Data
        currentClipData = new ClipLoudnessData(target.clip);
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

            if (clipLoudness > clipLoudnessDelayed)
            {
                clipLoudnessDelayed = clipLoudness;
            }

            // Apply feedback directly from loudness
            if (decreaseDelayed == false)
            {
                // Apply scale based on loudness
                transform.localScale = Vector3.Lerp(Vector3.zero, (Vector3.one * initialScale) / currentClipData.maxScaleAmplitude, clipLoudness);

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
            transform.localScale = Vector3.Lerp(Vector3.zero, (Vector3.one * initialScale) / currentClipData.maxScaleAmplitude, clipLoudnessDelayed);

            // Apply animation speed based on loudness
            anim.speed = Mathf.Lerp(0.025f, 8f, clipLoudnessDelayed);
        }
    }
}
