using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
//顺序播放动画
public class CustomPlayableBehaviour : PlayableBehaviour
{
    private int m_CurrentClipIndex = -1;
    private float m_TimeToNextClip;
    private Playable m_Mixer;

    public void Init(AnimationClip[] clipsToPlay, Playable owner, PlayableGraph graph)
    {
        owner.SetInputCount(1);
        m_Mixer = AnimationMixerPlayable.Create(graph, clipsToPlay.Length);
        graph.Connect(m_Mixer, 0, owner, 0);
        owner.SetInputWeight(0, 1);
        for (int clipIndex = 0; clipIndex < m_Mixer.GetInputCount(); ++clipIndex)
        {
            graph.Connect(AnimationClipPlayable.Create(graph, clipsToPlay[clipIndex]), 0, m_Mixer, clipIndex);
            m_Mixer.SetInputWeight(clipIndex, 1.0f);
        }
    }


    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {

    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {

    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if (m_Mixer.GetInputCount() == 0)
            return;
        // Advance to next clip if necessary
        m_TimeToNextClip -= (float)info.deltaTime;
        if (m_TimeToNextClip <= 0.0f)
        {
            m_CurrentClipIndex++;
            if (m_CurrentClipIndex >= m_Mixer.GetInputCount())
                m_CurrentClipIndex = 0;
            var currentClip = (AnimationClipPlayable)m_Mixer.GetInput(m_CurrentClipIndex);
            // Reset the time so that the next clip starts at the correct position
            currentClip.SetTime(0);
            m_TimeToNextClip = currentClip.GetAnimationClip().length;
        }
        // Adjust the weight of the inputs
        for (int clipIndex = 0; clipIndex < m_Mixer.GetInputCount(); ++clipIndex)
        {
            if (clipIndex == m_CurrentClipIndex)
                m_Mixer.SetInputWeight(clipIndex, 1.0f);
            else
                m_Mixer.SetInputWeight(clipIndex, 0.0f);
        }
    }
}
