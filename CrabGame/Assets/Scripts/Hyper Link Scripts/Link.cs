using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

// NOTE: Cannot not be tested in Unity Editor, must be exported as something to be tested
public class Link : MonoBehaviour
{
	///////// Opens link if Exported as Windows, Mac, or Linux Standalone
	// CURRENTLY NOT IN USE, to use simply remove the PressHandler component from the object, add an event to the button component on object, and drag Link script in
	public void OpenLink()
	{
		Application.OpenURL("https://forms.gle/yMpMNimxsDzpxAWU9");
	}
	/////////



	///////// Opens link if exported as WebGL
	public void OpenLinkJSPlugin()
	{
#if !UNITY_EDITOR
		openWindow("https://forms.gle/yMpMNimxsDzpxAWU9");
#endif
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);
	/////////

}