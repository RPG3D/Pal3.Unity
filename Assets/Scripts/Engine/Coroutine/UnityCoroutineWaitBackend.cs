namespace Engine.Coroutine
{
    using System;
    using UnityEngine;

    // UnrealSharp portability. Unity supplies its native yield instructions
    // here, behind the same facade that SharpDemo uses with its frame scheduler.
    internal static class CoroutineWaitBackend
    {
        internal static object WaitForSeconds(float seconds) => new WaitForSeconds(seconds);

        internal static object WaitUntil(Func<bool> predicate) => new WaitUntil(predicate);
    }
}
