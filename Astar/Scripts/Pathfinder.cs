using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pathfinder : MonoBehaviour
{

	Node[] map;
	public Transform end;
	public bool bot;

	private float minReachDistance = 0.1f;
	Vector3 velocity = Vector3.zero;
	float acceleration = 12;
	Rigidbody rigidbody;
	bool ladder = false;
	bool jump = false;
	bool water = false;
	private void Start()
	{
		Transform t = GameObject.Find("CheckPoints").transform;
		map = new Node[t.childCount];
		for (int i = 0; i < t.childCount; i++)
		{
			map[i] = new Node(t.GetChild(i).transform.position);
			map[i].idx = i;
			for (int j = 0; j < t.GetChild(i).childCount; j++)
			{
				if (!t.GetChild(i).GetChild(j).transform.name.Contains("ladder")&&!t.GetChild(i).GetChild(j).transform.name.Contains("jumpdir"))
					map[i].Neighbours.Add(int.Parse(t.GetChild(i).GetChild(j).transform.name));
			}
		}
		rigidbody = GetComponent<Rigidbody>();
	}
	Vector3 tmp;
	int endidx = 0;
	int startidx = 0;
	List<Node> path = new List<Node>();
	private void FixedUpdate()
	{
		if (!bot)
		{
			PlayerMove();
			//	PlayerAim();
		}
		else
		{
			MoveIt();
			//	AimShootSightTakeCover();
		}
		//	animateIt(velocity);
		if (Random.Range(0, 2) == 0)
		{
			//		end.position = new Vector3(0, 0, 0);
		}
		else
		{
			//		end.position = new Vector3(0, 0, 0);
			//		if ((transform.position - end.position).sqrMagnitude < 4)
			//			end.position = new Vector3(0, 0, 0);
		}
	}/*
	List<Vector3> waypoints = new List<Vector3>();
	private void OnDrawGizmos()
	{
        for (int i = 0; i < waypoints.Count; i++)
        {
			Gizmos.DrawSphere(waypoints[i],1f);
        }
	}*/
	void TakeCover(Vector3 target, int camefrom)
	{
		List<Node> waypoints = new List<Node>();
		waypoints.Add(map[camefrom]);
		int c = 0;
		for (int i = c; i < waypoints.Count; i++)
		{
			for (int j = 0; j < map[waypoints[i].idx].Neighbours.Count; j++)
			{
				bool p = false;
				for (int k = 0; k < waypoints.Count; k++)
				{
					if (map[waypoints[i].idx].Neighbours[j] == waypoints[k].idx)
					{
						p = true;
						break;
					}
				}
				if (!p && (map[map[waypoints[i].idx].Neighbours[j]].Position - transform.position).sqrMagnitude < 19)
				{
					waypoints.Add(map[map[waypoints[i].idx].Neighbours[j]]);
					c++;
				}
			}
		}
		for (int i = 0; i < waypoints.Count - 1; i++)
		{
			float d_tmp = (waypoints[i].Position - transform.position).sqrMagnitude;
			float d_tmp2 = (waypoints[i + 1].Position - transform.position).sqrMagnitude;
			if (d_tmp > d_tmp2)
			{
				Node tmp = waypoints[i];
				waypoints[i] = waypoints[i + 1];
				waypoints[i + 1] = tmp;
				i = -1;
			}
		}
		int layerMask = 1 << 9;
		//	layerMask = ~layerMask;
		for (int i = 0; i < waypoints.Count; i++)
		{
			Vector3 fromPosition = waypoints[i].Position;
			Vector3 toPosition = target;
			Vector3 direction = toPosition - fromPosition;
			Debug.DrawLine(fromPosition, toPosition, Color.yellow);

			RaycastHit hit;
			if (Physics.Raycast(fromPosition, direction, out hit, direction.magnitude, layerMask))
			{
				//	Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
				//Debug.Log("Did Hit");
				end.position = waypoints[i].Position;
				break;
			}
			else
			{
				//Debug.DrawRay(fromPosition, direction * 1000, Color.white);
				//Debug.Log("Did not Hit");		
			}
		}
	}
	void AimShootSightTakeCover()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("Terrorist");
		float d = 0;
		for (int i = 0; i < go.Length; i++)
		{
			Vector3 relp = go[i].transform.position - transform.position;
			d = (relp).sqrMagnitude;
			//if (d < 2000f && Vector3.Angle(relp, transform.Find("Aim").transform.forward) < 90)
			{
				if (Sight(go[i].transform.position))
				{
					Aim(go[i].transform.position);
					Shoot(go[i].transform.position);
					TakeCover(go[i].transform.position, startidx);
					//Debug.DrawRay(transform.Find("Aim").transform.position, transform.Find("Rig_Aim_Layer").transform.Find("Aim").transform.forward, Color.red);
				}
				TakeCover(go[i].transform.position, startidx);

			}
		}

	}
	int currpath = 0;
	float JumpTime = 0;
	bool dojump = false;
	Vector3 ljump = Vector3.zero;
	void MoveIt()
	{
		if (!end.position.Equals(tmp))
		{
			float mindist = 1000;
			float minend_dist = 1000;
			List<Node> wpts = new List<Node>();
			List<Node> wpts_end = new List<Node>();
			for (int i = 0; i < map.Length; i++)
			{
				int layerMask = 1 << 9;
				//	layerMask = ~layerMask;
				Vector3 fromPosition = transform.position;
				Vector3 toPosition = map[i].Position;
				Vector3 direction = toPosition - fromPosition;

				RaycastHit hit;
				if (Physics.Raycast(fromPosition, direction, out hit, direction.magnitude, layerMask))
				{
					//Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
					//Debug.Log("Did Hit");
				}
				else
				{
					//Debug.DrawRay(fromPosition, direction * 1000, Color.white);
					//Debug.Log("Did not Hit");
					wpts.Add(map[i]);
				}
				fromPosition = end.position;
				direction = toPosition - end.position;
				if (Physics.Raycast(fromPosition, direction, out hit, direction.magnitude, layerMask))
				{
					//Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
					//Debug.Log("Did Hit");
				}
				else
				{
					//	Debug.DrawRay(fromPosition, direction * 1000, Color.white);
					//Debug.Log("Did not Hit");
					wpts_end.Add(map[i]);
				}
			}
			for (int i = 0; i < wpts.Count; i++)
			{
				float magn = (transform.position - wpts[i].Position).magnitude;
				if (mindist > magn)
				{
					mindist = magn;
					startidx = wpts[i].idx;
				}
			}
			for (int i = 0; i < wpts_end.Count; i++)
			{
				float magn_end = (end.position - wpts_end[i].Position).magnitude;
				if (minend_dist > magn_end)
				{
					minend_dist = magn_end;
					endidx = wpts_end[i].idx;
				}
			}
			tmp = end.position;
			path = AStar.Search(map, map[startidx], map[endidx]);
			currpath = 0;
			jump = false;
		}
		if (path.Count > 0 && currpath < path.Count)
		{
			velocity += (path[currpath].Position - transform.position).normalized * Time.deltaTime * acceleration;

			Debug.DrawLine(path[currpath].Position, transform.position);

			if (currpath < path.Count - 1)
			{
				Transform tr = GameObject.Find("CheckPoints").transform;
				Transform tr2 = tr.GetChild(path[currpath].idx).GetChild(tr.GetChild(path[currpath].idx).childCount - 1);
				if (tr2.name.Equals("ladder") && (path[currpath + 1].idx).ToString().Equals(tr2.GetChild(0).name))
				{
					velocity = (path[currpath].Position - transform.position).normalized * Time.deltaTime * acceleration;
					//velocity *= 0.95f;	
				}
				if (tr2.name.Equals("jumpdir") && (path[currpath + 1].idx).ToString().Equals(tr2.GetChild(0).name))
				{
					velocity = (path[currpath].Position - transform.position).normalized * Time.deltaTime * acceleration;
					//velocity *= 0.95f;	
					jump = true;
				}
			}
			if (ladder || water)
			{
				velocity.x = Mathf.Clamp(velocity.x, -2, 2);
				velocity.y = Mathf.Clamp(velocity.y, -2, 2);
				velocity.z = Mathf.Clamp(velocity.z, -2, 2);
			}
			else
			{
				velocity.y = 0f;
			}
			float sqrDistance = Vector3.Magnitude(transform.position - path[currpath].Position);
			if (sqrDistance <= minReachDistance)
			{
				currpath++;
				if (jump)
				{
					Transform tr = GameObject.Find("CheckPoints").transform.Find("" + path[currpath - 1].idx).transform;
					ljump = (tr.GetChild(tr.childCount - 1).transform.position - path[currpath - 1].Position).normalized;
					JumpTime = Time.time + 0.2f;
					dojump = true;
					//velocity += l * 5;
					velocity = ljump * 0;
					//	rigidbody.velocity = ljump*1;
					rigidbody.velocity *= 0;
				}
			}
			if (dojump)
			{
				velocity *= 0;
				rigidbody.AddForce(ljump * 2000);
				if (JumpTime < Time.time)
				{
					jump = false;
					dojump = false;
				}
			}
			transform.position += velocity * Time.deltaTime;
			velocity *= 0.75f;
		}
	}
	void PlayerMove()
	{
		float xspeed = 2;
		if (transform.eulerAngles.y == 0)
			transform.position += transform.right * Input.GetAxis("Horizontal") * xspeed * Time.deltaTime;
		else
			transform.position += -transform.right * Input.GetAxis("Horizontal") * xspeed * Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.Space) && !dojump)
		{
			Jump();
		}
		if (dojump)
		{
			rigidbody.AddForce(new Vector3(0f, 2000, 0));
			if (JumpTime < Time.time)
			{
				dojump = false;
			}
		}
		transform.position += velocity * Time.deltaTime;
		velocity *= 0.75f;
	}
	void animateIt(Vector3 v)
	{
		float tmp = 0;
		float vert = Input.GetAxis("Horizontal");
		/*	if (vert > 0)
				tmp = xspeedf;
			else
				tmp = xspeedb;
			float xs = vert * tmp;

		*/
		Vector3 vecz = transform.TransformDirection(Vector3.forward);
		Vector3 vecy = transform.TransformDirection(Vector3.up);
		Vector3 vecx = transform.TransformDirection(Vector3.right);
		float co_x = Vector3.Dot(v, vecx);
		float co_y = Vector3.Dot(v, vecy);
		float co_z = Vector3.Dot(v, vecz);

		//	Debug.DrawRay(Vector3.zero,new Vector3(co_x,co_y,co_z));
		//if (isgrounded)
		{
			//		animator.SetFloat("Speed", xs, 0.1f, Time.deltaTime);
			//	animator.SetFloat("Strafe", Input.GetAxis("Horizontal") * yspeed, 0.1f, Time.deltaTime);
			//	animator.SetBool("Crouch", isCrouch);
			//	animator.SetBool("Crawl", isCrawl);
		}

	}
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
	float xSpeed = 20.0f;
	float ySpeed = 20.0f;
	float zSpeed = 20.0f;
	float xMinLimit = -360f;
	float xMaxLimit = 360f;
	float yMinLimit = -90;
	float yMaxLimit = 90;
	float zMinLimit = -360;
	float zMaxLimit = 360;
	float velocityX = 0.0f;
	float velocityY = 0.0f;
	float velocityZ = 0.0f;
	float delayc = 0;
	bool rotateTarget = true;

	bool tps = false;
	void PlayerAim()
	{
		Cursor.visible = false;
		Vector3 wp = cam.ScreenToWorldPoint(Input.mousePosition);
		wp.z = -1;
		//transform.Find("Aim").transform.Find("TargetAim").transform.position = wp; 
		GameObject.Find("AimReticle").transform.position = wp;
		Vector3 relp = wp - transform.Find("Aim").transform.position;
		Quaternion zQuaternion = Quaternion.FromToRotation(Vector3.right, relp.normalized);
		//transform.Find("Aim").transform.localRotation=q;
		//velocityX += xSpeed * Input.GetAxis("Vertical");
		//velocityY += ySpeed * Input.GetAxis("Vertical");
		//velocityZ += zSpeed * Input.GetAxis("Vertical");
		//velocityZ+=zSpeed* Input.GetAxis("Mouse X");
		//velocityX = ClampAngle(velocityX, xMinLimit, xMaxLimit);
		//velocityY = ClampAngle(velocityY, yMinLimit, yMaxLimit);
		//velocityZ = ClampAngle(velocityZ, zMinLimit, zMaxLimit);

		//Quaternion xQuaternion = Quaternion.AngleAxis(velocityX, Vector3.up);
		//Quaternion yQuaternion = Quaternion.AngleAxis(velocityY, -Vector3.right);
		//Quaternion zQuaternion = Quaternion.AngleAxis(velocityZ, Vector3.forward);
		//Quaternion rotation = xQuaternion * yQuaternion * zQuaternion;

		//transform.Find("Aim").transform.localRotation = zQuaternion;
		if (wp.x - transform.position.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
			transform.Find("Aim").transform.localRotation = zQuaternion;
		}
		else
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
			relp = wp - transform.Find("Aim").transform.position;
			relp = new Vector3(relp.x, relp.y * -1, relp.z);
			zQuaternion = Quaternion.FromToRotation(Vector3.right, relp.normalized);

			zQuaternion = Quaternion.AngleAxis(180, Vector3.right) * Quaternion.AngleAxis(180, -Vector3.up) * zQuaternion;
			transform.Find("Aim").transform.localRotation = zQuaternion;
		}
	}
	void Aim(Vector3 p)
	{
		float rotationEasing = 0.2f;
		//transform.Find("Aim").transform.Find("TargetAim").transform.position = wp; 
		//GameObject.Find("Bot_Reticle").transform.position = p;
		Vector3 relp = p - transform.Find("Aim").transform.position;
		Quaternion zQuaternion = Quaternion.FromToRotation(Vector3.right, relp.normalized);
		Quaternion zQuaternion2 = transform.Find("Aim").transform.localRotation;
		//Quaternion zQuaternion = Quaternion.Lerp(transform.Find("Aim").transform.localRotation, Quaternion.Euler(lc.eulerAngles.x, lc.eulerAngles.y, /*Mathf.Clamp(*/q2.eulerAngles.z/*, -90, 90)*/), Time.deltaTime * rotationEasing);
		zQuaternion2.eulerAngles = new Vector3(zQuaternion2.eulerAngles.x + (zQuaternion.eulerAngles.x - zQuaternion2.eulerAngles.x) * Time.deltaTime * rotationEasing,
			zQuaternion2.eulerAngles.y + (zQuaternion.eulerAngles.y - zQuaternion2.eulerAngles.y) * Time.deltaTime * rotationEasing,
			zQuaternion2.eulerAngles.z + (zQuaternion.eulerAngles.z - zQuaternion2.eulerAngles.z) * Time.deltaTime * rotationEasing);
		if (p.x - transform.position.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
			transform.Find("Aim").transform.localRotation = zQuaternion;
		}
		else
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
			relp = p - transform.Find("Aim").transform.position;
			relp = new Vector3(relp.x, relp.y * -1, relp.z);
			zQuaternion = Quaternion.FromToRotation(Vector3.right, relp.normalized);

			zQuaternion = Quaternion.AngleAxis(180, Vector3.right) * Quaternion.AngleAxis(180, -Vector3.up) * zQuaternion;
			transform.Find("Aim").transform.localRotation = zQuaternion;
		}
	}
	public Transform hud;
	public Camera cam;
	int yi;
	int xi;
	Transform Glock;
	float GlockReloadTime;
	float GlockShootTime;
	float GlockAmmo = 2000000000;
	float CurGlockAmmo = 2000000000;
	float Health = 100;
	float ShootTime = 0;
	void Shoot(Vector3 t)
	{
		Transform aim = transform.Find("Aim").transform;
		float angle = Vector3.Angle(t - aim.position, aim.Find("TargetAim").transform.position - aim.position);
		if (angle < 1f)
		{
			if (ShootTime < Time.time && xi == 0 && yi == 0 && CurGlockAmmo > 0)
			{
				ShootTime = Time.time + 1f;
				//	animator.SetBool("Shoot", true);
				//	if (!Gun_Shoot.isPlaying)
				//	Gun_Shoot.Play();
				int layerMask = 1 << 8;
				layerMask = ~layerMask;
				Vector3 fromPosition = transform.Find("Aim").transform.position;
				Vector3 toPosition = transform.Find("Aim").transform.Find("TargetAim").transform.position;
				Vector3 direction = toPosition - fromPosition;

				//	var bullet_tracer = Instantiate(Bullet_Trail, fromPosition, Quaternion.identity);
				//	bullet_tracer.AddPosition(fromPosition);
				CurGlockAmmo--;
				RaycastHit hit;
				if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, layerMask))
				{
					//Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
					//Debug.Log("Did Hit");
					//	HitEffect.transform.position = hit.transform.position;
					//		HitEffect.transform.forward = hit.normal;
					//	HitEffect.Emit(10);
					//		bullet_tracer.transform.position = hit.point;
				}
				else
				{
					//Debug.DrawRay(fromPosition, direction * 1000, Color.white);
					//Debug.Log("Did not Hit");
				}
			}
		}
		else
		{
			//animator.SetBool("Shoot", false);
			//Gun_Shoot.Stop();
		}
	}
	void Jump()
	{
		int layerMask = 1 << 9;
		//	layerMask = ~layerMask;
		Vector3 fromPosition = transform.position;
		Vector3 toPosition = transform.Find("Jump").transform.Find("Jump (1)").transform.position;
		Vector3 direction = toPosition - fromPosition;

		RaycastHit hit;
		if (Physics.Raycast(fromPosition, transform.position - Vector3.up - fromPosition, out hit, 0.3f, layerMask) || Physics.Raycast(fromPosition, transform.Find("Jump").transform.Find("Jump (2)").transform.position - fromPosition, out hit, 0.3f, layerMask) || Physics.Raycast(fromPosition, direction, out hit, 0.3f, layerMask))
		{
			//	Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
			//Debug.Log("Did Hit");
			dojump = true;
			JumpTime = Time.time + 0.2f;
		}
		else
		{
			//	Debug.DrawRay(fromPosition, direction * 1000, Color.white);
			//Debug.Log("Did not Hit");
		}

	}
	bool Sight(Vector3 t)
	{
		int layerMask = 1 << 9;
		//	layerMask = ~layerMask;
		Vector3 fromPosition = transform.Find("Aim").transform.position;
		Vector3 toPosition = t;
		Vector3 direction = toPosition - fromPosition;

		RaycastHit hit;
		if (Physics.Raycast(fromPosition, direction, out hit, direction.magnitude, layerMask))
		{
			//Debug.DrawRay(fromPosition, direction * hit.distance, Color.yellow);
			//Debug.Log("Did Hit");
		}
		else
		{
			//Debug.DrawRay(fromPosition, direction * 1000, Color.white);
			//Debug.Log("Did not Hit");
			return true;
		}
		return false;
	}
	private void OnTriggerEnter(Collider other)
	{

	}
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.tag.Equals("Ladder"))
		{
			Vector3 o = transform.position - other.transform.Find("LadderDown").transform.position;
			Vector3 l = other.transform.Find("LadderTop").transform.position - other.transform.Find("LadderDown").transform.position;

			//	transform.position = other.transform.Find("LadderDown").transform.position + (Vector3.Dot(o, l) / Vector3.Dot(l, l)) * l;
			/*if ((Vector3.Dot(o, l) / Vector3.Dot(l, l)) < 0)
				transform.position = new Vector3(transform.position.x,other.transform.Find("LadderDown").transform.position.y,transform.position.z);
			else*/
			if ((Vector3.Dot(o, l) / Vector3.Dot(l, l)) > 1)
				transform.position = new Vector3(transform.position.x, other.transform.Find("LadderTop").transform.position.y, transform.position.z);

			this.GetComponent<Rigidbody>().isKinematic = true;
			transform.position += l.normalized * Input.GetAxis("Vertical") * Time.deltaTime;
			ladder = true;
		}
		else if (other.transform.tag.Equals("Water"))
		{
			//velocity += Vector3.up * 150 * Time.deltaTime;
			velocity += Vector3.up * 15 * Input.GetAxis("Vertical") * Time.deltaTime;
			rigidbody.AddForce(Vector3.up * 800);
			water = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.tag.Equals("Ladder"))
		{
			this.GetComponent<Rigidbody>().isKinematic = false;
			ladder = false;
		}
		if (other.transform.tag.Equals("Water"))
		{
			water = false;
		}
	}
}
