using System;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class EngineExecutionAttribute : Attribute
    {
        public ExecutionMode Mode { get; private set; }

        public EngineExecutionAttribute(ExecutionMode mode)
        {
            Mode = mode;
        }
    }
}
