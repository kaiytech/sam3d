using System;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Entities.Game
{
    public class Square : MonoBehaviour
    {
        public enum EState
        {
            None,
            Highlighted,
            Selected
        }

        public EState State;

        public MeshFilter MeshLeftBottom;
        public MeshRenderer MeshLeftBottomRenderer;
        public MeshFilter MeshBottomLeft;
        public MeshRenderer MeshBottomLeftRenderer;
        public MeshFilter MeshTopLeft;
        public MeshRenderer MeshTopLeftRenderer;
        public MeshFilter MeshLeftTop;
        public MeshRenderer MeshLeftTopRenderer;
        public MeshFilter MeshRightTop;
        public MeshRenderer MeshRightTopRenderer;
        public MeshFilter MeshTopRight;
        public MeshRenderer MeshTopRightRenderer;
        public MeshFilter MeshBottomRight;
        public MeshRenderer MeshBottomRightRenderer;
        public MeshFilter MeshRightBottom;
        public MeshRenderer MeshRightBottomRenderer;

        public BoxCollider ClickableCollider;

        public int PosX, PosY;

        public event EventHandler<ClickedArgs> Clicked;
        public class ClickedArgs : EventArgs
        {
            public Square Square { get; set; }
        }

        public MeshRenderer MeshRenderer;
        
        void Start()
        {
            ClickableCollider = GetComponent<BoxCollider>();
        }

        public void Setup(int posX, int posY)
        {
            name = $"Square-{posX}/{posY}";
            PosX = posX;
            PosY = posY;
            /*
            // Create a new mesh
            Mesh mesh = new Mesh();

            // Define the vertices of the mesh
            Vector3[] vertices = new Vector3[] {
                new Vector3(0 + X, 0, 0),
                new Vector3(1 + X, 0, 0),
                new Vector3(0 + X, 0, 1),
                new Vector3(1 + X, 0, 1),
            };

            // Define the triangles of the mesh
            int[] triangles = new int[] {
                0, 2, 1,
                2, 3, 1,
            };

            // Assign the vertices and triangles to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            // Recalculate the normals of the mesh
            mesh.RecalculateNormals();

            // Add a MeshFilter component to the GameObject to display the mesh
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            //meshFilter.mesh = mesh;

            */
            MeshRenderer = GetComponent<MeshRenderer>();
            var pos1 = 2 * posX;
            var pos2 = 2 * posY;
            transform.localPosition = new Vector3(pos1, 0.02f, pos2);
            var mat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            mat.color = new Color32(255, 255, 255, 255);

            MeshLeftBottomRenderer = MeshLeftBottom.GetComponent<MeshRenderer>();
            MeshLeftBottomRenderer.sharedMaterial = mat;
            MeshBottomLeftRenderer = MeshBottomLeft.GetComponent<MeshRenderer>();
            MeshBottomLeftRenderer.sharedMaterial = mat;
            MeshTopLeftRenderer = MeshTopLeft.GetComponent<MeshRenderer>();
            MeshTopLeftRenderer.sharedMaterial = mat;
            MeshLeftTopRenderer = MeshLeftTop.GetComponent<MeshRenderer>();
            MeshLeftTopRenderer.sharedMaterial = mat;
            MeshRightTopRenderer = MeshRightTop.GetComponent<MeshRenderer>();
            MeshRightTopRenderer.sharedMaterial = mat;
            MeshTopRightRenderer = MeshTopRight.GetComponent<MeshRenderer>();
            MeshTopRightRenderer.sharedMaterial = mat;
            MeshBottomRightRenderer = MeshBottomRight.GetComponent<MeshRenderer>();
            MeshBottomRightRenderer.sharedMaterial = mat;
            MeshRightBottomRenderer = MeshRightBottom.GetComponent<MeshRenderer>();
            MeshRightBottomRenderer.sharedMaterial = mat;
        }
        
        void Update()
        {
            var camera = GameObject.Find("Main Camera");
            var ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (ClickableCollider.Raycast(ray, out hitData, 1000))
            {
                if (State == EState.None)
                    State = EState.Highlighted;
                if (Input.GetMouseButtonUp(0))
                    Clicked?.Invoke(this, new ClickedArgs() { Square = this });
            }
            else
                if (State == EState.Highlighted)
                    State = EState.None;
            
            var color = State switch
            {
                EState.Highlighted => new Color32(255, 255, 0, 255),
                EState.Selected => new Color32(255, 165, 0, 255),
                _ => new Color32(255, 255, 255, 255)
            };

            if (MeshLeftBottomRenderer.sharedMaterial.color != color)
            {
                MeshLeftBottomRenderer.sharedMaterial.color = color;
                MeshBottomLeftRenderer.sharedMaterial.color = color;
                MeshTopLeftRenderer.sharedMaterial.color = color;
                MeshLeftTopRenderer.sharedMaterial.color = color;
                MeshRightTopRenderer.sharedMaterial.color = color;
                MeshTopRightRenderer.sharedMaterial.color = color;
                MeshBottomRightRenderer.sharedMaterial.color = color;
                MeshRightBottomRenderer.sharedMaterial.color = color;
            }
        }
    }
}