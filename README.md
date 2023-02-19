```
STIMULATE
```

``` cs
	IEnumerator STIMULATE()
	{
		#region frame_rate
		QualitySettings.vSyncCount = 2;

		yield return null;
		yield return null;
		#endregion


		OBJ.INITIALIZE_CLEAR_HOLDER();


		BOID[] boids = new BOID[300];
		OBJ_S[] obj_s_boids = new OBJ_S[boids.Length];


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

		#region obj_s_boids
		
		for (int i = 0; i < obj_s_boids.Length; i += 1)
		{
			obj_s_boids[i] = new OBJ_S("boid_" + i.ToString());

			int index = Random.Range(0, 100);
				 if(index < 020) obj_s_boids[i].sprite("boid_0");
			else if(index < 050) obj_s_boids[i].sprite("boid_1");
			else if(index < 080) obj_s_boids[i].sprite("boid_2");
			else if(index < 100) obj_s_boids[i].sprite("boid_3");

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

			//
			for (int i = 0; i < boids.Length; i += 1)
			{
				boids[i].accumulated_vel = Vector2.zero;

				boids[i].accumulated_vel += boid_align   (boids, boids[i], 0.20f) * 0.3f;
				boids[i].accumulated_vel += boid_attract (boids, boids[i], 0.22f) * 0.89f;
				boids[i].accumulated_vel += boid_seperate(boids, boids[i], 0.25f) * 0.90f;

			}



			//
			for (int i = 0; i < boids.Length; i += 1)
			{
				boids[i].pos += boids[i].vel * dt;

				float w = 3f;
				if (boids[i].pos.x > +(w + 1f / 10 ) ) boids[i].pos.x = -w;
				if (boids[i].pos.x < -(w + 1f / 10 ) ) boids[i].pos.x = +w;
				if (boids[i].pos.y > +(w + 1f / 10 ) ) boids[i].pos.y = -w;
				if (boids[i].pos.y < -(w + 1f / 10 ) ) boids[i].pos.y = +w;



				float vel = 0.5f;
				float M_f = 0.2f;


				#region steer
				Vector2 steer = (boids[i].accumulated_vel * vel - boids[i].vel);
				float steer_mag = steer.magnitude;
				if (steer_mag != 0f)
					steer = steer / steer_mag * Z.clamp(steer_mag, 0f, M_f);
				#endregion


				boids[i].vel = (boids[i].vel + steer).normalized * vel;
			}


			//
			#region obj_s_boids

			for (int i = 0; i < obj_s_boids.Length; i += 1)
			{
				obj_s_boids[i].align(boids[i].pos, boids[i].pos + boids[i].vel, scale: false);
			}

			#endregion


			yield return null;
		}

	}
	//// .................. ////


```
<br><br><br>
<br><br><br>

```
boid
boid_methods
```

``` cs

	public class BOID
	{
		public Vector2 pos;
		public Vector2 vel;
		public Vector2 accumulated_vel;
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
					sum += (boid.pos - boids[i].pos) / dist;

					found_one = true;
				}

		}


		if (found_one)
			return sum.normalized;
		else
			return Vector3.zero;

	}

	#endregion


```
