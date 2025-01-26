using UnityEngine;
using UnityEngine.Video;

namespace GGJ2025
{
    public class VideoController : MonoBehaviour
    {
        #region Variable

        [SerializeField] VideoClip m_Clip;

        [SerializeField] VideoClip m_Clip2;

        VideoPlayer m_VideoPlayer;

        int m_Index;

        #endregion

        #region Main
        private void Awake()
        {
            m_VideoPlayer = GetComponent<VideoPlayer>();

            m_VideoPlayer.loopPointReached += M_VideoPlayer_loopPointReached;

            PlayVideo();
        }

        private void M_VideoPlayer_loopPointReached(VideoPlayer source)
        {
            
            if (m_Index > 0)
                return;
            //m_VideoPlayer.Stop();

            var clip = m_Clip2;
            m_VideoPlayer.clip = clip;
            m_VideoPlayer.Play();

            m_Index++;
        }

        public void PlayVideo()
        {
            m_VideoPlayer.clip = m_Clip;
            m_VideoPlayer.Play();
        }

        #endregion

    }
}