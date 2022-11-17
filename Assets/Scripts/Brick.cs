using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brick : MonoBehaviour , IBrick
{
	public Vector2 boxSize;
	public LayerMask brickLayer;
	private bool isHit;
	
	public int lives = 3;
	
	public bool WillHitNeighboursOnDeath
	{
		get
		{
			return true;
		}
		
	}
	
	public int Lives
	{
		get
		{
			return lives;
		}
		
	}
	
	public IEnumerable<IBrick> Neighbours
	{
		get
		{
			List<IBrick> neigbours = new List<IBrick>();
			
			
			RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0f, Vector2.zero, brickLayer);

			foreach(var hit in hits)
			{
				var neighbour = hit.transform.gameObject.GetComponent<IBrick>();
				neigbours.Add(neighbour);
			}
			return neigbours;
		}
		
	}
	


    // Start is called before the first frame update
    void Start()
    {
		isHit = false;
	
    }

    // Update is called once per frame
    void Update()
    {
		if(lives <= 0){
			Destroy(gameObject);
		}
    }

    public void OnResolveHit()
    {
		//get neigbours
		lives -= 1;
	
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
			isHit = true;
			var bricksToDestroy = BrickResolver.ResolveBricksToDestroy(this);
			if(bricksToDestroy != null){ 
				foreach(var brick in bricksToDestroy){
					Debug.Log($"brickLives: {brick.Lives}");
					brick.OnResolveHit();
				}
				Debug.Log($"er is wat!");
			}
			else{
				Debug.Log($"er is  niks :(!");
			}
        }
		
    }
	
	void OnDrawGizmosSelected(){
		if(isHit){
			Gizmos.color = new Color(1, 0, 0, 0.5f);
			Gizmos.DrawCube(transform.position, new Vector3(boxSize.x, boxSize.y, 0));
		}

	}
	
}
