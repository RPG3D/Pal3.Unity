﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

namespace Core.Command.SceCommands
{
    [SceCommand(62, "显示对话，等待用户确定后脚本继续")]
    public class DialogueRenderTextCommand : ICommand
    {
        public DialogueRenderTextCommand(string dialogueText)
        {
            DialogueText = dialogueText;
        }

        public string DialogueText { get; }
    }
}