﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2022, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Pal3.Scene.SceneObjects
{
    using Command;
    using Command.InternalCommands;
    using Core.DataReader.Scn;
    using Data;
    using UnityEngine;

    [ScnSceneObject(ScnSceneObjectType.SceneSfx)]
    public class SceneSfxObject : SceneObject
    {
        public string SfxName { get; }

        public SceneSfxObject(ScnObjectInfo objectInfo, ScnSceneInfo sceneInfo)
            : base(objectInfo, sceneInfo, hasModel: false)
        {
            SfxName = objectInfo.Name;
        }

        public override GameObject Activate(GameResourceProvider gameResourceProvider, Color tintColor)
        {
            var sceneGameObject = base.Activate(gameResourceProvider, tintColor);

            // We want some random delay before playing the scene sfx
            // since there might be more than one audio source in the scene
            // playing the exact same audio sfx, which will cause "Comb filter" effect.
            var startDelay = Random.Range(0f, 1f);

            CommandDispatcher<ICommand>.Instance.Dispatch(
                new PlaySfxAtGameObjectRequest(SfxName, 0, startDelay, sceneGameObject));

            return sceneGameObject;
        }
    }
}