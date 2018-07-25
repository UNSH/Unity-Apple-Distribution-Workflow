using UnityEngine;
using System.Collections;

public class OsxResolutionFix : MonoBehaviour 
{
	// CREDITS  https://gentlymad.org/blog/post/deliver-mac-store-unity

	#if UNITY_STANDALONE_MAC

		IEnumerator MacResolutionFix()
		{
			    if(!Screen.fullScreen){ yield break; }
			    Resolution[] resolutions = Screen.resolutions;
			    if(resolutions.Length > 0 && resolutions[resolutions.Length -1]. Width > 2048  )
			    {
			          Screen.fullScreen = false;
			          yield return new WaitForEndOfFrame();
			          Screen.fullScreen = true;
			          yield return new WaitForEndOfFrame();
			    }
		}

		Void Awake()
		{ 
			StartCoroutine(MacResolutionFix()); 
		}
	
	#endif

}



