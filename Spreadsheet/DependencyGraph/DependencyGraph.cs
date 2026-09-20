// Skeleton implementation written by Joe Zachary for CS 3500, September 2013
// Version 1.1 - Joe Zachary
// (Fixed error in comment for RemoveDependency)
// Version 1.2 - Daniel Kopta Fall 2018
// (Clarified meaning of dependent and dependee)
// (Clarified names in solution/project structure)
// Version 1.3 - H. James de St. Germain Fall 2024
namespace DependencyGraph;
/// <summary>
/// <para>
/// (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
/// (in other words: s1 must be evaluated before t1.)
/// </para>
/// <para>
/// A DependencyGraph can be modeled as a set of ordered pairs of strings.
/// Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
/// if s1 equals s2 and t1 equals t2.
/// </para>
/// <remarks>
/// Recall that sets never contain duplicates.
/// If an attempt is made to add an element to a set, and the element is already
/// in the set, the set remains unchanged.
/// </remarks>
/// <para>
/// Given a DependencyGraph DG:
/// </para>
/// <list type="number">
/// <item>
/// If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
/// (The set of things that depend on s.)
/// </item>
/// <item>
/// If s is a string, the set of all strings t such that (t,s) is in DG is  called dependees(s).
/// (The set of things that s depends on.)
/// </item>
/// </list>
/// <para>
/// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d","d")}.

/// </para>
/// <code>
/// dependents("a") = {"b", "c"}
/// dependents("b") = {"d"}
/// dependents("c") = {}
/// dependents("d") = {"d"}
/// dependees("a") = {}
/// dependees("b") = {"a"}
/// dependees("c") = {"a"}
/// dependees("d") = {"b", "d"}
/// </code>
/// </summary>
public class DependencyGraph
{
    private Dictionary<string, Node> nodes;
    private int size;
    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
    /// The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        nodes = new Dictionary<string, Node>();
        size = 0;
    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return size; }
    }

    /// <summary>
    /// Reports whether the given node has dependents (i.e., other nodes depend on it).

    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        if (nodes.ContainsKey(nodeName))
        {
            Node node = nodes[nodeName];
            return node.Dependents.Count > 0;
        }

        return false;
    }

    /// <summary>
    /// Reports whether the given node has dependees (i.e., depends on one or more other nodes).

    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        if (nodes.ContainsKey(nodeName))
        {
            Node node = nodes[nodeName];
            if (node.Dependees.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// <para>
    /// Returns the dependents of the node with the given name.
    /// </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        if (nodes.ContainsKey(nodeName))
        {
            Node node = nodes[nodeName];
            List<string> result = new List<string>();

            foreach (Node dependentNode in node.Dependents)
            {
                result.Add(dependentNode.Name);
            }
            return result;
        }

        return new List<string>();
    }

    /// <summary>
    /// <para>
    /// Returns the dependees of the node with the given name.
    /// </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        if (nodes.ContainsKey(nodeName))
        {
            Node node = nodes[nodeName];
            List<string> result = new List<string>();

            foreach (Node dependeeNode in node.Dependees)
            {
                result.Add(dependeeNode.Name);
            }
            return result;
        }
        return new List<string>();
    }

    /// <summary>
    /// <para>Adds the ordered pair (dependee, dependent), if it doesn'texist.</para>

    ///
    /// <para>
    /// This can be thought of as: dependee must be evaluated before dependent
    /// </para>
    /// </summary>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>

    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>

    public void AddDependency(string dependee, string dependent)
    {
        Node sNode = GetOrCreateNode(dependee);
        Node tNode = GetOrCreateNode(dependent);
        if (sNode.Dependents.Add(tNode))
        {
            tNode.Dependees.Add(sNode);
            size++;
        }
    }

    /// <summary>
    /// <para>
    /// Removes the ordered pair (dependee, dependent), if it exists.
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>

    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>

    public void RemoveDependency(string dependee, string dependent)
    {
        if (nodes.ContainsKey(dependee))
        {
            if (nodes.ContainsKey(dependent))
            {
                Node sNode = nodes[dependee];
                Node tNode = nodes[dependent];
                if (sNode.Dependents.Remove(tNode))
                {
                    tNode.Dependees.Remove(sNode);
                    size--;
                }
            }
        }
    }

    /// <summary>
    /// Removes all existing ordered pairs of the form (nodeName, *). Then, for each
        /// t in newDependents, adds the ordered pair (nodeName, t).
        /// </summary>
        /// <param name="nodeName"> The name of the node whose dependents are being replaced</param>

    /// <param name="newDependents"> The new dependents for nodeName</param>
    public void ReplaceDependents(string nodeName, IEnumerable<string>
        newDependents)
    {
        List<string> currentDependents = new List<string>(GetDependents(nodeName));

        for (int i = 0; i < currentDependents.Count; i++)
        {
            string dependent = currentDependents[i];
            RemoveDependency(nodeName, dependent);
        }
        List<string> newDependentsList = new List<string>(newDependents);

        for (int i = 0; i < newDependentsList.Count; i++)
        {
            string newDependent = newDependentsList[i];
            AddDependency(nodeName, newDependent);
        }
    }
    /// <summary>
    /// <para>
    /// Removes all existing ordered pairs of the form (*, nodeName). Then, for each
        /// t in newDependees, adds the ordered pair (t, nodeName).
        /// </para>
        /// </summary>
        /// <param name="nodeName"> The name of the node who's dependees are bein replaced</param>

    /// <param name="newDependees"> The new dependees for nodeName</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        List<string> currentDependees = new List<string>(GetDependees(nodeName));

        for (int i = 0; i < currentDependees.Count; i++)
        {
            string dependee = currentDependees[i];
            RemoveDependency(dependee, nodeName);
        }
        List<string> newDependeesList = new List<string>(newDependees);
        for (int i = 0; i < newDependeesList.Count; i++)
        {
            string newDependee = newDependeesList[i];
            AddDependency(newDependee, nodeName);
        }
    }

    private class Node
    {
        public string Name { get; }
        public HashSet<Node> Dependents { get; }
        public HashSet<Node> Dependees { get; }

        public Node(string name)
        {
            Name = name;
            Dependents = new HashSet<Node>();
            Dependees = new HashSet<Node>();
        }
    }

    private Node GetOrCreateNode(string name)
    {
        if (nodes.ContainsKey(name))
        {
            return nodes[name];
        }
        Node newNode = new Node(name);
        nodes[name] = newNode;
        return newNode;
    }
    
}