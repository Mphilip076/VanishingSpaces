using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 4x4 Picture Sliding Puzzle — Horror Game Mini-Puzzle
/// Attach this script to a GameObject in your scene (e.g., "PuzzleManager")
/// No move counter or timer — solve to trigger the next event.
/// </summary>
public class SlidingPuzzle : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSize = 4;                        // 4x4 grid

    [Header("UI References")]
    public Transform gridParent;                    // The Grid Layout Group parent
    public GameObject tilePrefab;                   // Prefab: Button + Image
    public Texture2D puzzleImage;                   // The image to slice into tiles
    public Image previewImage;                      // Small corner preview of full image
    public GameObject winPanel;                     // Panel shown on solve
    public TMP_Text winMessageText;                 // Win message text (optional)

    [Header("Shuffle Settings")]
    [Range(50, 500)]
    public int shuffleMoves = 200;                  // Higher = harder puzzle

    [Header("Animation")]
    public float slideSpeed = 0.12f;                // Tile slide duration in seconds

    // ── Internal State ────────────────────────────────────────────────
    private int totalTiles;                         // gridSize * gridSize
    private int emptyIndex;                         // Current index of the empty slot
    private int[] tileOrder;                        // tileOrder[boardPos] = correctTileIndex
    private GameObject[] tileObjects;               // UI tile GameObjects
    private Sprite[] tileSprites;                   // Sliced image sprites

    private bool gameActive = false;
    private bool isAnimating = false;

    // ─────────────────────────────────────────────────────────────────

    void Start()
    {
        totalTiles = gridSize * gridSize;
        tileOrder = new int[totalTiles];
        tileObjects = new GameObject[totalTiles];

        SliceImage();
        BuildBoard();
        SetupPreview();

        winPanel.SetActive(false);
        StartCoroutine(ShuffleAndBegin());
    }



    // ── Image Slicing ──────────────────────────────────────────────────

    /// <summary>
    /// Slices puzzleImage into gridSize*gridSize sprites.
    /// Tiles are indexed left-to-right, bottom-to-top (Unity texture origin).
    /// </summary>
    void SliceImage()
    {
        tileSprites = new Sprite[totalTiles];

        int tileW = puzzleImage.width / gridSize;
        int tileH = puzzleImage.height / gridSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                int index = row * gridSize + col;

                Texture2D tileTex = new Texture2D(tileW, tileH);
                Color[] pixels = puzzleImage.GetPixels(col * tileW, row * tileH, tileW, tileH);
                tileTex.SetPixels(pixels);
                tileTex.Apply();

                tileSprites[index] = Sprite.Create(
                    tileTex,
                    new Rect(0, 0, tileW, tileH),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }
    }

    // ── Board Construction ─────────────────────────────────────────────

    /// <summary>
    /// Instantiates tile GameObjects and sets them to the solved state.
    /// The last tile (index totalTiles-1) starts as the empty slot.
    /// </summary>
    void BuildBoard()
    {
        // Clear any existing children
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        emptyIndex = totalTiles - 1;

        for (int i = 0; i < totalTiles; i++)
        {
            tileOrder[i] = i;

            GameObject tile = Instantiate(tilePrefab, gridParent);
            tile.name = "Tile_" + i;
            tileObjects[i] = tile;

            int capturedIndex = i;
            tile.GetComponent<Button>().onClick.AddListener(() => OnTileClicked(capturedIndex));

            if (i == emptyIndex)
            {
                // Empty tile: invisible
                tile.GetComponent<Image>().color = Color.clear;
                tile.GetComponent<Button>().interactable = false;
            }
            else
            {
                tile.GetComponent<Image>().sprite = tileSprites[i];
                tile.GetComponent<Image>().color = Color.white;
            }
        }
    }

    void SetupPreview()
    {
        if (previewImage != null && puzzleImage != null)
        {
            previewImage.sprite = Sprite.Create(
                puzzleImage,
                new Rect(0, 0, puzzleImage.width, puzzleImage.height),
                new Vector2(0.5f, 0.5f)
            );
        }
    }

    // ── Shuffling ──────────────────────────────────────────────────────

    /// <summary>
    /// Shuffles by making random valid moves — guarantees solvability.
    /// </summary>
    IEnumerator ShuffleAndBegin()
    {
        yield return new WaitForSeconds(0.3f);

        int lastMoved = -1;
        for (int i = 0; i < shuffleMoves; i++)
        {
            List<int> neighbors = GetAdjacentToEmpty();
            neighbors.Remove(lastMoved); // Avoid immediate reversal

            int chosen = neighbors[Random.Range(0, neighbors.Count)];
            SwapWithEmpty(chosen, animate: false);
            lastMoved = emptyIndex; // After swap, emptyIndex moved to old chosen
        }

        RefreshAllTileVisuals();
        gameActive = true;
    }

    // ── Tile Interaction ───────────────────────────────────────────────

    void OnTileClicked(int boardPosition)
    {
        if (!gameActive || isAnimating) return;
        if (!IsAdjacentToEmpty(boardPosition)) return;

        StartCoroutine(AnimateSlide(boardPosition));
    }

    IEnumerator AnimateSlide(int boardPosition)
    {
        isAnimating = true;

        RectTransform movingTile = tileObjects[boardPosition].GetComponent<RectTransform>();
        RectTransform emptySlot  = tileObjects[emptyIndex].GetComponent<RectTransform>();

        Vector3 startPos = movingTile.localPosition;
        Vector3 endPos   = emptySlot.localPosition;

        float elapsed = 0f;
        while (elapsed < slideSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideSpeed);
            movingTile.localPosition = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        movingTile.localPosition = endPos;

        SwapWithEmpty(boardPosition, animate: false);
        RefreshAllTileVisuals();

        isAnimating = false;

        if (CheckWin())
            OnPuzzleSolved();
    }

    // ── Core Logic ─────────────────────────────────────────────────────

    bool IsAdjacentToEmpty(int index)
    {
        int row  = index / gridSize,      col  = index % gridSize;
        int eRow = emptyIndex / gridSize, eCol = emptyIndex % gridSize;
        return Mathf.Abs(row - eRow) + Mathf.Abs(col - eCol) == 1;
    }

    List<int> GetAdjacentToEmpty()
    {
        List<int> result = new List<int>();
        int eRow = emptyIndex / gridSize;
        int eCol = emptyIndex % gridSize;

        if (eRow > 0)            result.Add((eRow - 1) * gridSize + eCol); // Above
        if (eRow < gridSize - 1) result.Add((eRow + 1) * gridSize + eCol); // Below
        if (eCol > 0)            result.Add(eRow * gridSize + (eCol - 1)); // Left
        if (eCol < gridSize - 1) result.Add(eRow * gridSize + (eCol + 1)); // Right

        return result;
    }

    /// <summary>
    /// Swaps tileOrder values at boardPosition and emptyIndex,
    /// then updates emptyIndex.
    /// </summary>
    void SwapWithEmpty(int boardPosition, bool animate = true)
    {
        int temp = tileOrder[boardPosition];
        tileOrder[boardPosition] = tileOrder[emptyIndex];
        tileOrder[emptyIndex] = temp;
        emptyIndex = boardPosition;
    }

    bool CheckWin()
    {
        for (int i = 0; i < totalTiles; i++)
            if (tileOrder[i] != i) return false;
        return true;
    }

    // ── Visuals ────────────────────────────────────────────────────────

    /// <summary>
    /// Re-assigns sprites to tile GameObjects based on current tileOrder.
    /// </summary>
    void RefreshAllTileVisuals()
    {
        for (int i = 0; i < totalTiles; i++)
        {
            Image img    = tileObjects[i].GetComponent<Image>();
            Button btn   = tileObjects[i].GetComponent<Button>();

            int correctTile = tileOrder[i];

            if (i == emptyIndex)
            {
                img.sprite = null;
                img.color  = Color.clear;
                btn.interactable = false;
            }
            else
            {
                img.sprite = tileSprites[correctTile];
                img.color  = Color.white;
                btn.interactable = true;
            }
        }
    }

    // ── Win / Lose ─────────────────────────────────────────────────────

    void OnPuzzleSolved()
    {
        gameActive = false;
        winPanel.SetActive(true);

        if (winMessageText != null)
            winMessageText.text = "Puzzle Solved...";

        // TODO: Trigger your next horror event here
        // e.g.: GameManager.instance.OnPuzzleComplete();
    }

    // ── Public Buttons ─────────────────────────────────────────────────

    /// <summary>Call from a "Restart" button in the UI.</summary>
    public void RestartPuzzle()
    {
        winPanel.SetActive(false);
        gameActive = false;
        BuildBoard();
        StartCoroutine(ShuffleAndBegin());
    }
}