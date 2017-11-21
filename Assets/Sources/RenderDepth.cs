﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RenderDepth : MonoBehaviour {

    public Vector3 threshold;

    public Material mat;
    [SerializeField, Range(0, 1)] float _intensity = 0.5f;

    private Camera camera;
    private Matrix4x4 view;
    private Matrix4x4 proj;
    private Matrix4x4 invVP;

    private void OnEnable()
    {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
    }
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 trs = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        //Matrix4x4 trs = Matrix4x4.TRS(Vector3.zero, transform.rotation, Vector3.one);
        view = this.camera.worldToCameraMatrix;
        //Matrix4x4 view = this.camera.cameraToWorldMatrix;
        //proj = this.camera.projectionMatrix;
        proj = GL.GetGPUProjectionMatrix(this.camera.projectionMatrix,false);
        invVP = Matrix4x4.Inverse(proj * view);
        //invVP = (view);

        mat.SetFloat("_XThreshold", threshold.x);
        mat.SetFloat("_YThreshold", threshold.y);
        mat.SetFloat("_ZThreshold", threshold.z);
        mat.SetMatrix("_TRS", trs);
        mat.SetMatrix("_InvVP", invVP);
        //mat.SetMatrix("_InvVP", this.camera.projectionMatrix);
        mat.SetFloat("_Intensity", _intensity);
        Graphics.Blit(source, destination, mat);
    }

    private void OnDrawGizmos()
    {
        const float boarder_size = 100;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(threshold.x, 0, 0), new Vector3(0, boarder_size, boarder_size));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, threshold.y, 0), new Vector3(boarder_size, 0, boarder_size));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(0, 0, threshold.z), new Vector3(boarder_size, boarder_size, 0));

    }
}