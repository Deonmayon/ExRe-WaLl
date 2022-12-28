using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MediaPipe.HandLandmark;
using System.Linq;

public class HandLandmark : MonoBehaviour
{
        #region Editable attributes

    [SerializeField] Camera mainCamera;
    [SerializeField] WebCamInput webCamInput;
    [SerializeField] RawImage inputImageUI;
    [SerializeField] Shader shader;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField, Range(0, 1)] float humanExistThreshold = 0.5f;

    #endregion

    #region Private members

    HandLandmarkDetector _detector;
    Material _material;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _detector = new HandLandmarkDetector(_resources);
        // _material = new Material(_shader);
    }

    void OnDestroy()
    {
        _detector.Dispose();
        // Destroy(_material);
    }

    void LateUpdate()
    {
        _detector.ProcessImage(webCamInput.inputImageTexture);
        inputImageUI.texture = webCamInput.inputImageTexture;
        // foreach(Vector2 point in Enum.GetValues(typeof(HandLandmarkDetector.KeyPoint))){
            /*
            0~32 index datas are pose world landmark.
            Check below Mediapipe document about relation between index and landmark position.
            https://google.github.io/mediapipe/solutions/pose#pose-landmark-model-blazepose-ghum-3d
            Each data factors are
            x, y and z: Real-world 3D coordinates in meters with the origin at the center between hips.
            w: The score of whether the world landmark position is visible ([0, 1]).
        
            33 index data is the score whether human pose is visible ([0, 1]).
            This data is (score, 0, 0, 0).
            */
        //     Debug.LogFormat("{0}", point);
        // }
        // Debug.Log("---");

        // _scoreUI.text = $"Score: {_detector.Score:0.00}";
        // _handednessUI.text = $"Handedness: {_detector.Handedness:0.00}";
    }

    // void OnRenderObject()
    // {
    //     _material.SetBuffer("_Vertices", _detector.OutputBuffer);
    //     _material.SetPass(0);
    //     Graphics.DrawProceduralNow
    //       (MeshTopology.Lines, 4, HandLandmarkDetector.VertexCount);
    // }

    #endregion
}
