using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

/// <summary>
/// 对应Animator中的1D Blend Tree
/// </summary>
public class Playable1DBlendTree : PlayableBehaviour
{
    private Playable m_Mixer;
    private float m_MinValue;
    private float m_MaxValue;
    private float m_CurValue = 0.0f;

    private int m_Length;
    private BlendNode_1D[] m_BlendNodes;

    public Playable Init(AnimationClip[] clipsToPlay, PlayableGraph graph, float min = 0, float max = 1)
    {
        this.m_MinValue = min;
        this.m_MaxValue = max;
        this.m_Length = clipsToPlay.Length;
        this.m_BlendNodes = new BlendNode_1D[m_Length];
        m_Mixer = AnimationMixerPlayable.Create(graph, clipsToPlay.Length);

        float single = (m_MaxValue - m_MinValue) / (m_Length - 1);
        for (int i = 0; i < m_Length; i++)
        {
            m_BlendNodes[i].value = single * i + m_MinValue;
            m_BlendNodes[i].length = clipsToPlay[i].length;
            // Debug.LogError(m_BlendNode[i].weight + "::" + m_BlendNode[i].length);
            var clipPlayable = AnimationClipPlayable.Create(graph, clipsToPlay[i]);
            clipPlayable.SetSpeed(m_BlendNodes[i].length);
            m_Mixer.ConnectInput(i, clipPlayable, 0);
            m_Mixer.SetInputWeight(i, 0);//默认是平均的
        }
        CalcWeight();
        return m_Mixer;
    }

    public void SetInputValue(float value)
    {
        m_CurValue = value;
        CalcWeight();
    }

    private void CalcWeight()
    {
        int targetIndex = 0;
        for (int i = 0; i < m_BlendNodes.Length - 1; i++)
        {
            if (m_CurValue >= m_BlendNodes[i].value && m_CurValue <= m_BlendNodes[i + 1].value)
            {
                targetIndex = i;
            }
        }

        for (int i = 0; i < m_Length; i++)
        {
            if (i == targetIndex || i == targetIndex + 1)
            {
                // float targetTarget = single * i;
            }
            else
            {
                m_Mixer.SetInputWeight(i, 0);//默认是平均的
            }

        }

        float weight = (m_CurValue - m_BlendNodes[targetIndex].value) / (m_BlendNodes[targetIndex + 1].value - m_BlendNodes[targetIndex].value);
        // Debug.LogError(m_CurValue + "====" + targetIndex + "===" + weight);
        m_Mixer.SetInputWeight(targetIndex, 1 - weight);
        m_Mixer.SetInputWeight(targetIndex + 1, weight);
        float speed = m_BlendNodes[targetIndex].length * (1 - weight) + m_BlendNodes[targetIndex + 1].length * weight;
        m_Mixer.SetSpeed(1.0f / speed);
    }


    // // Called when the owning graph starts playing
    // public override void OnGraphStart(Playable playable)
    // {
    //     Debug.LogError("OnGraphStart");
    // }

    // // Called when the owning graph stops playing
    // public override void OnGraphStop(Playable playable)
    // {
    //     Debug.LogError("OnGraphStop");
    // }

    // // Called when the state of the playable is set to Play
    // public override void OnBehaviourPlay(Playable playable, FrameData info)
    // {
    //     Debug.LogError("OnBehaviourPlay");
    // }

    // // Called when the state of the playable is set to Paused
    // public override void OnBehaviourPause(Playable playable, FrameData info)
    // {
    //     Debug.LogError("OnBehaviourPause");
    // }

    // // Called each frame while the state is set to Play
    // public override void PrepareFrame(Playable playable, FrameData info)
    // {
    //     Debug.LogError("PrepareFrame");
    // }
}


public struct BlendNode_1D
{
    public float value;
    public float length;
}
