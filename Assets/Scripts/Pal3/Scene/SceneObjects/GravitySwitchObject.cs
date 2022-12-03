﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2022, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Scene.SceneObjects
{
    using System.Collections;
    using Actor;
    using Command;
    using Command.InternalCommands;
    using Command.SceCommands;
    using Common;
    using Core.Animation;
    using Core.DataReader.Scn;
    using Core.Services;
    using Data;
    using Player;
    using State;
    using UnityEngine;

    [ScnSceneObject(ScnSceneObjectType.GravitySwitch)]
    public sealed class GravitySwitchObject : SceneObject
    {
        private const float DESCENDING_HEIGHT = 0.5f;
        private const float DESCENDING_ANIMATION_DURATION = 2.5f;

        private StandingPlatformController _platformController;

        private readonly PlayerManager _playerManager;
        private readonly SceneManager _sceneManager;
        private readonly TeamManager _teamManager;

        public GravitySwitchObject(ScnObjectInfo objectInfo, ScnSceneInfo sceneInfo)
            : base(objectInfo, sceneInfo)
        {
            _playerManager = ServiceLocator.Instance.Get<PlayerManager>();
            _sceneManager = ServiceLocator.Instance.Get<SceneManager>();
            _teamManager = ServiceLocator.Instance.Get<TeamManager>();
        }

        public override GameObject Activate(GameResourceProvider resourceProvider, Color tintColor)
        {
            if (Activated) return GetGameObject();

            GameObject sceneGameObject = base.Activate(resourceProvider, tintColor);

            // wz06.cvd
            var bounds = new Bounds
            {
                center = new Vector3(0f, 0.8f, 0f),
                size = new Vector3(4f, 1f, 4f),
            };

            _platformController = sceneGameObject.AddComponent<StandingPlatformController>();
            _platformController.SetBounds(bounds, ObjectInfo.LayerIndex);
            _platformController.OnPlayerActorEntered += OnPlayerActorEntered;

            // Set to final position if it is already activated
            if (ObjectInfo.Times == 0)
            {
                Vector3 finalPosition = sceneGameObject.transform.position;
                finalPosition.y -= DESCENDING_HEIGHT;
                sceneGameObject.transform.position = finalPosition;
            }

            return sceneGameObject;
        }

        private void OnPlayerActorEntered(object sender, GameObject playerActorGameObject)
        {
            // Check if total team members are equal to or greater than required headcount
            if (_teamManager.GetActorsInTeam().Count >= ObjectInfo.Parameters[0])
            {
                if (!IsInteractableBasedOnTimesCount()) return;
                CommandDispatcher<ICommand>.Instance.Dispatch(
                    new GameStateChangeRequest(GameState.Cutscene));
                Pal3.Instance.StartCoroutine(Interact(true));
            }
            else if (ObjectInfo.Times > 0)
            {
                CommandDispatcher<ICommand>.Instance.Dispatch(new UIDisplayNoteCommand("重量不足，无法激活"));
            }
        }

        public override IEnumerator Interact(bool triggerredByPlayer)
        {
            GameObject gravityTriggerGo = GetGameObject();
            var platformController = gravityTriggerGo.GetComponent<StandingPlatformController>();
            Vector3 platformPosition = platformController.transform.position;
            var actorStandingPosition = new Vector3(
                platformPosition.x,
                platformController.GetPlatformHeight(),
                platformPosition.z);

            var actorMovementController = _sceneManager.GetCurrentScene()
                .GetActorGameObject((int)_playerManager.GetPlayerActor())
                .GetComponent<ActorMovementController>();

            yield return actorMovementController.MoveDirectlyTo(actorStandingPosition, 0);

            CommandDispatcher<ICommand>.Instance.Dispatch(new PlaySfxCommand("we026", 1));

            var cvdModelRenderer = GetCvdModelRenderer();
            yield return cvdModelRenderer.PlayOneTimeAnimation(true);

            CommandDispatcher<ICommand>.Instance.Dispatch(new PlaySfxCommand("wg005", 1));

            Vector3 finalPosition = gravityTriggerGo.transform.position;
            finalPosition.y -= DESCENDING_HEIGHT;
            yield return AnimationHelper.MoveTransform(gravityTriggerGo.transform,
                finalPosition,
                DESCENDING_ANIMATION_DURATION,
                AnimationCurveType.Sine);

            // Don't wait for linked object to finish interaction before enabling player control
            CommandDispatcher<ICommand>.Instance.Dispatch(
                new GameStateChangeRequest(GameState.Gameplay));

            yield return ActivateOrInteractWithLinkedObjectIfAny();
        }

        public override void Deactivate()
        {
            if (_platformController != null)
            {
                _platformController.OnPlayerActorEntered -= OnPlayerActorEntered;
                Object.Destroy(_platformController);
            }

            base.Deactivate();
        }
    }
}