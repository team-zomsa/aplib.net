using Aplib.Core.Collections;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Aplib.Core.Tests.Collections;

public class OptimizedActivationStackTests
{
    /// <summary>
    /// Given an optimized activation stack with activatables,
    /// When ActivatableStackItems is enumerated,
    /// Then for each activatable there should be an ActivatableStackItem.
    /// </summary>
    [Fact]
    public void ActivatableStackItems_WhenActivatablesAreGiven_ShouldContainEncapsulatedActivatables()
    {
        // Arrange
        int[] activatables = [1, 1, 2, 3, 5, 8];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Act
        IEnumerable<OptimizedActivationStack<int>.StackItem> activatableStackItems
            = activationStack.ActivatableStackItems;

        // Assert
        IEnumerable<int> items = activatableStackItems.Select(stackItem => stackItem.Data);
        items.Should().BeEquivalentTo(activatables);
    }

    /// <summary>
    /// Given an optimized activation stack with items on the stack,
    /// When an item is activated that was not activated yet,
    /// Then the item should be on top of the stack.
    /// </summary>
    [Fact]
    public void Activate_WhenItemIsActivated_ShouldBeOnTop()
    {
        // Arrange
        int[] activatables = [1, 1, 2, 3, 5];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        OptimizedActivationStack<int>.StackItem[] activationStackItems
            = activationStack.ActivatableStackItems.ToArray();

        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStackItems[0..^1])
            activationStack.Activate(stackItem);

        // Select the first stack item, because it is not on top of the stack.
        OptimizedActivationStack<int>.StackItem stackItemToActivate = activationStackItems[^1];

        // Act
        activationStack.Activate(stackItemToActivate);
        int topItem = activationStack.Peek();

        // Assert
        topItem.Should().Be(stackItemToActivate.Data);
    }

    /// <summary>
    /// Given an optimized activation stack with an item on the stack,
    /// When an item is activated that was already activated (i.e., reactivated),
    /// Then the item should be on top of the stack.
    /// </summary>
    [Fact]
    public void Activate_WhenItemIsReactivated_ShouldBeOnTop()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStack.ActivatableStackItems)
            activationStack.Activate(stackItem);

        // Select the first stack item, because it is not on top of the stack.
        OptimizedActivationStack<int>.StackItem stackItemToActivate
            = activationStack.ActivatableStackItems.First();

        // Act
        activationStack.Activate(stackItemToActivate);
        int topItem = activationStack.Peek();

        // Assert
        topItem.Should().Be(stackItemToActivate.Data);
    }

    /// <summary>
    /// Given an optimized activation stack,
    /// When an item from a different optimized activation stack is activated on this stack,
    /// Then an argument exception should be thrown.
    /// </summary>
    [Fact]
    public void Activate_WhenActivatedItemIsFromDifferentStack_ShouldThrowArgumentException()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        int[] otherActivatables = [4, 5, 6];
        OptimizedActivationStack<int> otherActivationStack = new(otherActivatables);
        OptimizedActivationStack<int>.StackItem stackItemToActivate
            = otherActivationStack.ActivatableStackItems.First();

        // Act
        System.Action activateItem = () => activationStack.Activate(stackItemToActivate);

        // Assert
        activateItem.Should().Throw<System.ArgumentException>();
    }

    /// <summary>
    /// Given an optimized activation stack with activated items on the stack,
    /// When an item is peeked multiple times,
    /// Then the peeked item should be the same.
    /// </summary>
    [Fact]
    public void Peek_WhenCalledMultipleTimes_ShouldReturnSameItem()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStack.ActivatableStackItems)
            activationStack.Activate(stackItem);

        // Act
        int firstPeekedItem = activationStack.Peek();
        int secondPeekedItem = activationStack.Peek();

        // Assert
        firstPeekedItem.Should().Be(secondPeekedItem);
    }

    /// <summary>
    /// Given an optimized activation stack with no activated items on the stack,
    /// When an item is peeked,
    /// Then an invalid operation exception should be thrown.
    /// </summary>
    [Fact]
    public void Peek_WhenStackIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Act
        System.Func<int> peekItem = activationStack.Peek;

        // Assert
        peekItem.Should().Throw<System.InvalidOperationException>();
    }

    /// <summary>
    /// Given an optimized activation stack with activated items on the stack,
    /// When an item is peeked,
    /// Then the count of the optimized activation stack should stay the same.
    /// </summary>
    [Fact]
    public void Peek_WhenCalled_CountShouldStayTheSame()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStack.ActivatableStackItems)
            activationStack.Activate(stackItem);

        // Act
        int countBeforePeek = activationStack.Count;
        _ = activationStack.Peek();
        int countAfterPeek = activationStack.Count;

        // Assert
        countBeforePeek.Should().Be(countAfterPeek);
    }

    /// <summary>
    /// Given an optimized activation stack with activated items on the stack,
    /// When multiple items are popped in succession,
    /// Then the popped items are different.
    /// </summary>
    [Fact]
    public void Pop_WhenCalledMultipleTimes_ShouldReturnDifferentItems()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStack.ActivatableStackItems)
            activationStack.Activate(stackItem);

        // Act
        int firstPoppedItem = activationStack.Pop();
        int secondPoppedItem = activationStack.Pop();

        // Assert
        firstPoppedItem.Should().NotBe(secondPoppedItem);
    }

    /// <summary>
    /// Given an optimized activation stack with no activated items on the stack,
    /// When an item is popped,
    /// Then an invalid operation exception should be thrown.
    /// </summary>
    [Fact]
    public void Pop_WhenStackIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Act
        System.Func<int> popItem = activationStack.Pop;

        // Assert
        popItem.Should().Throw<System.InvalidOperationException>();
    }

    /// <summary>
    /// Given an optimized activation stack with activated items on the stack,
    /// When an item is popped,
    /// Then the count of the optimized activation stack should be decreased by one.
    /// </summary>
    [Fact]
    public void Pop_WhenCalled_CountShouldDecrement()
    {
        // Arrange
        int[] activatables = [1, 2, 3];
        OptimizedActivationStack<int> activationStack = new(activatables);

        // Activate all stack items.
        foreach (OptimizedActivationStack<int>.StackItem stackItem in activationStack.ActivatableStackItems)
            activationStack.Activate(stackItem);

        // Act
        int countBeforePop = activationStack.Count;
        _ = activationStack.Pop();
        int countAfterPop = activationStack.Count;

        // Assert
        countAfterPop.Should().Be(countBeforePop - 1);
    }
}
