using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class PlayableAnimation : MonoBehaviour
{
    private PlayableGraph m_Graph;
    public AnimationClip[] forwardMove;

    [Range(0, 1)]
    public float value;

    private Playable1DBlendTree m_OneDBlendBehaviour;

    void Start()
    {
        m_Graph = PlayableGraph.Create("Playable");

        AnimationPlayableOutput animationOut = AnimationPlayableOutput.Create(m_Graph, "Animation", GetComponent<Animator>());

        var oneDBlendTree = ScriptPlayable<Playable1DBlendTree>.Create(m_Graph);
        m_OneDBlendBehaviour = oneDBlendTree.GetBehaviour();
        var oneDBlend = m_OneDBlendBehaviour.Init(forwardMove, m_Graph);
        animationOut.SetSourcePlayable(oneDBlend, 0);


        m_Graph.Play();

    }

    private void Update()
    {
        m_OneDBlendBehaviour.SetInputValue(value);
    }



}
