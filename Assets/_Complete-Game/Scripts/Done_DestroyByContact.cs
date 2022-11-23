using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
    public GameObject shieldExplosion;
	public int scoreValue;
	private Done_GameController gameController;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "EnemiesArea" || other.tag == "EnemyBullet" || other.tag == "EnemyCenter")
		{
			return;
		}

        if(other.tag == "PlayerGiantBullet")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            return;
        }

        if(other.tag == "Shield" && transform.tag == "EnemyBullet")
        {
            Instantiate(shieldExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
            CameraShake.Shake(1f, 1f);
            return;
        }

		if (explosion != null)
		{
			Instantiate(explosion, transform.position, transform.rotation);
		}

		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.DiscountLife();
            Destroy(gameObject);
            //gameController.GameOver();
            return;
		}
		
		gameController.AddScore(scoreValue);
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}