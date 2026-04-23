using UnityEngine;
using UnityEngine.UI;

public class SlidingPuzzle : MonoBehaviour
{
    [Header("Puzzle Settings")]
    public Sprite puzzleImage;
    public GameObject spawnItem;
    public Transform spawnPoint;

    [Header("Interaction")]
    public float interactRange = 3f;
    public string interactMessage = "Press E to open puzzle";

    [Header("UI")]
    public GameObject puzzleUI;
    public GridLayoutGroup gridLayout;

    private const int size = 4;
    private int[,] board = new int[size, size];
    private int[,] initialBoard = new int[size, size];
    private GameObject[] tiles;
    private int emptyRow, emptyCol;
    private bool isSolved = false;
    private bool isUIOpen = false;
    private bool playerNearby = false;

    void Start()
    {
        if (puzzleUI != null)
            puzzleUI.SetActive(false);

        if (PlayerPrefs.GetInt("sliding_puzzle_solved", 0) == 1)
        {
            isSolved = true;
            SpawnItem();
        }
    }

    void Update()
    {
        if (isSolved) return;

        // Check if player is nearby
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange);
        playerNearby = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        if (playerNearby)
        {
            PersistCanvas.ShowPrompt(interactMessage);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isUIOpen)
                    OpenPuzzleUI();
                else
                    ClosePuzzleUI();
            }
        }
        else
        {
            if (!isUIOpen)
                PersistCanvas.HidePrompt();
        }
    }

    void OpenPuzzleUI()
    {
        isUIOpen = true;
        puzzleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        PlayerMovement pm = FindAnyObjectByType<PlayerMovement>();
        if (pm != null) pm.enabled = false;

        GeneratePuzzle();
    }

    public void ClosePuzzleUI()
    {
        isUIOpen = false;
        puzzleUI.SetActive(false);

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
    }

    void SpawnItem()
    {
        if (spawnItem == null || spawnPoint == null) return;
        Instantiate(spawnItem, spawnPoint.position, Quaternion.identity);
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

        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        tiles = new GameObject[size * size];
        RenderBoard();
    }
}