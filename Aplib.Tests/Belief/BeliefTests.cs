﻿using Aplib.Core.Belief;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="Belief{TReference,TObservation}"/> class.
/// </summary>
public class BeliefTests
{
    /// <summary>
    /// Given a Belief instance,
    /// When it is assigned to a variable of its observation type,
    /// Then it is implicitly converted to its observation type.
    /// </summary>
    [Fact]
    public void Belief_AssignedToObservationType_IsCorrectlyImplicitlyConvertedToObservationType()
    {
        // Arrange
        // ReSharper disable once ConvertToConstant.Local
        string def = "def";
        // Observation: Get the first letter.
        Belief<string, char> belief = new(def, reference => reference[0]);

        // Act
        char observation = belief;

        // Assert
        Assert.Equal('d', observation);
        Assert.Equal('d', belief.Observation);
        Assert.Equal(belief.Observation, belief);
    }

    /// <summary>
    /// Given a Belief instance with an shouldUpdate condition that is satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ShouldUpdateConditionIsSatisfied_UpdatesObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, reference => reference.Count, () => true);

        // Act
        list.Add(69);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(list.Count, belief);
        Assert.Equal(list.Count, belief.Observation);
    }

    /// <summary>
    /// Given a Belief instance with an shouldUpdate condition that is not satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ShouldUpdateConditionIsNotSatisfied_DoesNotUpdateObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, reference => reference.Count, () => false);

        // Act
        list.Add(420);
        belief.UpdateBelief();

        // Assert
        Assert.NotEqual(list.Count, belief);
        Assert.NotEqual(list.Count, belief.Observation);
    }

    /// <summary>
    /// Given a Belief instance with a reference,
    /// When the reference is assigned to and UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ReferenceIsAssignedTo_DoesNotUpdateObservation()
    {
        // Arrange
        string def = "def";
        Belief<string, string> belief = new(def, reference => reference, () => true);

        // Act
        def = "abc";
        belief.UpdateBelief();

        // Assert
        Assert.NotEqual(def, belief.Observation);
    }

    /// <summary>
    /// Given a reference,
    /// When a new Belief is constructed,
    /// Then the observation is also initialized.
    /// </summary>
    [Fact]
    public void Belief_DuringConstruction_UpdatesTheObservation()
    {
        // Arrange
        // ReSharper disable once ConvertToConstant.Local
        string def = "def";

        // Act
        Belief<string, string> belief = new(def, str => str);

        // Assert
        Assert.Equal(def, belief.Observation);
    }

    /// <summary>
    /// Given a reference that is actually a value type, hidden behind an interface,
    /// When a new Belief is constructed from this reference,
    /// The constructor throws an ArgumentException.
    /// </summary>
    [Fact]
    public void Belief_ConstructedWithAValueTypeViaAnInterface_IsRejected()
    {
        // Arrange
        MyEnumerable value = new(1);
        const string paramName = "reference";
        // ReSharper disable once ConvertToLocalFunction
        Action construction = () =>
        {
            // The bug is the fact that we can get around the constraint that `TReference` should be a reference type.
            Belief<IEnumerable<int>, List<int>> _ = new(value, values => values.ToList());
        };

        // Act, Assert
        Assert.Throws<ArgumentException>(paramName, construction);
    }

    private struct MyEnumerable : IEnumerable<int>
    {
        private readonly int _number;

        private const int MAX = 3;

        public MyEnumerable(int number) => _number = number;

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < MAX; i++) yield return _number;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
