//========= Copyright 2019, Sam Tague, All rights reserved. ===================
//
// Extension of hand poser window to allow gun trigger finger action
//
//===================Contact Email: Sam@MassGames.co.uk===========================

#if VRInteraction && Int_FinalIK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VRInteraction;

namespace VRWeaponInteractor
{
	public class WeaponHandPoseWindow : HandPoserWindow
	{
		private Transform _leftTriggerFinger;
		private Vector3 _leftDefaultPosition;
		private Quaternion _leftDefaultRotation;
		private Vector3 _leftPulledPosition;
		private Quaternion _leftPulledRotation;
		private bool _leftTriggerPulled;

		private Transform _rightTriggerFinger;
		private Vector3 _rightDefaultPosition;
		private Quaternion _rightDefaultRotation;
		private Vector3 _rightPulledPosition;
		private Quaternion _rightPulledRotation;
		private bool _rightTriggerPulled;
		private bool _firstRender = false;

		[MenuItem("VR Weapon Interactor/FinalIK Hand Poser With Trigger", false, 0)]
		public static void MenuInitHandPoserWithTrigger()
		{
			EditorWindow.GetWindow(typeof(WeaponHandPoseWindow), true, "Final IK Hand Poser For Weapons", true);
		}

		override protected void OnGUI () 
		{
			base.OnGUI();

			if (_vrIK == null || _item == null || _item.item == null || IsPrefab(_item.item.gameObject) || _item.GetType() != typeof(VRGunHandler)) return;
			if (!_firstRender)
			{
				_firstRender = true;
				CheckForGunTrigger();
			}

			ShowTriggerFinger();
		}

		private void CheckForGunTrigger()
		{
			Transform leftReferencedPos = _handPoseController.GetPoseByName(_item.leftHandIKPoseName);
			if (leftReferencedPos != null)
			{
				VRGunTrigger gunTrigger = leftReferencedPos.GetComponentInChildren<VRGunTrigger>();
				if (gunTrigger == null) return;
				_leftDefaultPosition = gunTrigger.defaultTriggerPosition;
				_leftDefaultRotation = gunTrigger.defaultTriggerRotation;
				_leftPulledPosition = gunTrigger.pulledTriggerPosition;
				_leftPulledRotation = gunTrigger.pulledTriggerRotation;

				if (_vrIK.references.leftHand != null)
				{
					Transform[] handTransforms = _vrIK.references.leftHand.GetComponentsInChildren<Transform>();
					foreach(Transform handTrans in handTransforms)
					{
						if (handTrans.name != gunTrigger.name) continue;
						_leftTriggerFinger = handTrans;
						break;
					}
				}
			}
			Transform rightReferencedPos = _handPoseController.GetPoseByName(_item.rightHandIkPoseName);
			if (rightReferencedPos != null)
			{
				VRGunTrigger gunTrigger = leftReferencedPos.GetComponentInChildren<VRGunTrigger>();
				if (gunTrigger == null) return;
				_rightDefaultPosition = gunTrigger.defaultTriggerPosition;
				_rightDefaultRotation = gunTrigger.defaultTriggerRotation;
				_rightPulledPosition = gunTrigger.pulledTriggerPosition;
				_rightPulledRotation = gunTrigger.pulledTriggerRotation;

				if (_vrIK.references.rightHand != null)
				{
					Transform[] handTransforms = _vrIK.references.rightHand.GetComponentsInChildren<Transform>();
					foreach(Transform handTrans in handTransforms)
					{
						if (handTrans.name != gunTrigger.name) continue;
						_rightTriggerFinger = handTrans;
						break;
					}
				}
			}
		}

