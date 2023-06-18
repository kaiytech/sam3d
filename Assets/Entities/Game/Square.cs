using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
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

        public enum EFieldType
        {
            Default,
            Marked,
            DugUp
        }

        public enum EUndergroundType
        {
            None,
            Mined
        }

        public GameObject TextPrefab;
        private TextMeshPro _text;

        public EFieldType Field;
        public EUndergroundType Underground;

        public EState State;

        public Renderer Renderer;
        
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
        public MeshFilter MeshCenter;
        public MeshRenderer MeshCenterRenderer;

        public Material SquareMaterial;

        public BoxCollider ClickableCollider;

        public int PosX, PosY;

        private int _number = -1;

        public int Number
        {
            get
            {
                if (_number is -1)
                    _number = GetNumber();
                return _number;
            }
        }

        public void Reset()
        {
            Field = Square.EFieldType.Default;
            Underground = Square.EUndergroundType.None;
            _number = 0;
        }
        
        

        public event EventHandler<ClickedArgs> Clicked;
        public event EventHandler<ClickedArgs> ClickedRight; 

        public bool CanMoveTo = false;
        public class ClickedArgs : EventArgs
        {
            public Square Square { get; set; }
        }

        public MeshRenderer MeshRenderer;
        
        void Start()
        {
            ClickableCollider = GetComponent<BoxCollider>();
            Renderer = this.AddComponent<Renderer>();
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
            var mat = new Material(SquareMaterial);
            mat.color = new Color32(255, 255, 255, 255);

            var mat2 = new Material(SquareMaterial);


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
            MeshCenterRenderer = MeshCenter.GetComponent<MeshRenderer>();
            MeshCenterRenderer.sharedMaterial = mat2;
        }
        
        void Update()
        {
            if (_text is null)
            {
                _text = TextPrefab.GetComponent<TextMeshPro>();
                _text.text = "";
            }
            
            var ray = Globals.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (ClickableCollider.Raycast(ray, out hitData, 1000))
            {
                if (State == EState.None)
                    State = EState.Highlighted;
                if (Input.GetMouseButtonUp(0))
                    Clicked?.Invoke(this, new ClickedArgs() { Square = this });
                if (Input.GetMouseButtonUp(1))
                    ClickedRight?.Invoke(this, new ClickedArgs() {Square = this});
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
            
            MeshCenterRenderer.transform.localScale = new Vector3(2, 2, 2);

            if (!(Field == EFieldType.DugUp && Underground != EUndergroundType.Mined))
                _text.text = "";

            if (Field == EFieldType.Default)
                color = new Color32(255, 255, 255, 255);
            else if (Field == EFieldType.Marked)
                color = new Color32(0, 0, 255, 255);
            else if (Underground == EUndergroundType.Mined)
                color = new Color32(0, 0, 0, 255);
            else
            {
                switch (Number)
                {
                    default:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 255, 255, 255);
                        break;
                    case 0:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 200, 200, 255);
                        break;
                    case 1:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 180, 180, 255);
                        _text.color = Color.blue;
                        break;
                    case 2:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 170, 170, 255);
                        _text.color = Color.green;
                        break;
                    case 3:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 140, 140, 255);
                        _text.color = Color.yellow;
                        break;
                    case 4:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 120, 120, 255);
                        _text.color = Color.magenta;
                        break;
                    case 5:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 100, 100, 255);
                        _text.color = Color.red;
                        break;
                    case 6:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 80, 80, 255);
                        _text.color = new Color32(255, 80, 80, 255);
                        break;
                    case 7:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 60, 60, 255);
                        _text.color = new Color32(255, 140, 0, 255);
                        break;
                    case 8:
                        MeshCenterRenderer.sharedMaterial.color = new Color32(255, 0, 0, 255);
                        _text.color = Color.gray;
                        break;
                }

                if (Number > 0)
                    _text.text = Number.ToString();
            }

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

        private int GetNumber()
        {
            var n = 0;
            if (GetSideSquare(ESide.TopLeft) is not null &&
                GetSideSquare(ESide.TopLeft).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.Top) is not null &&
                GetSideSquare(ESide.Top).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.TopRight) is not null &&
                GetSideSquare(ESide.TopRight).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.Left) is not null &&
                GetSideSquare(ESide.Left).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.Right) is not null &&
                GetSideSquare(ESide.Right).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.BottomLeft) is not null &&
                GetSideSquare(ESide.BottomLeft).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.Bottom) is not null &&
                GetSideSquare(ESide.Bottom).Underground == EUndergroundType.Mined)
                n++;
            if (GetSideSquare(ESide.BottomRight) is not null &&
                GetSideSquare(ESide.BottomRight).Underground == EUndergroundType.Mined)
                n++;
            return n;
        }

        public enum ESide
        {
            TopLeft,
            Top,
            TopRight,
            Left,
            Right,
            BottomLeft,
            Bottom,
            BottomRight
        }
        
        public Square GetSideSquare(ESide side)
        {
            switch (side)
            {
                case ESide.TopLeft:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY + 1)?.Item2;
                case ESide.Top:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX && i.Item2.PosY == PosY + 1)?.Item2;
                case ESide.TopRight:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY + 1)?.Item2;
                case ESide.Left:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY)?.Item2;
                case ESide.Right:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY)?.Item2;
                case ESide.BottomLeft:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY - 1)?.Item2;
                case ESide.Bottom:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX && i.Item2.PosY == PosY - 1)?.Item2;
                case ESide.BottomRight:
                    return Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY - 1)?.Item2;
            }

            return null;
        }

        public List<Square> GetAllSideSquares()
        {
            #nullable enable
            return new List<Square?>
            {
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY + 1)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX && i.Item2.PosY == PosY + 1)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY + 1)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX + 1 && i.Item2.PosY == PosY - 1)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX && i.Item2.PosY == PosY - 1)?.Item2,
                Globals.Arena.Squares.FirstOrDefault(i => i.Item2.PosX == PosX - 1 && i.Item2.PosY == PosY - 1)?.Item2
            }.Where(square => square != null).ToList();;
            #nullable restore
        }

        public struct DigResult
        {
            public bool HitMine;
        }

        public DigResult Dig(bool spread = true)
        {
            var result = _Dig();
            if (!result.HitMine && Number is 0 or 1 && spread)
                Spread(true);
            
            // if all bombs around are marked, attempt to dig every field around
            var sideSquares = GetAllSideSquares();
            if (!result.HitMine && sideSquares.Count(s => s.Field == EFieldType.Marked) == Number)
                foreach (var sideSquare in sideSquares)
                {
                    if (sideSquare.Underground == EUndergroundType.Mined && sideSquare.Field != EFieldType.Marked)
                        result.HitMine = true;
                    if (sideSquare.Field != EFieldType.Marked)
                        sideSquare.Field = EFieldType.DugUp;
                }

            return result;
        }

        public void Spread(bool force = false)
        {
            if (Underground == EUndergroundType.Mined)
                return;
            if (Field == EFieldType.DugUp && !force)
                return;
            var sideSquares = GetAllSideSquares();
            if (sideSquares.All(s => s.Number != 0))
                return;
            Field = EFieldType.DugUp;
            foreach (var sideSquare in sideSquares)
                sideSquare.Spread();
        }

        private DigResult _Dig()
        {
            if (Field is EFieldType.Marked or EFieldType.DugUp)
                return new DigResult() { HitMine = false };
            
            Field = EFieldType.DugUp;

            if (Underground == EUndergroundType.Mined)
                return new DigResult() { HitMine = true };
            
            return new DigResult() { HitMine = false };
        }

        public struct MarkResult
        {
            public bool Marked;
            public bool DeMarked;
        }
        
        public MarkResult Mark()
        {
            if (Field is EFieldType.DugUp)
                return new MarkResult() {Marked = false, DeMarked = false};
            if (Field is EFieldType.Default)
            {
                Field = EFieldType.Marked;
                return new MarkResult() { Marked = true, DeMarked = false };
            }
            Field = EFieldType.Default;
            return new MarkResult() { Marked = false, DeMarked = true };
        }
    }
}