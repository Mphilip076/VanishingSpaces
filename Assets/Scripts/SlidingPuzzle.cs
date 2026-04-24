using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzle : InteractableItem
{
    [Header("Puzzle Settings")]
    public Sprite puzzleImage;
    public GameObject spawnItem;
    public Transform spawnPoint;

    [Header("UI")]
    public GameObject puzzleUI;
    public GridLayoutGroup gridLayout;

    [Header("Auto Solve")]
    public float autoSolveDelay = 60f;
    public TextMeshProUGUI timerText;
    public GameObject autoSolveButton;

    private const int size = 4;
    private int[,] board = new int[size, size];
    private int[,] initialBoard = new int[size, size];
    private GameObject[] tiles;
    private int emptyRow, emptyCol;
    private bool isSolved = false;
    private bool isUIOpen = false;
    private float timeOpen = 0f;

    void Start()
    {
        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (PlayerPrefs.GetInt("sliding_puzzle_solved", 0) == 1)
        {
            isSolved = true;
            SpawnItem();
        }

        interactMessage = "Press E to open puzzle";
        interactKey = KeyCode.E;
        interactRange = 3f;
    }

    void Update()
    {
        if (!isUIOpen || isSolved) return;

        timeOpen += Time.deltaTime;
        float remaining = Mathf.Max(0f, autoSolveDelay - timeOpen);

        if (timeOpen < autoSolveDelay)
        {
            if (timerText != null)
                timerText.text = $"Auto-solve unlocks in {Mathf.CeilToInt(remaining)}s";
        }
        else
        {
            if (timerText != null)
                timerText.text = "Auto-solve available!";

            if (autoSolveButton != null && !autoSolveButton.activeSelf)
                autoSolveButton.SetActive(true);
        }
    }

    public override void OnInteract()
    {
        if (isUIOpen) ClosePuzzleUI();
        else OpenPuzzleUI();
    }

    void OpenPuzzleUI()
    {
        isUIOpen = true;
        timeOpen = 0f;
        if (autoSolveButton != null) autoSolveButton.SetActive(false);
        puzzleUI.SetActive(true);

        Canvas.ForceUpdateCanvases();
        FitGridToPanel();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        GeneratePuzzle();
    }

    void FitGridToPanel()
    {
        RectTransform gridRect = gridLayout.GetComponent<RectTransform>();
        RectTransform panelRect = gridRect.parent as RectTransform;
        if (panelRect == null) return;

        float boardSize = Mathf.Min(panelRect.rect.width, panelRect.rect.height) * 0.85f;
        float cellSize  = boardSize / size;

        gridRect.anchorMin        = new Vector2(0.5f, 0.5f);
        gridRect.anchorMax        = new Vector2(0.5f, 0.5f);
        gridRect.pivot            = new Vector2(0.5f, 0.5f);
        gridRect.anchoredPosition = Vector2.zero;
        gridRect.sizeDelta        = new Vector2(boardSize, boardSize);

        gridLayout.cellSize = new Vector2(cellSize, cellSize);
        gridLayout.spacing  = Vector2.zero;
    }

    public void ClosePuzzleUI()
    {
        isUIOpen = false;
        puzzleUI.SetActive(false);

        if (timerText != null)
            timerText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = true;
    }

    void GeneratePuzzle()
    {
        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        tiles = new GameObject[size * size];

        int num = 1;
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                board[r, c] = num++;

        board[size - 1, size - 1] = 0;
        emptyRow = size - 1;
        emptyCol = size - 1;

        ShuffleBoard();

        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                initialBoard[r, c] = board[r, c];

        RenderBoard();
    }

    void ShuffleBoard()
    {
        for (int i = 0; i < 200; i++)
        {
            int dir = Random.Range(0, 4);
            int newRow = emptyRow, newCol = emptyCol;

            if (dir == 0) newRow--;
            else if (dir == 1) newRow++;
            else if (dir == 2) newCol--;
            else newCol++;

            if (newRow >= 0 && newRow < size && newCol >= 0 && newCol < size)
            {
                board[emptyRow, emptyCol] = board[newRow, newCol];
                board[newRow, newCol] = 0;
                emptyRow = newRow;
                emptyCol = newCol;
            }
        }
    }

    void RenderBoard()
    {
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                int num = board[r, c];
                int index = r * size + c;

                GameObject tile = new GameObject("Tile_" + num);
                tile.transform.SetParent(gridLayout.transform, false);

                Image img = tile.AddComponent<Image>();

                if (num == 0)
                {
                    img.color = new Color(0, 0, 0, 0);
                }
                else
                {
                    img.sprite = CreateTileSprite(num - 1);
                    img.color = Color.white;

                    Button btn = tile.AddComponent<Button>();
                    int capturedRow = r;
                    int capturedCol = c;
                    btn.onClick.AddListener(() => OnTileClick(capturedRow, capturedCol));
                }

                tiles[index] = tile;
            }
        }
    }

    Sprite CreateTileSprite(int tileIndex)
    {
        int row = tileIndex / size;
        int col = tileIndex % size;

        float tileW = puzzleImage.texture.width / (float)size;
        float tileH = puzzleImage.texture.height / (float)size;

        Rect rect = new Rect(col * tileW, puzzleImage.texture.height - (row + 1) * tileH, tileW, tileH);
        return Sprite.Create(puzzleImage.texture, rect, new Vector2(0.5f, 0.5f));
    }

    void OnTileClick(int row, int col)
    {
        bool isAdjacent = (Mathf.Abs(row - emptyRow) + Mathf.Abs(col - emptyCol)) == 1;
        if (!isAdjacent) return;

        board[emptyRow, emptyCol] = board[row, col];
        board[row, col] = 0;

        int clickedIndex = row * size + col;
        int emptyIndex = emptyRow * size + emptyCol;

        Image clickedImg = tiles[clickedIndex].GetComponent<Image>();
        Image emptyImg = tiles[emptyIndex].GetComponent<Image>();

        emptyImg.sprite = clickedImg.sprite;
        emptyImg.color = Color.white;

        Button emptyBtn = tiles[emptyIndex].GetComponent<Button>();
        if (emptyBtn == null) emptyBtn = tiles[emptyIndex].AddComponent<Button>();
        int newEmptyRow = emptyRow, newEmptyCol = emptyCol;
        emptyBtn.onClick.RemoveAllListeners();
        emptyBtn.onClick.AddListener(() => OnTileClick(newEmptyRow, newEmptyCol));

        clickedImg.sprite = null;
        clickedImg.color = new Color(0, 0, 0, 0);

        Button clickedBtn = tiles[clickedIndex].GetComponent<Button>();
        if (clickedBtn != null) clickedBtn.onClick.RemoveAllListeners();

        emptyRow = row;
        emptyCol = col;

        CheckWin();
    }

    void CheckWin()
    {
        int num = 1;
        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                if (r == size - 1 && c == size - 1) break;
                if (board[r, c] != num++) return;
            }
        }

        isSolved = true;
        PlayerPrefs.SetInt("sliding_puzzle_solved", 1);
        PlayerPrefs.Save();

        Invoke("ClosePuzzleUI", 1f);
        Invoke("SpawnItem", 1.5f);

        canInteract = false;
    }

    public void AutoSolve()
    {
        int num = 1;
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                board[r, c] = (r == size - 1 && c == size - 1) ? 0 : num++;

        emptyRow = size - 1;
        emptyCol = size - 1;

        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        tiles = new GameObject[size * size];
        RenderBoard();

        CheckWin();
    }

    void SpawnItem()
    {
        if (spawnItem == null || spawnPoint == null) return;

        Vector3 spawnPos = spawnPoint.position + Vector3.up * 1.2f;
        GameObject item = Instantiate(spawnItem, spawnPos, Random.rotation);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 popDir = new Vector3(Random.Range(-0.5f, 0.5f), 1f, Random.Range(-0.5f, 0.5f)).normalized;
            rb.AddForce(popDir * 4f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 3f, ForceMode.Impulse);
        }
    }

    public void RestartPuzzle()
    {
        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                board[r, c] = initialBoard[r, c];

        for (int r = 0; r < size; r++)
            for (int c = 0; c < size; c++)
                if (board[r, c] == 0)
                {
                    emptyRow = r;
                    emptyCol = c;
                }

        timeOpen = 0f;
        if (autoSolveButton != null) autoSolveButton.SetActive(false);

        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        tiles = new GameObject[size * size];
        RenderBoard();
    }
}
