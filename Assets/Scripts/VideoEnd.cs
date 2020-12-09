using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEnd : MonoBehaviour
{
    VideoPlayer clip;

    void Awake()
    {
        clip = GetComponent<VideoPlayer>();
        clip.Play();
        clip.loopPointReached += Verify;
    }

    void Verify(VideoPlayer v)
    {
        SceneManager.LoadScene(0);
    }
}
