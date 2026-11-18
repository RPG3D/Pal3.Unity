# PAL3.Unity 适配说明

跨引擎适配基于上游 `cfed96a2` 的 Unity `6000.5.0f1`。工程设置、包版本、资源导入配置和生成的 `PlayerInputActions.cs` 随该基线。

**UnrealSharp portability** 用于两边共享的接口和引擎适配。当前共用 `Engine/Coroutine/CoroutineYieldInstruction.cs`，Unity 侧的 `UnityCoroutineWaitBackend.cs` 将它接到 Unity 原生 yield 指令。同步时保持这个公共入口与 SharpDemo 的同路径 C# 文件一致。

`Engine/Core/Implementation/GameEntityScript.cs` 也由两端共用。`Transform` 通过已有的 `GameEntity` 取得。同目录的 `GameEntityScript.Unity.cs` 保留 Unity 的组件查找、协程入口和原生碰撞消息。

优先在 UE 工程的托管 wrapper 中实现游戏用到的 Unity API。需要两边共用抽象时，在现有 `Engine` 接口扩展，并在改动处标明 `UnrealSharp portability` 和需要保留的行为。
