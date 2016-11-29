using UnityEditor;
using UnityEngine;

public class EditorMenu {

	[MenuItem("AmamiHaruka/Build AssetBundles")]
	static void BuildAllAssetBundles() {
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Android", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
	}

	[MenuItem("AmamiHaruka/ClearPrefs")]
	static void ClearPrefs() {
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}
}
