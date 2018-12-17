using UnityEngine;
using UnityEngine.EventSystems;

public class Paacj : MonoBehaviour, IPointerDownHandler
{
	public AudioClip bloodEffect;
	public Sprite[] bloods;
	public AudioClip[] paacjs;
	Loudness loud;

	AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource>();
		loud = GetComponentInChildren<Loudness>();

		source.loop = true;
		source.clip = paacjs[Random.Range(0, paacjs.Length)];
		source.pitch = 1 + Random.Range(-0.35f, 0.35f);
		source.Play();

		GameController.PaacjUp();
	}

	public void OnPointerDown(PointerEventData pointerData)
	{
		SpriteRenderer mySpriteRenderer = GetComponent<SpriteRenderer>();

		GameController.PointUp(loud.CurrentLoudnessLevel);
		ScoreDrawer.Instance.DrawScore(loud.CurrentLoudnessLevel, transform.position);

		// Death sound effect
		source.Stop();
		source.pitch = 2.2f;
		source.PlayOneShot(bloodEffect, 0.2f);

		// Draw blood
		mySpriteRenderer.sprite = bloods[Random.Range(0, bloods.Length)];
		mySpriteRenderer.sortingOrder = -99;

		// Randomize blood
		transform.Rotate(Vector3.zero * 90 * Random.Range(0, 4));
		mySpriteRenderer.flipX = (Random.Range(0, 2) == 0);
		mySpriteRenderer.flipY = (Random.Range(0, 2) == 0);
		gameObject.name = "Blood";

		// Destroy unnecessary components
		Destroy(GetComponent<Animator>());
		Destroy(GetComponent<Collider2D>());
		Destroy(source, bloodEffect.length);
		Destroy(transform.GetChild(0).gameObject);
		Destroy(this);

		GameController.PaacjDestroyed();
	}
}
