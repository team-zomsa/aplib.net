using Aplib.Core;
using Aplib.Core.Desire.Goals;
using System;
using System.Collections;

namespace Aplib.Integrations.Unity
{
    /// <summary>
    /// Represents a test runner for an agent.
    /// </summary>
    class TestRunner
    {
        /// <summary>
        /// The agent that the test runner is testing.
        /// </summary>
        private readonly IAgent _agent;

        public TestRunner(IAgent agent)
        {
            _agent = agent;
        }

        /// <summary>
        /// Runs the test for the agent. The test continues until the agent's status is no longer Unfinished.
        /// </summary>
        /// <returns>An IEnumerator that can be used to control the execution of the test.</returns>
        public IEnumerator Test()
        {
            while (_agent.Status == CompletionStatus.Unfinished)
            {
                // Perform computation or update the agent here
                _agent.Update();

                // Wait for the next frame
                yield return null;
            }
        }
    }
}
