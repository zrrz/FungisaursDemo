using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BuildScript : MonoBehaviour
{

    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            Debug.Log("Changing encryption plist file");

            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            var rootDict = plist.root;

            // Change value of NSCameraUsageDescription in Xcode plist
            var buildKey = "NSCameraUsageDescription";
            rootDict.SetString(buildKey, "Using the camera for augmented reality");

            // Change value of NSCameraUsageDescription in Xcode plist
            var buildKey2 = "NSPhotoLibraryUsageDescription";
            rootDict.SetString(buildKey2, "Saving screenshots to your device");

            var buildKey3 = "ITSAppUsesNonExemptEncryption";
            rootDict.SetString(buildKey3, "false");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}
