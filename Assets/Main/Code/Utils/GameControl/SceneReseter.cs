using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReseter : MonoBehaviour
{
    [SerializeField] private KeyCode _resetSceneButton = KeyCode.R;

    private void Update()
    {
        if (Input.GetKeyDown(_resetSceneButton))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}