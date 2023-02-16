using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMinion : MinionScript
{
	private static Material whiteMat;
	private static Material defaultMat;
	private static float maxSpawnTimer = 10;
	private static Vector3 startingScale = new Vector3(0.3f, 0.3f, 0.3f);

	private MeshRenderer meshRenderer;

	private void OnEnable()
	{
		if (transform.localScale == startingScale)
		{
			StartCoroutine(SpawnMinion());
			meshRenderer.material = defaultMat;
		}
		else
		{
			meshRenderer.material = whiteMat;
		}
		StartCoroutine(RescaleMinion());
	}

	protected void Awake()
	{
		if (!defaultMat) { defaultMat = Resources.Load<Material>("StarMinionMaterial"); }
		if (!whiteMat) { whiteMat = Resources.Load<Material>("StarMinionMaterial2"); }
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private IEnumerator RescaleMinion(/*int sign = 1*/)
	{
		Vector3 rescaleVector = transform.localScale / 20;
		transform.localScale = Vector3.zero;

		for (int i = 0; i < 20; i++)
		{
			transform.localScale += rescaleVector;
			yield return new WaitForSeconds(0.05f);
		}
	}

	private IEnumerator SpawnMinion()
	{
		yield return new WaitForSeconds(maxSpawnTimer);

		agent.isStopped = true;

		yield return new WaitForSeconds(0.5f);

		Transform trans = BossParentClass.minionObjPool.DequeueObj(true, false).transform.GetChild(0);
		trans.position = transform.position + Vector3.up * 10;
		trans.localScale = transform.localScale / 2;

		trans.parent.gameObject.SetActive(true);
		trans.gameObject.GetComponent<Rigidbody>().velocity = transform.up * 30;

		yield return new WaitForSeconds(0.5f);

		agent.isStopped = false;

		StartCoroutine(SpawnMinion());
	}

	protected override void CollisionFunc(Collision col)
	{
		agent.enabled = true;
	}

	protected void OnDisable()
	{
		agent.enabled = false;
		transform.localScale = startingScale;
	}

	//protected override void Move()
	//{
	//	//print("w");
	//	//Vector3 dir = Vector3.Normalize(playerTransform.position - transform.position);
	//	//dir.y = 0;

	//	//rb.velocity = dir * 100 + Vector3.up * rb.velocity.y;
	//	//       rb.AddForce(dir * 100);
	//	//       rb.AddTorque(dir * 100);

	//	//       Vector3 velocityY = Vector3.up * rb.velocity.y;

	//	//       rb.velocity = Vector3.ClampMagnitude(rb.velocity - velocityY, 100) + velocityY;
	//	//       //rb.velocity = Vector3.ClampMagnitude(rb.velocity, 100);
	//	//}
	//}
}