		private void ShowTriggerFinger()
		{
			Transform triggerFinger = _hand == Hand.LEFT ? _leftTriggerFinger : _rightTriggerFinger;
			Vector3 defaultPosition = _hand == Hand.LEFT ? _leftDefaultPosition : _rightDefaultPosition;
			Quaternion defaultRotation = _hand == Hand.LEFT ? _leftDefaultRotation : _rightDefaultRotation;
			Vector3 pulledPosition = _hand == Hand.LEFT ? _leftPulledPosition : _rightPulledPosition;
			Quaternion pulledRotation = _hand == Hand.LEFT ? _leftPulledRotation : _rightPulledRotation;
			bool triggerPulled = _hand == Hand.LEFT ? _leftTriggerPulled : _rightTriggerPulled;

			//Chose trigger finger bone
			triggerFinger = (Transform)EditorGUILayout.ObjectField("Trigger Finger Bone", triggerFinger, typeof(Transform), true);
			if (triggerFinger == null) return;

			defaultPosition = EditorGUILayout.Vector3Field("Default Position", defaultPosition);
			Quaternion tempDefaultRotation = defaultRotation;
			tempDefaultRotation.eulerAngles = EditorGUILayout.Vector3Field("Default Rotation", tempDefaultRotation.eulerAngles);
			defaultRotation = tempDefaultRotation;

			pulledPosition = EditorGUILayout.Vector3Field("Pulled Position", pulledPosition);
			Quaternion tempPulledRotation = pulledRotation;
			tempPulledRotation.eulerAngles = EditorGUILayout.Vector3Field("Pulled Rotation", tempPulledRotation.eulerAngles);
			pulledRotation = tempPulledRotation;

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Set Current To Default"))
			{
				triggerPulled = false;
				defaultPosition = triggerFinger.localPosition;
				defaultRotation = triggerFinger.localRotation;
			}
			if (GUILayout.Button("Set Current To Pulled"))
			{
				triggerPulled = true;
				pulledPosition = triggerFinger.localPosition;
				pulledRotation = triggerFinger.localRotation;
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.HelpBox("Move the finger bone in the scene and save the positions using the buttons above. Use the toggle button to switch between the two positions", MessageType.Info);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Toggle Trigger"))
			{
				if (triggerPulled)
				{
					triggerFinger.localPosition = defaultPosition;
					triggerFinger.localRotation = defaultRotation;
				} else
				{
					triggerFinger.localPosition = pulledPosition;
					triggerFinger.localRotation = pulledRotation;
				}
				triggerPulled = !triggerPulled;
			}
			if (GUILayout.Button("Select Trigger"))
			{
				Selection.activeGameObject = triggerFinger.gameObject;
			}

			if (_hand == Hand.LEFT)
			{
				_leftTriggerFinger = triggerFinger;
				_leftDefaultPosition = defaultPosition;
				_leftDefaultRotation = defaultRotation;
				_leftPulledPosition = pulledPosition;
				_leftPulledRotation = pulledRotation;
				_leftTriggerPulled = triggerPulled;
			} else
			{
				_rightTriggerFinger = triggerFinger;
				_rightDefaultPosition = defaultPosition;
				_rightDefaultRotation = defaultRotation;
				_rightPulledPosition = pulledPosition;
				_rightPulledRotation = pulledRotation;
				_rightTriggerPulled = triggerPulled;
			}

			EditorGUI.BeginDisabledGroup((_hand == Hand.LEFT ? _vrIK.references.leftHand : _vrIK.references.rightHand) == null ||
				string.IsNullOrEmpty(_hand == Hand.LEFT ? _leftPoseName : _rightPoseName));
			if (GUILayout.Button("Save Pose With Trigger"))
			{
				SaveHand();
			}
			EditorGUI.EndDisabledGroup();
		}

		override protected Transform SaveHand()
		{
			//Replace hand pose controller with hand pose controller weapon
			if (_handPoseController.GetType() != typeof(HandPoseControllerWeapon))
			{
				HandPoseControllerWeapon handPoseControllerWeapon = _handPoseController.gameObject.AddComponent<HandPoseControllerWeapon>();
				handPoseControllerWeapon.defaultLeftPose = _handPoseController.defaultLeftPose;
				handPoseControllerWeapon.defaultRightPose = _handPoseController.defaultRightPose;
				handPoseControllerWeapon.poses = _handPoseController.poses;
				DestroyImmediate(_handPoseController);
				_handPoseController = handPoseControllerWeapon;
			}

			Transform newPoseRoot = base.SaveHand();
			Transform triggerFinger = _hand == Hand.LEFT ? _leftTriggerFinger : _rightTriggerFinger;
			if (triggerFinger == null) return newPoseRoot;

			Transform[] handTransforms = newPoseRoot.GetComponentsInChildren<Transform>();
			Transform referenceFingerBone = null;
			foreach(Transform handTrans in handTransforms)
			{
				if (handTrans.name != triggerFinger.name) continue;
				referenceFingerBone = handTrans;
				break;
			}
			if (referenceFingerBone == null)
			{
				Debug.LogError("Could not find finger bone in reference. trigger finger has not been applied");
				return newPoseRoot;
			}
			VRGunTrigger gunTrigger = referenceFingerBone.gameObject.AddComponent<VRGunTrigger>();

			Vector3 defaultPosition = _hand == Hand.LEFT ? _leftDefaultPosition : _rightDefaultPosition;
			Quaternion defaultRotation = _hand == Hand.LEFT ? _leftDefaultRotation : _rightDefaultRotation;
			Vector3 pulledPosition = _hand == Hand.LEFT ? _leftPulledPosition : _rightPulledPosition;
			Quaternion pulledRotation = _hand == Hand.LEFT ? _leftPulledRotation : _rightPulledRotation;

			gunTrigger.defaultTriggerPosition = defaultPosition;
			gunTrigger.defaultTriggerRotation = defaultRotation;
			gunTrigger.pulledTriggerPosition = pulledPosition;
			gunTrigger.pulledTriggerRotation = pulledRotation;
			return newPoseRoot;
		}
	}
}
#endif