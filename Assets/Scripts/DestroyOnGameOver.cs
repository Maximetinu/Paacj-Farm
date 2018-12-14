using UnityEngine;

public class DestroyOnGameOver : MonoBehaviour
{
	public Object objectToDestroy;

	void Start()
	{
		GameController.RegisterOnGameOver(() =>
		{
			Destroy(objectToDestroy);
			Destroy(this);
		});
	}
}
