using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Exposition : MonoBehaviour
{

    public Animator anim;
    private SceneTransition sceneTransition;

    public Text title;
    public Text firstLine;
    public Text secondLine;
    public Text thirdLine;
    public Image image;

    public string Title1;
    public string firstLine1;
    public string secondLine1;
    public string thirdLine1;
    public Sprite image1;

    public string Title2;
    public string firstLine2;
    public string secondLine2;
    public string thirdLine2;
    public Sprite image2;

    public string Title3;
    public string firstLine3;
    public string secondLine3;
    public string thirdLine3;
    public Sprite image3;
    

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition = GetComponent<SceneTransition>();
        title.text = Title1;
        firstLine.text = firstLine1;
        secondLine.text = secondLine1;
        thirdLine.text = thirdLine1;
        image.sprite = image1;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Switch());
    }

    private IEnumerator Switch(){
        yield return new WaitForSeconds(12f);
        title.text = Title2;
        firstLine.text = firstLine2;
        secondLine.text = secondLine2;
        thirdLine.text = thirdLine2;
        image.sprite = image2;
        yield return new WaitForSeconds(12f);
        title.text = Title3;
        firstLine.text = firstLine3;
        secondLine.text = secondLine3;
        thirdLine.text = thirdLine3;
        image.sprite = image3;
        yield return new WaitForSeconds(12f);

        anim.StopPlayback();
        sceneTransition.ChangeScene();
    }
}
