using Aplib.Core.Belief.Beliefs;
using System.Collections.Generic;
using static Aplib.Core.Belief.Beliefs.UpdateMode;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="SampledMemoryBelief{TReference,TObservation}"/> class.
/// </summary>
public class SampledMemoryBeliefTests
{
    /// <summary>
    /// Given a SampledMemoryBelief instance with update mode 'AlwaysUpdate',
    /// When the belief is not in a sampleInterval-th cycle,
    /// Then GetMostRecentMemory should be outdated.
    /// </summary>
    [Fact]
    public void GetMostRecentMemory_WhenUpdateModeIsAlwaysUpdate_ShouldBeOutdated()
    {
        // Arrange
        List<int> list = [];
        int sampleInterval = 2,
            framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, AlwaysUpdate, framesToRemember);

        // Act
        // Expected values:
        // ----------------------------------------------------
        //             | list.Count | Observation |  Memory
        // ----------------------------------------------------
        // Initial     | 0          | 0 *         | [0, 0, 0]
        // Iteration 0 | 1          | 1           | [0, 0, 0]   (memory * is sampled)
        // Iteration 1 | 2          | 2 *         | [0, 0, 0]
        // Iteration 2 | 3          | 3           | [2, 0, 0]   (memory * is sampled)
        // Iteration 3 | 4          | 4 *         | [2, 0, 0]
        // Iteration 4 | 5          | 5           | [4, 2, 0]   (memory * is sampled)
        // Iteration 5 | 6          | 6           | [4, 2, 0]
        // ----------------------------------------------------
        for (int i = 0; i < 6; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        Assert.Equal(4, belief.GetMostRecentMemory());
    }

    /// <summary>
    /// Given a SampledMemoryBelief instance with update mode 'UpdateWhenSampled',
    /// When the belief is not in a sampleInterval-th cycle,
    /// Then GetMostRecentMemory should be outdated.
    /// </summary>
    [Fact]
    public void GetMostRecentMemory_WhenUpdateModeIsUpdateWhenSampled_ShouldBeOutdated()
    {
        // Arrange
        List<int> list = [];
        int sampleInterval = 2,
            framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, UpdateWhenSampled, framesToRemember);

        // Act
        // Expected values:
        // ----------------------------------------------------
        //             | list.Count | Observation |  Memory
        // ----------------------------------------------------
        // Initial     | 0          | 0 *         | [0, 0, 0]
        // Iteration 0 | 1          | 1           | [0, 0, 0]   (memory * is sampled & observation is updated)
        // Iteration 1 | 2          | 1 *         | [0, 0, 0]
        // Iteration 2 | 3          | 3           | [1, 0, 0]   (memory * is sampled & observation is updated)
        // Iteration 3 | 4          | 3 *         | [1, 0, 0]
        // Iteration 4 | 5          | 5           | [3, 1, 0]   (memory * is sampled & observation is updated)
        // Iteration 5 | 6          | 5           | [3, 1, 0]
        // ----------------------------------------------------
        for (int i = 0; i < 6; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        Assert.Equal(3, belief.GetMostRecentMemory());
    }

    /// <summary>
    /// Given a SampledMemoryBelief instance with update mode 'AlwaysUpdate',
    /// When the belief is not in a sampleInterval-th cycle,
    /// Then Observation should be up-to-date.
    /// </summary>
    [Fact]
    public void Observation_WhenUpdateModeIsAlwaysUpdate_ShouldBeUpToDate()
    {
        // Arrange
        List<int> list = [];
        int sampleInterval = 2,
            framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, AlwaysUpdate, framesToRemember);

        // Act
        // Expected values:
        // -----------------------------------------
        //             | list.Count | Observation
        // -----------------------------------------
        // Initial     | 0          | 0              (observation is updated)
        // Iteration 0 | 1          | 1              (observation is updated)
        // Iteration 1 | 2          | 2              (observation is updated)
        // -----------------------------------------
        for (int i = 0; i < 2; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        Assert.Equal(list.Count, belief.Observation);
    }

    /// <summary>
    /// Given a SampledMemoryBelief instance with update mode 'UpdateWhenSampled',
    /// When the belief is not in a sampleInterval-th cycle,
    /// Then Observation should be outdated.
    /// </summary>
    [Fact]
    public void Observation_WhenUpdateModeIsUpdateWhenSampled_ShouldBeOutdated()
    {
        // Arrange
        List<int> list = [];
        int sampleInterval = 2,
            framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, UpdateWhenSampled, framesToRemember);

        // Act
        // Expected values:
        // -----------------------------------------
        //             | list.Count | Observation
        // -----------------------------------------
        // Initial     | 0          | 0
        // Iteration 0 | 1          | 1              (observation is updated)
        // Iteration 1 | 2          | 1
        // Iteration 2 | 3          | 3              (observation is updated)
        // Iteration 3 | 4          | 3
        // -----------------------------------------
        for (int i = 0; i < 4; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        Assert.NotEqual(list.Count, belief.Observation);
    }

    /// <summary>
    /// Given a SampledMemoryBelief instance with update mode 'UpdateWhenSampled',
    /// When the belief is not in a sampleInterval-th cycle,
    /// Then Observation should have a value from the previous sample interval.
    /// </summary>
    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Observation_WhenUpdateModeIsUpdateWhenSampled_ShouldBeFromPreviousSampleInterval(int sampleInterval)
    {
        // Arrange
        List<int> list = [];
        int framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, UpdateWhenSampled, framesToRemember);

        // Act
        // Expected values:
        // ------------------------------------------------------------------
        //                          | list.Count         | Observation
        // ------------------------------------------------------------------
        // Initial                  | 0                  | 0
        // Iteration 0              | 1                  | 1                  (observation is updated)
        // Iteration 1              | 2                  | 1
        // ...
        // Iteration sampleInterval | sampleInterval + 1 | sampleInterval + 1 (observation is updated)
        // ...
        // ------------------------------------------------------------------
        // Iterate 2 intervals.
        for (int i = 0; i < sampleInterval * 2; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        // We expect the previous observation update to occur at the sampleInterval-th iteration/cycle,
        // when the list count is sampleInterval + 1 (see table).
        int expected = sampleInterval + 1;
        Assert.Equal(expected, belief.Observation);
    }

    /// <summary>
    /// Given a SampledMemoryBelief instance with a shouldUpdate method,
    /// When shouldUpdate returns false,
    /// Then Observation should not be updated regardless of the update mode.
    /// </summary>
    [Fact]
    public void Observation_WhenShouldUpdateReturnsFalse_ShouldNotBeUpdated()
    {
        // Arrange
        List<int> list = [];
        int sampleInterval = 2,
            framesToRemember = 3;
        SampledMemoryBelief<List<int>, int> belief
            = new(list, reference => reference.Count, sampleInterval, AlwaysUpdate, framesToRemember, () => false);

        // Act
        // Expected values:
        // -----------------------------------------
        //             | list.Count | Observation
        // -----------------------------------------
        // Initial     | 0          | 0              (set initial observation)
        // Iteration 0 | 1          | 0
        // Iteration 1 | 2          | 0
        // -----------------------------------------
        for (int i = 0; i < 2; i++)
        {
            list.Add(0);
            belief.UpdateBelief();
        }

        // Assert
        Assert.Equal(0, belief.Observation);
    }
}
