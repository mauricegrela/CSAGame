// This script is published under the CC0 1.0 Universal License
// https://creativecommons.org/publicdomain/zero/1.0/
// To the extent possible under law, Spryly® Ltd. has waived all copyright
// and related or neighboring rights to this script. This work is published from: Canada.

using UnityEngine;
using UnityEditor;
using System.Collections;

public static class SetConstantTangents {

	// Takes the selected animation clips and converts all keyframe tangents to constant / step tangents
	[MenuItem ("Maple/Animation/Set Constant Tangents")]
	static public void SetConstantTangentsOnAnimationClip() {
		Object[] clips = Selection.GetFiltered( typeof( AnimationClip ), SelectionMode.TopLevel );

		for( int i = 0; i < clips.Length; ++i ) {
			AnimationClip clip = clips[ i ] as AnimationClip;
			EditorCurveBinding[] floatBindings = AnimationUtility.GetCurveBindings( clip );

			for( int j = 0; j < floatBindings.Length; ++j ) {
				EditorCurveBinding binding = floatBindings[ j ];
				AnimationCurve curve = AnimationUtility.GetEditorCurve( clip, binding );

				for( int k = 0; k < curve.keys.Length; ++k ) {
					AnimationUtility.SetKeyLeftTangentMode( curve, k, AnimationUtility.TangentMode.Constant );
					AnimationUtility.SetKeyRightTangentMode( curve, k, AnimationUtility.TangentMode.Constant );
				}

				Undo.RecordObject( clip, "Set Constant Tangents" );
				clip.SetCurve( binding.path, typeof( Transform ), binding.propertyName, curve );
			}
		}
	}

}