
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace felipsteles
{
    public class ChessBoard : MonoBehaviour
    {   

        [Header("Art stuff")]
        [SerializeField] private Material tileMaterial;
        [SerializeField] private float tileSize = 1.5f;
        [SerializeField] private float yOffset = 0.15f;
        [SerializeField] private Vector3 boardCenter = Vector3.zero;
        [SerializeField] private float deathSize = 0.3f;
        [SerializeField] private float deathSpacing = 0.3f;
        [SerializeField] private float dragOffset = 1.5f;
        [SerializeField] private GameObject victoryScreen;
        

        //Ajuste de escala
        private Vector3 ajuste = new Vector3(1f,1f,1500f);

        [Header("Prefabs & Materials")]
        [SerializeField] private GameObject[] prefabs;
        [SerializeField] private Material[] teamMaterials;



        //De acordo com o tabuleiro de xadrez
        private ChessPiece[,] chessPieces;
        private ChessPiece currentlyDragging;
        private List<Vector2Int> availableMoves = new List<Vector2Int>();
        private List<ChessPiece> deadWhites = new List<ChessPiece>();
        private List<ChessPiece> deadBlacks = new List<ChessPiece>();
        private const int TILE_COUNT_X = 8;
        private const int TILE_COUNT_Y = 8;
        private GameObject[,] tiles; //Criando um obejto de duas dimensoes
        private Camera currentCamera;
        private Vector2Int currentHover;
        private Vector3 bounds; //limite
        private bool isWhiteTurn;

        private void Awake()
        {
            isWhiteTurn = true;

            GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);

            SpawnAllPieces();
            PositionAllPieces();
        }

        private void Update()
        {
            if(!currentCamera)
            {
                currentCamera = Camera.main;
                return;
            }

            RaycastHit info;
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover","Highlight"))) //Add as camadas q as peças podem ser dropadas
            {
                //Pega o index de cada tile que selecionamos
                Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

                //If we're hovering a tile after not hovering any tiles
                if(currentHover == -Vector2Int.one)
                {
                    currentHover = hitPosition;
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                }

                // If we were already hovering a tile, change the previous one
                if (currentHover != hitPosition)
                {
                    tiles[currentHover.x, currentHover.y].layer = ChangeLayerValue();
                    currentHover = hitPosition;
                    tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
                }

                //Selecionando a peça
                if(Input.GetMouseButtonDown(0))
                {
                    if(chessPieces[hitPosition.x, hitPosition.y] != null)
                    {
                        //Is it your turn?
                        if((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn) || (chessPieces[hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn))
                        {
                            currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];

                            // Get a list of where i can go highlight tiles as well
                            availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
                            HighlightTiles();
                        }
                    }
                }

                //Soltando a peça
                if(currentlyDragging != null && Input.GetMouseButtonUp(0))
                {
                    Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                    bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                    if(!validMove)
                    {
                        currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    }
                    
                    RemoveHighlightTiles();
                    currentlyDragging = null;
                    
                    

                }
            }
            else
            {
                if(currentHover != -Vector2Int.one)
                {
                    tiles[currentHover.x,currentHover.y].layer = ChangeLayerValue();
                    currentHover = -Vector2Int.one;
                }
                
                if (currentlyDragging && Input.GetMouseButtonUp(0))
                {
                    currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                    currentlyDragging = null;
                    RemoveHighlightTiles(); //toda vez que o currentlydragging for null remove os highlights
                }
            }

            //Quando estivermos segurando um peça
            if(currentlyDragging)
            {
                Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
                float distance = 0.0f;
                if(horizontalPlane.Raycast(ray, out distance))
                    currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
            }
        }

        //Generando o tabuleiro
        private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
        {
            yOffset += transform.position.y;
            bounds = new Vector3((tileCountX / 2)*tileSize, 0, (tileCountX / 2)*tileSize) + boardCenter;

            tiles = new GameObject[tileCountX, tileCountY];
            for(int i = 0 ; i <  tileCountX ; i++ )
                for(int j = 0; j < tileCountY; j++)
                    tiles[i,j] = GenerateSingleTile(tileSize, i, j);
        }
        private GameObject GenerateSingleTile(float tileSize, int x, int y)
        {
            //nomeando de acordo com a posição
            GameObject tileObject = new GameObject(string.Format("X:{0} Y:{1}", x, y));
            tileObject.transform.parent = transform;

            Mesh mesh = new Mesh();
            tileObject.AddComponent<MeshFilter>().mesh = mesh;
            tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(x * tileSize, yOffset, y* tileSize) - bounds;
            vertices[1] = new Vector3( x* tileSize, yOffset, (y+1)* tileSize) - bounds;
            vertices[2] = new Vector3((x+1) * tileSize, yOffset, y* tileSize) - bounds;
            vertices[3] = new Vector3((x+1) * tileSize, yOffset, (y+1)* tileSize) - bounds;

            int[] tris = new int[]{0, 1, 2, 1, 3, 2};

            mesh.vertices = vertices;
            mesh.triangles = tris;

            mesh.RecalculateNormals();

            tileObject.layer = LayerMask.NameToLayer("Tile");
            tileObject.AddComponent<BoxCollider>();
            
            return tileObject;
        }

        //Spawnando as peças
        private void SpawnAllPieces()
        {
            chessPieces = new ChessPiece[TILE_COUNT_X,TILE_COUNT_Y];

            int whiteTeam = 0, blackTeam = 1;

            //White team
            chessPieces[0,0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);
            chessPieces[1,0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
            chessPieces[2,0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
            chessPieces[3,0] = SpawnSinglePiece(ChessPieceType.Queen, whiteTeam);
            chessPieces[4,0] = SpawnSinglePiece(ChessPieceType.King, whiteTeam);
            chessPieces[5,0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
            chessPieces[6,0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
            chessPieces[7,0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);
            for(int i = 0; i< TILE_COUNT_X; i++)
                    chessPieces[i,1] = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam);

            //White team
            chessPieces[0,7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);
            chessPieces[1,7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
            chessPieces[2,7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
            chessPieces[3,7] = SpawnSinglePiece(ChessPieceType.Queen, blackTeam);
            chessPieces[4,7] = SpawnSinglePiece(ChessPieceType.King, blackTeam);
            chessPieces[5,7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
            chessPieces[6,7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
            chessPieces[7,7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);
            for(int i = 0; i< TILE_COUNT_X; i++)
                    chessPieces[i,6] = SpawnSinglePiece(ChessPieceType.Pawn, blackTeam);

        }
        private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
        {
            ChessPiece piece = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

            piece.type = type;
            piece.team = team;
            piece.GetComponent<MeshRenderer>().material = teamMaterials[team];
            piece.SetScale(ajuste, true);

            return piece;
        }

        //Posicionando as peças
        private void PositionAllPieces()
        {
            for(int i = 0; i < TILE_COUNT_X; i++)
                for(int j = 0; j < TILE_COUNT_Y; j++)
                    if(chessPieces[i,j] != null)
                        PositionSinglePiece(i, j, true);
        }
        private void PositionSinglePiece(int x, int y, bool force = false)
        {
            chessPieces[x,y].currentX = x;
            chessPieces[x,y].currentY = y;
            chessPieces[x,y].SetPosition(GetTileCenter(x, y), force);
        }
        private Vector3 GetTileCenter(int x, int y)
        {
            return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize/2, 0, tileSize/2);
        }

        // Highlighting Tiles
        private void HighlightTiles()
        {
            for(int i = 0; i< availableMoves.Count; i++)
                tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
        }
        private void RemoveHighlightTiles()
        {
            for(int i = 0; i< availableMoves.Count; i++)
                tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");

                availableMoves.Clear();
        }

        // Checkmate
        private void CheckMate(int team)
        {
            DisplayVictory(team);
        }
        private void DisplayVictory(int winningTeam)
        {
            victoryScreen.SetActive(true);
            victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
        }
        public void OnResetButton()
        {
            //UI
            victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
            victoryScreen.transform.GetChild(1).gameObject.SetActive(false);
            victoryScreen.SetActive(false);

            // Fields reset
            currentlyDragging = null;
            availableMoves = new List<Vector2Int>();

            // Clean up
            for(int i = 0; i < TILE_COUNT_X; i++)
            {
                for(int j = 0; j < TILE_COUNT_Y; j++ )
                {
                    if(chessPieces[i,j] != null)
                        Destroy(chessPieces[i,j].gameObject);

                    chessPieces[i,j] = null;
                }
            }

            for (int i = 0; i < deadWhites.Count; i++)
                Destroy(deadWhites[i].gameObject);
            for (int i = 0; i < deadBlacks.Count; i++)
                Destroy(deadBlacks[i].gameObject);

            deadWhites.Clear();
            deadBlacks.Clear();

            SpawnAllPieces();
            PositionAllPieces();
            isWhiteTurn = true;

        }
        public void OnExitButton()
        {
            //Change scene
        }

        //Operaçoes
        private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
        {
            for(int i = 0; i < moves.Count; i++)
                if(moves[i].x == pos.x && moves[i].y == pos.y)
                    return true;

            return false;
        }
        private Vector2Int LookupTileIndex(GameObject hitInfo)
        {
            for(int i = 0; i < TILE_COUNT_X; i++)
                for(int j = 0; j < TILE_COUNT_Y; j++)
                    if(tiles[i,j] == hitInfo)
                        return new Vector2Int(i,j);

            return -Vector2Int.one; // -1 -1
        }
        private bool MoveTo(ChessPiece piece, int x, int y)
        {
            if(!ContainsValidMove(ref availableMoves, new Vector2(x,y)))
                return false;

            Vector2Int previousPosition = new Vector2Int(piece.currentX, piece.currentY);

            //Is there another piece on the target position?
            if(chessPieces[x, y] != null)
            {
                ChessPiece otherPiece = chessPieces[x, y];

                if(piece.team == otherPiece.team)
                    return false;

                if(otherPiece.team == 0)
                {
                    if(otherPiece.type == ChessPieceType.King)
                        CheckMate(1);

                    deadWhites.Add(otherPiece);
                    otherPiece.SetScale(ajuste * deathSize);
                    otherPiece.SetPosition(new Vector3(8*tileSize, yOffset, - 1 * tileSize) - bounds 
                                            + new Vector3(tileSize / 2,0,tileSize/2)
                                            + (Vector3.forward * deathSpacing) * deadWhites.Count);
                }
                else
                {
                    if(otherPiece.type == ChessPieceType.King)
                        CheckMate(0);

                    deadBlacks.Add(otherPiece);
                    otherPiece.SetScale(ajuste * deathSize);
                    otherPiece.SetPosition(new Vector3(-1*tileSize, yOffset, 8 * tileSize) - bounds 
                                        + new Vector3(tileSize / 2,0,tileSize/2)
                                        + (Vector3.back * deathSpacing) * deadBlacks.Count);
                }
            }

            chessPieces[x, y] = piece;
            chessPieces[previousPosition.x, previousPosition.y] = null;

            PositionSinglePiece(x, y);

            isWhiteTurn = !isWhiteTurn;

            return true;
        }

        //Auxiliares
        private int ChangeLayerValue()
        {
            if(ContainsValidMove(ref availableMoves, currentHover))
                return LayerMask.NameToLayer("Highlight");
            else
                return LayerMask.NameToLayer("Tile");
        }

    }
}