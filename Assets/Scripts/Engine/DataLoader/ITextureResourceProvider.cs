﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Engine.DataLoader
{
    using UnityEngine;

    /// <summary>
    /// Texture2D provider.
    /// </summary>
    public interface ITextureResourceProvider
    {
        string GetTexturePath(string name);

        Texture2D GetTexture(string name);

        Texture2D GetTexture(string name, out bool hasAlphaChannel);
    }
}