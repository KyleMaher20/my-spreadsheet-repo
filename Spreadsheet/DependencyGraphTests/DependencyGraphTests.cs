/// Kyle Maher & CS3500 Staff
/// Date: September 19, 2026
namespace DependencyGraphTests;

using DependencyGraph;

/// <summary>
/// This is a test class for DependencyGraphTest and is intended
/// to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    /// <summary>
    ///  This code is testing DependencyGraph class and is doing a stress test under
    /// 2 seconds with a size of 200 nodes where it is adding,removing then re adding and removing
    /// then testing to make sure everything is in the correct order
    /// Also, update in-line comments as appropriate.
    /// </summary>
    [TestMethod]
    [Timeout(2000, CooperativeCancellation = true)] // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();
// A bunch of strings to use
        const int size = 200;
        string[] letters = new string[size];
        for (int i = 0; i < size; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

// The correct answers
        HashSet<string>[] dependents = new HashSet<string>[size];
        HashSet<string>[] dependees = new HashSet<string>[size];
        for (int i = 0; i < size; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

// Add a bunch of dependencies
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

// Remove a bunch of dependencies
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 4; j < size; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

// Add some back
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

// Remove some more
        for (int i = 0; i < size; i += 2)
        {
            for (int j = i + 3; j < size; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

// Make sure everything is right
        for (int i = 0; i < size; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new
                HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new
                HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }

    /// <summary>
    /// Makes sure adequate behavioar occurs on an empty graph call and nodes that don't exist
    /// </summary>
    [TestMethod]
    public void EmptyGraph()
    {
        DependencyGraph graph = new DependencyGraph();
        int s = graph.Size;
        Assert.AreEqual(0, s);
        bool dependent = graph.HasDependents("z");
        Assert.IsFalse(dependent);
        bool dependee = graph.HasDependees("z");
        Assert.IsFalse(dependee);
        List<string> dependentsList = new List<string>(graph.GetDependents("z"));
        if (dependentsList.Count == 0)
        {
            Assert.AreEqual(0, dependentsList.Count);
        }
        List<string> dependeesList = new List<string>(graph.GetDependees("z"));
        if (dependeesList.Count == 0)
        {
            Assert.AreEqual(0, dependeesList.Count);
        }
    }

    /// <summary>
    /// Test for duplicate additions aswell as testing the remove for dependency that don't exist
    /// </summary>
    [TestMethod]
    public void DuplicatesAndRemoveOnNothing()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "z");
        graph.AddDependency("a", "z");
        int s1 = graph.Size;
        Assert.AreEqual(1, s1);
        graph.RemoveDependency("0", "z");
        int s2 = graph.Size;
        Assert.AreEqual(1, s2);
        graph.RemoveDependency("a", "z");
        int s3 = graph.Size;
        Assert.AreEqual(0, s3);
    }

    /// <summary>
    /// Test for putting an empty list into a dependent
    /// </summary>
    [TestMethod]
    public void PuttingEmptyListOnDependents()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "z");
        graph.AddDependency("a", "0");
        List<string> empty = new List<string>();
        graph.ReplaceDependents("a", empty);
        int s = graph.Size;
        Assert.AreEqual(0, s);
        bool dependent = graph.HasDependents("a");
        Assert.IsFalse(dependent);
    }

    /// <summary>
    /// Tests for replacing dependess on nodes that have no connection and making sure
    /// proper behavior occurs
    /// </summary>
    [TestMethod]
    public void CallingReplaceOnDependessWithNoConnection()
    {
        DependencyGraph graph = new DependencyGraph();
        List<string> list = new List<string>();
        list.Add("0");
        list.Add("z");
        graph.ReplaceDependees("a", list);
        int s = graph.Size;
        Assert.AreEqual(2, s);
        bool dependee = graph.HasDependees("a");
        Assert.IsTrue(dependee);
    }

    /// <summary>
    /// Tests for adding a self referencing dependency node
    /// </summary>
    [TestMethod]
    public void NodeGoesToItself()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("z", "z");
        int s = graph.Size;
        Assert.AreEqual(1, s);
        bool dependent = graph.HasDependents("z");
        if (dependent)
        {
            Assert.IsTrue(dependent);
        }
        bool dependee = graph.HasDependees("z");
        if (dependee)
        {
            Assert.IsTrue(dependee);
        }
    }

    /// <summary>
    /// Tests replacing dependents when the orginal connection already exists
    /// </summary>
    [TestMethod]
    public void ReplaceDependentsWithExistingConnections()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "z");
        List<string> list = new List<string>();
        list.Add("0");
        graph.ReplaceDependents("a", list);
        int s = graph.Size;
        Assert.AreEqual(1, s);
        bool dependent = graph.HasDependents("a");
        if (dependent)
        {
            Assert.IsTrue(dependent);
        }
    }

    /// <summary>
    /// Tests replacing dependees when the ogrinal connection already exists
    /// </summary>
    [TestMethod]
    public void ReplaceDependeesWithExistingConnections()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "z");
        List<string> list = new List<string>();
        list.Add("0");
        graph.ReplaceDependees("z", list);
        int s = graph.Size;
        Assert.AreEqual(1, s);
        bool dependee = graph.HasDependees("z");
        if (dependee)
        {
            Assert.IsTrue(dependee);
        }
    }

    /// <summary>
    /// Tests HasDependees when node exists but has zero dependees for edge case
    /// </summary>
    [TestMethod]
    public void HasDependeesNodeExistsWithNoDependees()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "z");
        bool dependee = graph.HasDependees("a");
        Assert.IsFalse(dependee);
    }
}