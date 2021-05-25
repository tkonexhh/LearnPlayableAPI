using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class CustomAnimationController : MonoBehaviour
{
    public AnimationClip clip;
    public AnimationClip clip2;


    private PlayableGraph m_Graph;
    private AnimationPlayableOutput m_Output;
    private AnimationMixerPlayable m_Mixer;


    public float tranTime = 5;
    private float leftTime;

    void Start()
    {
        leftTime = tranTime;


        m_Graph = PlayableGraph.Create("CustomAnimationController");
        GraphVisualizerClient.Show(m_Graph);
        m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        m_Output = AnimationPlayableOutput.Create(m_Graph, "Animation", GetComponent<Animator>());

        // AnimationClipPlayable clipPlayable = AnimationClipPlayable.Create(m_Graph, clip);
        // m_Output.SetSourcePlayable(clipPlayable);
        // m_Graph.Play();

        m_Mixer = AnimationMixerPlayable.Create(m_Graph, 2);
        m_Output.SetSourcePlayable(m_Mixer);

        AnimationClipPlayable clipPlayable1 = AnimationClipPlayable.Create(m_Graph, clip);
        AnimationClipPlayable clipPlayable2 = AnimationClipPlayable.Create(m_Graph, clip2);
        m_Graph.Connect(clipPlayable1, 0, m_Mixer, 0);
        m_Graph.Connect(clipPlayable2, 0, m_Mixer, 1);
        m_Mixer.SetInputWeight(0, 1);
        m_Mixer.SetInputWeight(1, 0);

        m_Graph.Play();
    }

    void Update()
    {
        if (leftTime > 0)
        {
            leftTime = leftTime - Time.deltaTime;
            float weight = Mathf.Clamp(leftTime / tranTime, 0, 1);
            Debug.LogError(weight);
            m_Mixer.SetInputWeight(0, weight);
            m_Mixer.SetInputWeight(1, 1 - weight);
        }
    }

    private void OnDestroy()
    {
        m_Graph.Destroy();
    }
}