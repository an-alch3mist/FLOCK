using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_BOID_0 : MonoBehaviour
{

	//// .................. ////

	private void Update()
	{
		//
		if (Input.GetMouseButtonDown(1))
		{
			StopAllCoroutines();
			StartCoroutine(STIMULATE());
		}
		//
	}




	/* TODO 
	
	swarn change of weights and radius over time


	swarn rest reached safe zone

	*/


	[Range(0.05f, 0.5f)]
	public float radius_algn = 0.20f,
				 radius_sepr = 0.22f,
				 radius_attr = 0.25f;

	[Range(0.05f, 1f)]
	public float weight_algn = 0.30f,
				 weight_sepr = 0.89f,
				 weight_attr = 0.90f;




	IEnumerator STIMULATE()
	{
		#region frame_rate
		QualitySettings.vSyncCount = 2;

		yield return null;
		yield return null;
		#endregion


		OBJ.INITIALIZE_CLEAR_HOLDER();


		BOID[] boids = new BOID[350];
		OBJ_S[] obj_s_boids = new OBJ_S[boids.Length];



		#region obj_boid_pred , pred_pos

		Vector2 pred_pos = Vector2.zero;
		OBJ_S obj_boid_pred = new OBJ_S("boid_pred");
		obj_boid_pred.enable(true);
		obj_boid_pred.sprite("boid_0");
		obj_boid_pred.SCALE(0.3f, 0.3f); 
		#endregion






		for (int i = 0; i < boids.Length; i += 1)
		{
			boids[i] = new BOID();
			boids[i].vel = new Vector2()
			{
				x = Random.Range(-0.5f, 0.5f),
				y = Random.Range(-0.5f, 0.5f)
			};

			//
			boids[i].pos = new Vector2()
			{
				x = Random.Range(-3f, 3f),
				y = Random.Range(-3f, 3f)
			};

		}


		#region obj_s_boids_radius

		OBJ_S boid_rad_algn = new OBJ_S("rad_algn", -1);
		OBJ_S boid_rad_attr = new OBJ_S("rad_attr", -2);
		OBJ_S boid_rad_sepr = new OBJ_S("rad_sepr", -3);
		boid_rad_algn.sprite("boid_4_algn_circle");
		boid_rad_attr.sprite("boid_5_attr_circle");
		boid_rad_sepr.sprite("boid_6_sepr_circle");


		boid_rad_algn.enable(true);
		boid_rad_attr.enable(true);
		boid_rad_sepr.enable(true);

		#endregion


		#region obj_s_boids

		for (int i = 0; i < obj_s_boids.Length; i += 1)
		{
			obj_s_boids[i] = new OBJ_S("boid_" + i.ToString());


			int index = Random.Range(0, 100);
				 if(index < 020) { obj_s_boids[i].sprite("boid_0");    boids[i].type = 0; }
			else if(index < 050) { obj_s_boids[i].sprite("boid_1");    boids[i].type = 1; }
			else if(index < 080) { obj_s_boids[i].sprite("boid_2");    boids[i].type = 2; }
			else if(index < 100) { obj_s_boids[i].sprite("boid_3");    boids[i].type = 3; }

			obj_s_boids[i].enable(true);


		}

		#endregion


		#region obj_s_boids

		for (int i = 0; i < obj_s_boids.Length; i += 1)
		{
			obj_s_boids[i].align(boids[i].pos, boids[i].pos + boids[i].vel , scale : false);
			obj_s_boids[i].SCALE(0.1f , 0.1f);
		}

		#endregion



		float dt = Time.deltaTime;

		while(true)
		{


			#region input_predator_pos
			Vector2 prev_pred_pos = pred_pos;

			float pred_vel = 1f;

			if (Input.GetKey(KeyCode.DownArrow))	pred_pos -= Vector2.up		* pred_vel * dt;
			if (Input.GetKey(KeyCode.UpArrow))		pred_pos += Vector2.up		* pred_vel * dt;
			if (Input.GetKey(KeyCode.LeftArrow))	pred_pos -= Vector2.right	* pred_vel * dt;
			if (Input.GetKey(KeyCode.RightArrow))	pred_pos += Vector2.right	* pred_vel * dt;

			//
			if (prev_pred_pos != pred_pos)
				obj_boid_pred.align(pred_pos, pred_pos + pred_pos - prev_pred_pos);

			#endregion



			//
			for (int i = 0; i < boids.Length; i += 1)
			{
				boids[i].accumulated_vel = Vector2.zero;

				boids[i].accumulated_vel += boid_align   (boids, boids[i], radius_algn) * weight_algn;
				boids[i].accumulated_vel += boid_attract (boids, boids[i], radius_attr) * weight_attr;
				boids[i].accumulated_vel += boid_seperate(boids, boids[i], radius_sepr) * weight_sepr;
				boids[i].accumulated_vel -= boid_point_force(boids[i], pred_pos, radius_sepr * 2) * 1000f;

			}


			//
			for (int i = 0; i < boids.Length; i += 1)
			{
				boids[i].pos += boids[i].vel * dt;


				float w = 3f;
				if (boids[i].pos.x > +(w + 1f / 10)  ) { boids[i].pos.x = -w; }
				if (boids[i].pos.x < -(w + 1f / 10 ) ) { boids[i].pos.x = +w; }
				if (boids[i].pos.y > +(w + 1f / 10 ) ) { boids[i].pos.y = -w; }
				if (boids[i].pos.y < -(w + 1f / 10 ) ) { boids[i].pos.y = +w; }



				float const_vel = 0.5f;


				float m_f = 0.00f,
					  M_f = 0.10f;


				#region steer
				Vector2 steer = (boids[i].accumulated_vel - boids[i].vel);
				float steer_mag = steer.magnitude;
				if (steer_mag != 0f)
					steer = steer / steer_mag * Z.clamp(steer_mag, m_f, M_f);
				#endregion


				boids[i].vel = (boids[i].vel + steer).normalized * const_vel;
			}


			//
			#region obj_s_boids

			for (int i = 0; i < obj_s_boids.Length; i += 1)
			{
				obj_s_boids[i].align(boids[i].pos, boids[i].pos + boids[i].vel, scale: false);

				#region obj_s_boids_radius
				if (i == 0)
				{
					boid_rad_algn.POS(boids[i].pos);
					boid_rad_attr.POS(boids[i].pos);
					boid_rad_sepr.POS(boids[i].pos);

					boid_rad_algn.SCALE(radius_algn * 2, radius_algn * 2);
					boid_rad_attr.SCALE(radius_attr * 2, radius_attr * 2);
					boid_rad_sepr.SCALE(radius_sepr * 2, radius_sepr * 2);
				}
				#endregion
			}

			#endregion


			yield return null;
		}

	}
	//// .................. ////





	public class BOID
	{
		public Vector2 pos;
		public Vector2 vel;
		public Vector2 accumulated_vel;

		public int type = 0;
	}


	#region boid_methods

	// align
	// attract
	// repel

	public static Vector2 boid_align(BOID[] boids, BOID boid, float radius)
	{
		bool found_one = false;
		Vector2 sum = Vector2.zero;

		//
		for (int i = 0; i < boids.Length; i += 1)
		{
			if (boid != boids[i])
				if (boids[i].type == boid.type)
				if (Z.sqr_mag(boids[i].pos - boid.pos) <= radius * radius)
				{
					sum += boids[i].vel;
					found_one = true;
				}

		}


		if (found_one)
			return sum.normalized;
		else
			return Vector3.zero;

	}


	public static Vector2 boid_attract(BOID[] boids, BOID boid, float radius)
	{
		bool found_one = false;
		Vector2 sum = Vector2.zero;

		//
		for (int i = 0; i < boids.Length; i += 1)
		{
			if (boid != boids[i])
				if (boids[i].type == boid.type)
				if (Z.sqr_mag(boids[i].pos - boid.pos) <= radius * radius)
				{
					sum += (boids[i].pos - boid.pos);
					found_one = true;
				}

		}


		if (found_one)
			return sum .normalized;
		else
			return Vector3.zero;

	}

	public static Vector2 boid_seperate(BOID[] boids, BOID boid, float radius)
	{
		bool found_one = false;
		Vector2 sum = Vector2.zero;

		//
		for (int i = 0; i < boids.Length; i += 1)
		{
			if (boid != boids[i])
				if (Z.sqr_mag(boids[i].pos - boid.pos) <= radius * radius)
				{
					//
					float dist = Z.mag(boids[i].pos - boid.pos);
					if (dist == 0f) dist = 1f / 1000;

					//
					sum += (boid.pos - boids[i].pos) / dist * 
						   ( (boids[i].type != boid.type)? 2 : 1 );


					found_one = true;
				}

		}


		if (found_one)
			return sum.normalized;
		else
			return Vector3.zero;

	}





	public static Vector2 boid_point_force(BOID boid, Vector2 o  , float radius)
	{
		float dist = Z.sqr_mag(boid.pos - o);

		if (dist < radius * radius)
			return (o - boid.pos).normalized / dist;

		return Vector2.zero;
	}

	#endregion




	 





	#region TOOL , OBJ , MESH

	#region TOOL

	#region Z
	public static class Z
	{
		#region lerp
		public static Vector3 lerp(Vector3 a, Vector3 b, float t)
		{
			Vector3 n = b - a;
			return a + n * t;
		}

		public static float lerp(float a, float b, float t)
		{
			float n = b - a;
			return a + n * t;
		}
		#endregion

		#region dot
		public static float dot(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}
		#endregion

		#region area
		public static float area(Vector2 a, Vector2 b)
		{
			return a.x * b.y - a.y * b.x;
		}
		#endregion

		#region angle
		public static float angle(Vector2 n0, Vector2 n1)
		{
			float X = Z.dot(n0, n1);
			float Y = Z.area(n0, n1);

			float a = Mathf.Atan2(Y, X);
			if (a < 0f)
				a += 2 * C.PI;

			return a;
		}
		#endregion


		#region polar
		public static Vector2 polar(float angle)
		{
			return new Vector2()
			{
				x = Mathf.Cos(angle),
				y = Mathf.Sin(angle)
			};
		}
		#endregion



		//
		#region clamp
		public static float clamp(float v, float m, float M)
		{
			if (v < m) return m;
			if (v > M) return M;
			return v;
		} 
		#endregion

		#region sqr_mag
		public static float sqr_mag(Vector2 v)
		{
			return Z.dot(v, v);
		}
		#endregion

		#region mag
		public static float mag(Vector3 v)
		{
			return Mathf.Sqrt(Z.dot(v, v));
		}
		#endregion
	}

	#endregion


	#region C
	public static class C
	{
		//
		public static float PI = Mathf.PI;
		public static float de = 1f / 1000000;

		public static float ease(float x, int ease_type = 0)
		{
			if (x <= 0f) return 0f;
			if (x >= 1f) return 1f;



			//
			if (ease_type == 0)
			{
				float Y = Mathf.Cos(x * C.PI);
				return (-Y + 1) / 2f;
			}
			else if (ease_type == 1)
			{
				return 1 - (x - 1) * (x - 1) * (x - 1) * (x - 1);
			}

			return x;
		}

		public static float EASE(int i, int frames, int ease_type = 0)
		{
			return ease(i * 1f / (frames - 1), ease_type);
		}



		public static IEnumerator wait
		{
			get
			{
				// capture //
				return null;
			}
		}

		public static IEnumerator wait_(int frames)
		{
			for (int i = 0; i < frames; i += 1)
				yield return C.wait;
		}


	}
	#endregion

	#endregion


	#region OBJ

	public class OBJ
	{
		#region HOLDER

		public static Transform holder;
		public static int holder_count = 0;

		public static void INITIALIZE_CLEAR_HOLDER()
		{
			//
			holder_count = 0;


			string name = "__create";
			//
			if (GameObject.Find(name) == null)
			{
				holder = new GameObject(name).transform;
			}
			//

			else
			{

				holder = GameObject.Find(name).transform;
				GameObject[] GO_1D = new GameObject[holder.childCount];
				for (int i = 0; i < holder.childCount; i += 1) GO_1D[i] = holder.GetChild(i).gameObject;
				for (int i = 0; i < GO_1D.Length; i += 1) GameObject.Destroy(GO_1D[i]);
			}
			//
		}

		#endregion



		MeshFilter mf;
		MeshRenderer mr;
		public GameObject G;

		public OBJ(string name, int layer = 0)
		{
			G = new GameObject(name + "--" + OBJ.holder_count.ToString());

			G.transform.parent = OBJ.holder;
			OBJ.holder_count += 1;


			mf = G.AddComponent<MeshFilter>();
			mr = G.AddComponent<MeshRenderer>();
			mr.sharedMaterial = new Material(Shader.Find("Unlit/Transparent"));

			G.transform.parent = OBJ.holder;
			G.transform.position = new Vector3(0f, 0f, -layer * 1f / 1000);
		}


		#region CONTROLS
		public void enable(bool need_to_enable) { G.SetActive(need_to_enable); }

		public void POS(Vector2 pos) { G.transform.position = new Vector3(pos.x, pos.y, G.transform.position.z); }

		public void ROT(float angle) { G.transform.localEulerAngles = new Vector3(0f, 0f, angle / C.PI * 180); }

		public void SCALE(float x, float y) { G.transform.localScale = new Vector3(x, y, 1f); }

		public void align(Vector2 a, Vector2 b, bool scale = false)
		{

			POS(a);
			ROT(Z.angle(Vector2.right, b - a));
			//
			if (scale)
			{
				float mag = Z.mag(b - a);
				SCALE(mag, mag);
			}
		}   //



		public void mesh(Mesh mesh) { mf.sharedMesh = mesh; }

		public void tex2D(Texture2D tex2D) { mr.sharedMaterial.mainTexture = tex2D; }

		#endregion


	}

	public class OBJ_S
	{
		SpriteRenderer sr;
		public GameObject G;

		public OBJ_S(string name, int layer = 0)
		{
			G = new GameObject(name + "--" + OBJ.holder_count.ToString());

			G.transform.parent = OBJ.holder;
			OBJ.holder_count += 1;
		

			sr = G.AddComponent<SpriteRenderer>();
			G.transform.position = new Vector3(0f, 0f, -layer * 1f / 1000);

		}


		#region CONTROLS
		public void enable(bool need_to_enable) { G.SetActive(need_to_enable); }

		public void POS(Vector2 pos) { G.transform.position = new Vector3(pos.x, pos.y, G.transform.position.z); }

		public void ROT(float angle) { G.transform.localEulerAngles = new Vector3(0f, 0f, angle / C.PI * 180); }

		public void SCALE(float x, float y) { G.transform.localScale = new Vector3(x, y, 1f); }

		public void align(Vector2 a, Vector2 b, bool scale = false)
		{

			POS(a);
			ROT(Z.angle(Vector2.right, b - a));
			//
			if (scale)
			{
				float mag = Z.mag(b - a);
				SCALE(mag, mag);
			}
		}   //

		public void sprite(string location)
		{
			sr.sprite = Resources.Load<Sprite>(location);
		}

		#endregion


	}

	#endregion




	#region MESH
	public static class MESH
	{
		// sx = width , sy = height
		public static Mesh quad(float sx, float sy)
		{

			Mesh mesh = new Mesh()
			{
				vertices = new Vector3[4]
				{
					new Vector3(-sx , -sy ) / 2,
					new Vector3(-sx , +sy ) / 2,
					new Vector3(+sx , +sy ) / 2,
					new Vector3(+sx , -sy ) / 2,
				},

				triangles = new int[2 * 3]
				{
					0 , 1 , 2,
					0 , 2 , 3
				},

				uv = new Vector2[4]
				{
					new Vector2(0 , 0),
					new Vector2(0 , 1),
					new Vector2(1 , 1),
					new Vector2(1 , 0),
				}

			};

			mesh.RecalculateNormals();
			return mesh;
		}

	}

	#endregion

	#region DRAW
	public class DRAW
	{

		public static float dt = Time.deltaTime;
		public static Color col = Color.red;



		#region LINE
		public static void LINE(Vector2 a, Vector2 b, float e = 1f / 50)
		{
			Vector2 nX = b - a,
					nY = new Vector2(-nX.y, nX.x).normalized;

			Debug.DrawLine(a + nY * e, b + nY * e, DRAW.col, DRAW.dt);
			Debug.DrawLine(a - nY * e, b - nY * e, DRAW.col, DRAW.dt);
		}
		#endregion

		#region QUAD
		public static void QUAD(Vector2 o, float sx, float sy, float se = 1f / 100, float e = 1f / 100)
		{

			Vector2[] o_corner_1D = new Vector2[4]
			{
				o + new Vector2(+sx * 0.5f - se, +sy * 0.5f - se),
				o + new Vector2(-sx * 0.5f + se, +sy * 0.5f - se),
				o + new Vector2(-sx * 0.5f + se, -sy * 0.5f + se),
				o + new Vector2(+sx * 0.5f - se, -sy * 0.5f + se),
			};


			Vector2[] i_corner_1D = new Vector2[4]
			{
				o + new Vector2( +sx * 0.5f - (se + e), +sy * 0.5f - (se + e) ),
				o + new Vector2( -sx * 0.5f + (se + e), +sy * 0.5f - (se + e) ),
				o + new Vector2( -sx * 0.5f + (se + e), -sy * 0.5f + (se + e) ),
				o + new Vector2( +sx * 0.5f - (se + e), -sy * 0.5f + (se + e) ),
			};

			for (int i = 0; i < o_corner_1D.Length; i += 1)
			{
				Debug.DrawLine(o_corner_1D[i], o_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
				Debug.DrawLine(i_corner_1D[i], i_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
			}

		}
		#endregion

		#region POLYGON
		public static void POLYGON(Vector2 o, float r, float offset_angle, int N = 6, float se = 1f / 100, float e = 1f / 100)
		{

			Vector2[] o_corner_1D = new Vector2[N],
					  i_corner_1D = new Vector2[N];
			for (int i = 0; i < N; i += 1)
			{
				o_corner_1D[i] = Z.polar(2 * C.PI * i * 1f / N + offset_angle) * (r - se);
				i_corner_1D[i] = Z.polar(2 * C.PI * i * 1f / N + offset_angle) * (r - se - e);
			}

			//
			for (int i = 0; i < o_corner_1D.Length; i += 1)
			{
				Debug.DrawLine(o_corner_1D[i], o_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
				Debug.DrawLine(i_corner_1D[i], i_corner_1D[(i + 1) % o_corner_1D.Length], DRAW.col, DRAW.dt);
			}
			//

		}
		#endregion


		#region CHAR
		/*
        TODO char
        0 - +
        1 - x
        */
		public static void CHAR(Vector2 o, float s, int type = 0)
		{

		}
		#endregion


	}
	#endregion


	#endregion

}
