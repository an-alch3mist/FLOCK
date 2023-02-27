using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_req_0 : MonoBehaviour
{
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

		string _url = "https://pastebin.com/raw/1VXaQYEg";
		UnityEngine.Networking.UnityWebRequest _req = UnityEngine.Networking.UnityWebRequest.Get(_url);

		StartCoroutine(show_progress(_req));
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





		yield return null;
	}


	IEnumerator show_progress(UnityEngine.Networking.UnityWebRequest _req)
	{
		while(true)
		{
			if (_req.isDone)
				break;

			Debug.Log(_req.downloadProgress);
			yield return null;
		}
	}


}
