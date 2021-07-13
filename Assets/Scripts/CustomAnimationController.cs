using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CustomAnimationController : MonoBehaviour
{
    // public AnimationClip[] clipsToPlay;
    public AnimationClip animationClip;
    public AudioClip audioClip;

    private PlayableGraph m_Graph;



    void Start()
    {
        m_Graph = PlayableGraph.Create("CustomAnimationController");
        m_Graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
        GraphVisualizerClient.Show(m_Graph);


        // AnimationPlayableOutput animOutput = AnimationPlayableOutput.Create(m_Graph, "Animation", GetComponent<Animator>());
        // var queuePlayableBehaviour = ScriptPlayable<CustomPlayableBehaviour>.Create(m_Graph);
        // var playQueue = queuePlayableBehaviour.GetBehaviour();
        // playQueue.Init(clipsToPlay, queuePlayableBehaviour, m_Graph);
        // animOutput.SetSourcePlayable(queuePlayableBehaviour, 0);

        AnimationPlayableOutput animOutput = AnimationPlayableOutput.Create(m_Graph, "Animation Output", GetComponent<Animator>());
        var animationPlayable = AnimationClipPlayable.Create(m_Graph, animationClip);
        animOutput.SetSourcePlayable(animationPlayable, 0);

        AudioPlayableOutput audioOutput = AudioPlayableOutput.Create(m_Graph, "Audio", GetComponent<AudioSource>());
        var audioPlayable = AudioClipPlayable.Create(m_Graph, audioClip, true);
        audioOutput.SetSourcePlayable(audioPlayable, 0);

        m_Graph.Play();
    }

    private void OnDestroy()
    {
        m_Graph.Destroy();
    }
}