// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2025, Jiaqi (0x7c13) Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Engine.Core.Implementation
{
    using Abstraction;
    using UnityEngine;

    // UnrealSharp portability. Keep gameplay hooks shared and let each
    // engine resolve the entity associated with this behaviour.
    public abstract partial class GameEntityScript : MonoBehaviour
    {
        public ITransform Transform => GameEntity.Transform;
        public IGameEntity GameEntity => ResolveGameEntity();

        private partial IGameEntity ResolveGameEntity();

        protected virtual void OnEnableGameEntity() {}

        protected virtual void OnDisableGameEntity() {}

        protected virtual void OnCollisionEnterGameEntity(IGameEntity other) {}

        protected virtual void OnCollisionExitGameEntity(IGameEntity other) {}

        protected virtual void OnTriggerEnterGameEntity(IGameEntity other) {}

        protected virtual void OnTriggerExitGameEntity(IGameEntity other) {}

        private void OnEnable()
        {
            OnEnableGameEntity();
        }

        private void OnDisable()
        {
            OnDisableGameEntity();
        }
    }
}
