using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{

    //AssetBundle sfx_bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, "sfx"));
    //defaultAttackSound = sfx_bundle.LoadAsset<AudioClip>(name);

    // A dictionary to hold the AssetBundle references
    static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
    static AssetBundleManager()
    {
        dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
    }
    // Class with the AssetBundle reference, url and version
    private class AssetBundleRef
    {
        public AssetBundle assetBundle = null;
        /*
        public int version;
        public string url;
        public AssetBundleRef(string strUrlIn, int intVersionIn)
        {
            url = strUrlIn;
            version = intVersionIn;
        }
        */
    };
    // Get an AssetBundle
    public static AssetBundle getAssetBundle(string url)
    {
        string keyName = url;
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
            return abRef.assetBundle;
        else
            return null;
    }

    // Download an AssetBundle
    /*
    public static IEnumerator downloadAssetBundle(string url, int version)
    {
        string keyName = url + version.ToString();
        if (dictAssetBundleRefs.ContainsKey(keyName))
            yield return null;
        else
        {
            while (!Caching.ready)
                yield return null;

            using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
            {
                yield return www;
                if (www.error != null)
                    throw new Exception("WWW download:" + www.error);
                AssetBundleRef abRef = new AssetBundleRef(url, version);
                abRef.assetBundle = www.assetBundle;
                dictAssetBundleRefs.Add(keyName, abRef);
            }
        }
    }
    */

    // Load an AssetBundle from project files
    public static void LoadAssetBundle(string url)
    {
        if (dictAssetBundleRefs.ContainsKey(url))
            return;
        else
        {
            AssetBundleRef abRef = new AssetBundleRef();
            AssetBundle sfx_bundle = AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath, url));

            abRef.assetBundle = sfx_bundle;
            dictAssetBundleRefs.Add(url, abRef);

            return;
        }

    }
    // Unload an AssetBundle
    public static void Unload(string url, int version, bool allObjects)
    {
        string keyName = url + version.ToString();
        AssetBundleRef abRef;
        if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
        {
            abRef.assetBundle.Unload(allObjects);
            abRef.assetBundle = null;
            dictAssetBundleRefs.Remove(keyName);
        }
    }

}
