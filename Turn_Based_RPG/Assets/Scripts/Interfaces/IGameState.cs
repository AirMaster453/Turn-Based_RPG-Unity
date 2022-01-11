using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PsychesBound
{
    /// <summary>
    /// Interface for utilizing game states
    /// </summary>
    public interface IGameState
    {
        GameManager GameManager { get; set; }
        UniTask OnEnter();

        void OnUpdate();

        UniTask OnExit();
    }
}