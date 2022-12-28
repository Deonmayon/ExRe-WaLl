using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MediaPipe.HandPose {

public sealed class HandAnimator : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] WebCamInput _webcam = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] bool _useAsyncReadback = true;
    // [Space]
    // [SerializeField] Mesh _jointMesh = null;
    // [SerializeField] Mesh _boneMesh = null;
    // [Space]
    // [SerializeField] Material _jointMaterial = null;
    // [SerializeField] Material _boneMaterial = null;
    // [Space]
    [SerializeField] GameObject _Screen = null;
    [SerializeField] RawImage _monitorUI = null;
    [SerializeField] GameObject _cursor = null;

    #endregion

    #region Private members

    RectTransform _ScreenTransform;
    RectTransform _CursorTransform;
    HandPipeline _pipeline;

    static readonly (int, int)[] BonePairs =
    {
        (0, 1), (1, 2), (1, 2), (2, 3), (3, 4),     // Thumb
        (5, 6), (6, 7), (7, 8),                     // Index finger
        (9, 10), (10, 11), (11, 12),                // Middle finger
        (13, 14), (14, 15), (15, 16),               // Ring finger
        (17, 18), (18, 19), (19, 20),               // Pinky
        (0, 17), (2, 5), (5, 9), (9, 13), (13, 17)  // Palm
    };

    Matrix4x4 CalculateJointXform(Vector3 pos)
      => Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 0.07f);

    Matrix4x4 CalculateBoneXform(Vector3 p1, Vector3 p2)
    {
        var length = Vector3.Distance(p1, p2) / 2;
        var radius = 0.03f;

        var center = (p1 + p2) / 2;
        var rotation = Quaternion.FromToRotation(Vector3.up, p2 - p1);
        var scale = new Vector3(radius, length, radius);

        return Matrix4x4.TRS(center, rotation, scale);
    }

    #endregion

    #region MonoBehaviour implementation

    void Start(){
      _pipeline = new HandPipeline(_resources);
      _ScreenTransform = _Screen.GetComponent<RectTransform>();
      _CursorTransform = _cursor.GetComponent<RectTransform>();
    }

    void OnDestroy()
      => _pipeline.Dispose();

    void LateUpdate()
    {
        // Feed the input image to the Hand pose pipeline.
        _pipeline.UseAsyncReadback = _useAsyncReadback;
        _pipeline.ProcessImage(_webcam.inputImageTexture);
        if (_pipeline.Score() >= 0.00f){
          //change to curser active animated

          // visualize hand
          Vector3 finger_positions = _pipeline.GetKeyPoint(8);// tip of index finger relative position 
          Vector3 cursor_positions = new Vector3(
            (1-finger_positions.x)/2 * _ScreenTransform.rect.width ,
            finger_positions.y  * _ScreenTransform.rect.height  + _ScreenTransform.rect.height/2 ,
            finger_positions.z
          );
          Debug.LogFormat("position: {0}",cursor_positions);
          _CursorTransform.position = cursor_positions;
        }else{
          //change to curser inactive animated
        }

        // var layer = gameObject.layer;

        // // Joint balls
        // for (var i = 0; i < HandPipeline.KeyPointCount; i++)
        // {
        //     Debug.LogFormat("{0} : {1}",i,_pipeline.GetKeyPoint(i));
        //     var xform = CalculateJointXform(_pipeline.GetKeyPoint(i));
        //     Graphics.DrawMesh(_jointMesh, xform, _jointMaterial, layer);
        // }

        // // Bones
        // foreach (var pair in BonePairs)
        // {
        //     var p1 = _pipeline.GetKeyPoint(pair.Item1);
        //     var p2 = _pipeline.GetKeyPoint(pair.Item2);
        //     var xform = CalculateBoneXform(p1, p2);
        //     Graphics.DrawMesh(_boneMesh, xform, _boneMaterial, layer);
        // }

        // UI update
        _monitorUI.texture = _webcam.inputImageTexture;
    }

    #endregion
}

} // namespace MediaPipe.HandPose
