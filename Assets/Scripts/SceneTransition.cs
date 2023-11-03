using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transition;
    public string sceneToLoad;
    public float transitionTime = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.HasTag("Player"))
        {
            return;
        }
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
