using System.Diagnostics;
using Verse;

namespace CombinationTraps
{
    class DebugLog
    {
        private static int count = 0;
        [Conditional("DEBUG")]
        public static void LogDebugPoint(string st = "", int stackcount = 1)
        {
            if (st == "")
            {
                st = DebugLog.GetMethodName(stackcount + 1);
            }
            count++;
            Log.Message(st + "@" + count);
        }
        public static string GetMethodName(int stackcount = 1)
        {
            StackFrame stack = new StackFrame(stackcount);
            return stack.GetMethod().ReflectedType.FullName + "." + stack.GetMethod().Name + "(): ";
        }
        public static string Sign()
        {
            return "[" + typeof(DebugLog).Namespace + "]:";
        }
    }
}
