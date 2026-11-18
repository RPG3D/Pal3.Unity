// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2025, Jiaqi (0x7c13) Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Engine.Core.Implementation
{
    using System.Collections;
    using Abstraction;
    using UnityEngine;

    // UnrealSharp portability. Unity resolves the entity and native
    // collision messages from its own component lifecycle.
    public abstract partial class GameEntityScript
    {
        private IGameEntity _gameEntity;

        private partial IGameEntity ResolveGameEntity() => _gameEntity ??= new GameEntity(gameObject);

        protected new void StartCoroutine(IEnumerator coroutine)
        {
            base.StartCoroutine(coroutine);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject != null)
            {
                OnCollisionEnterGameEntity(new GameEntity(collision.gameObject));
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject != null)
            {
                OnCollisionExitGameEntity(new GameEntity(collision.gameObject));
            }
        }

        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.gameObject != null)
            {
                OnTriggerEnterGameEntity(new GameEntity(otherCollider.gameObject));
            }
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.gameObject != null)
            {
                OnTriggerExitGameEntity(new GameEntity(otherCollider.gameObject));
            }
        }
    }
}
