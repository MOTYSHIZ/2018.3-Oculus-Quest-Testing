#if Int_FinalIK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRInteraction;
using VRWeaponInteractor;

public class HandPoseControllerWeapon : HandPoseController
{
	public void CheckForTrigger(object[] args)
	{
		VRGunHandler gunHandler = (VRGunHandler)args[0];
		string poseName = (string)args[1];
		bool pickingUp = (bool)args[2];

		Transform pose = GetPoseByName(poseName);
		if (pose == null) return;
		VRGunTrigger gunTrigger = pose.GetComponentInChildren<VRGunTrigger>();
		if (gunTrigger == null) return;
		gunTrigger.enabled = true;
		gunTrigger.gunHandler = pickingUp ? gunHandler : null;
	}
}
#endif