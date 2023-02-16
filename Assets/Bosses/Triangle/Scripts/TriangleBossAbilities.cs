using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBossAbilities : BossParentClass
{
	[SerializeField]
	private GameObject throwingDoritoObject;	
	
	[SerializeField]
	private GameObject explosiveDoritoObject;

	private TriangleBossManager tManager;

	private PoolingClass throwingDoritosObjPool;

	private PoolingClass explosiveDoritosObjPool;

	private bool stopSpinning;
	
	protected override void StartFunc()
	{
		//	rb = GetComponent<Rigidbody>();
		tManager = GetComponent<TriangleBossManager>();

		throwingDoritosObjPool = new PoolingClass(throwingDoritoObject);

		explosiveDoritosObjPool = new PoolingClass(explosiveDoritoObject);

		stopSpinning = false;
		//StartCoroutine(ThrowingDoritos());
		//StartCoroutine(ShootBullets());
		//StartCoroutine(SpinningDorito());
		
		StartCoroutine(SpawnMinions());
	}

	private void Update()
	{
		transform.Rotate(0.5f * Vector3.up);

		if (abilitiesActive >= maxAbilitiesActive || !inRange) return;

		switch (Random.Range(0, 4))
		{
			case 0:
				StartCoroutine(ShootBullets());
				break;
			case 1:
				StartCoroutine(ThrowingDoritos());
				break;
			case 2:
				StartCoroutine(DroppingTriangles());
				break;
			case 3:
				if(miniBoss)
					StartCoroutine(SpinningDorito(4));
				break;
			default:
				print("Shit gone wrong");
				break;
		}
	}

	private IEnumerator ShootBullets(int waves = 3)
	{
		if (abilitiesActive >= maxAbilitiesActive) yield break;
		abilitiesActive++;

		GameObject obj;

		for (int i = 0; i < waves; i++)
		{
			obj = shootObjPool.DequeueObj();
			obj.transform.position = transform.position + Vector3.up * 5;
			obj.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-100, 100), Random.Range(50, 150), Random.Range(-100, 100));
			yield return new WaitForSeconds(1f);
		}
		abilitiesActive--;
	}

	private IEnumerator ThrowingDoritos(int repeats = 3)
	{
		if (abilitiesActive >= maxAbilitiesActive) yield break;
		abilitiesActive++;

		Rigidbody rb;

		for (int i = 0; i < repeats; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				rb = throwingDoritosObjPool.DequeueObj().GetComponent<Rigidbody>();

				Transform trans = rb.transform;

				trans.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 30 + j * 120, 0);

				trans.position = new Vector3(transform.position.x, floorTransform.position.y, transform.position.z) + trans.forward * 20;
				
				rb.velocity = trans.forward * 200;
			}

			yield return new WaitForSeconds(0.75f);
		}

		abilitiesActive--;
	}

	private IEnumerator DroppingTriangles(int repeats = 6)
	{
		if (abilitiesActive >= maxAbilitiesActive) yield break;
		abilitiesActive++;

		GameObject obj;
		Vector3 pos;
		int maxK = 3;
		int startK = -3;
		float offset;

		for (int i = 0; i < repeats; i++)
		{
			pos = playerTransform.position;

			for(int j = -3; j < 3; j++)
			{
				if (j == 1)
					offset = 20;
				else offset = 0;

				for (int k = startK; k < maxK; k++)
				{
					obj = explosiveDoritosObjPool.DequeueObj();

					obj.transform.position = pos + new Vector3(j * 20, 100, k * 20 - 10 * (j % 2) + offset );

					obj.GetComponent<Rigidbody>().velocity = Vector3.down * 50;
				}
				if (maxK > Mathf.Abs(startK))
					maxK--; // 3, 2 , 1
				else
					startK++; // -3, -2, -1 , 0

				// 3 , -3// -3 . -2 , -1, 0 , 1, 2
				// 3 , -2// -3 . -2 , -1, 0 , 1
				// 2 , -2// -2 , -1, 0 , 1 //-1,0,1,2
				// 2 , -1// -2 , -1, 0
				// 1 , -1// -1 ,0
				// 1 , 0 //  0

			}
			maxK = 3;
			startK = -3;
			yield return new WaitForSeconds(2);
		}
		abilitiesActive--;
	}

	private IEnumerator SpinningDorito(int repeats = 6)
	{
		if (abilitiesActive >= maxAbilitiesActive) yield break;
		abilitiesActive++;

		Vector3 pos;

		for (int i = 0; i < repeats; i++)
		{
			stopSpinning = false;

			pos = playerTransform.position;
			pos.y = transform.position.y;
			pos += Vector3.Normalize(pos - transform.position) * 100f;

			while (Vector3.Distance(transform.position, pos) > 5 && !stopSpinning)
			{
				transform.parent.position = Vector3.MoveTowards(transform.position, pos, 3);

				yield return new WaitForSeconds(0.01f);
			}
			yield return new WaitForSeconds(0.5f);
		}

		abilitiesActive--;
	}

	private IEnumerator SpawnMinions()
	{
		while (!inRange) { yield return new WaitForSeconds(5f); }

		GameObject obj;
		Transform trans;
		Vector3 spawnPosition;
		Vector3 floorPos = floorTransform.position;

		for (int i = 0; i < 3; i++)
		{
			transform.Rotate(0, 120, 0);
			spawnPosition = transform.position + transform.forward * 100;
			spawnPosition.y = floorPos.y + 30;

			for (int j = 0; j < 3; j++)
			{
				obj = minionObjPool.DequeueObj();
				trans = obj.transform;
				trans.Rotate(0, j * 120, 0);
				trans.position = spawnPosition + trans.forward * 30;
			}
		}

		yield return new WaitForSeconds(Random.Range(10, 20));
		StartCoroutine(SpawnMinions());
	}

	//public static new void SetInRange(bool state, bool upperFloor = false)
	//{
	//	if(upperFloor) { inRangeUpper = state; return; }
	//	inRange = state;
	//}

	//public static new bool GetInRange(bool upperFloor = false, bool both = false)
	//{
	//	if (upperFloor)
	//		return inRangeUpper;
	//	else if (both)
	//		return inRangeUpper || inRange;
	//	return inRange;
	//}

	private void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag != "Player") 
			stopSpinning = true;
	}

	public void SetBulletPool(PoolingClass pool)
	{
		shootObjPool = pool;
	}

	public void SetMinionPool(PoolingClass pool)
	{
		minionObjPool = pool;
	}
}
