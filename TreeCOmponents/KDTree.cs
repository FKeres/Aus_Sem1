using System.Collections;
using Microsoft.VisualBasic;

class KDTree<T> : IEnumerable<T>
{
    #region Attributes
    private Node<T>? _root;
    #endregion

    #region Constructor

    public KDTree()
    {
    }

    public KDTree(Node<T> root)
    {
        _root = root;
    }
    #endregion

    #region Get/Set
    internal Node<T> Root { get => _root; set => _root = value; }
    #endregion

    #region Methods

    /// <summary>
    /// inserts data into the tree with given keys
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="data"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddElement(List<Key> keys, T data) {

        Node<T> nodeToBeAdded = new Node<T>(keys, data, null, null, null);

        if (!CheckKeyDimensions(nodeToBeAdded)) {
            throw new ArgumentException("Node does not contain correct number of dimensions.");
        }

        if (_root is null) {
            _root = nodeToBeAdded;
            _root.Dimension = 0;
            return;
        }

        var actualNode = _root;
        int actualCompNodeLevel = 0;
        bool spotFound = false;
        int compResult;

        while(!spotFound){
            int currentDimension = actualCompNodeLevel % actualNode.Keys.Count;
            int nextDimension = (actualCompNodeLevel + 1) % actualNode.Keys.Count;

            compResult = KDTree<T>.CompareNodeKeys(currentDimension, actualNode, nodeToBeAdded);

            if(compResult <= 0) {
                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    actualNode.LeftN = nodeToBeAdded;
                    actualNode.LeftN.Parent = actualNode;
                    actualNode.LeftN.ImLeft = true;
                    actualNode.LeftN.Dimension = nextDimension;
                    spotFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    actualNode.RightN = nodeToBeAdded;
                    actualNode.RightN.Parent = actualNode;
                    actualNode.RightN.ImLeft = false;
                    actualNode.RightN.Dimension = nextDimension;
                    spotFound = true;
                }
            }

            ++actualCompNodeLevel;
        }
    }

    /// <summary>
    /// inserts data into the tree with given keys
    /// </summary>
    /// <param name="nodeToBeAdded"></param>
    /// <exception cref="ArgumentException"></exception>
    public void AddNode(Node<T> nodeToBeAdded) {

        if (!CheckKeyDimensions(nodeToBeAdded)) {
            throw new ArgumentException("Node does not contain correct number of dimensions.");
        }

        if (_root is null) {
            _root = nodeToBeAdded;
            _root.Dimension = 0;
            return;
        }

        var actualNode = _root;
        int actualCompNodeLevel = 0;
        bool spotFound = false;
        int compResult;

        while(!spotFound){
            int currentDimension = actualCompNodeLevel % actualNode.Keys.Count;
            int nextDimension = (actualCompNodeLevel + 1) % actualNode.Keys.Count;

            compResult = KDTree<T>.CompareNodeKeys(currentDimension, actualNode, nodeToBeAdded);

            if(compResult <= 0) {
                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    actualNode.LeftN = nodeToBeAdded;
                    actualNode.LeftN.Parent = actualNode;
                    actualNode.LeftN.ImLeft = true;
                    actualNode.LeftN.Dimension = nextDimension;
                    spotFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    actualNode.RightN = nodeToBeAdded;
                    actualNode.RightN.Parent = actualNode;
                    actualNode.RightN.ImLeft = false;
                    actualNode.RightN.Dimension = nextDimension;
                    spotFound = true;
                }
            }

            ++actualCompNodeLevel;
        }
    }

    /// <summary>
    /// finds elements by given keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>List<T></returns>
    /// <exception cref="ArgumentException"></exception>
    public List<T> FindElement(List<Key> keys) {
        if (!CheckKeyDimensionsFromK(keys)) {
            throw new ArgumentException("Key List does not contain correct number of dimensions.");
        }

        if (_root is null) {
            return null;
        }

        bool itemsFound = false;
        var actualNode = _root;
        int actualCompNodeLevel = 0;
        int compResult;
        List<T> items = new List<T>();

        while(!itemsFound) {
            compResult = KDTree<T>.CompareKeys(actualCompNodeLevel % actualNode.Keys.Count, actualNode, keys);

            if(compResult <= 0) {
                if(compResult == 0 && KeysMatch(actualNode, keys)) {
                    items.Add(actualNode.Data);
                }

                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    itemsFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    itemsFound = true;
                }
            }

            ++actualCompNodeLevel;
        }

        return items;

    }

    /// <summary>
    /// finds elements by given keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>List<T></returns>
    /// <exception cref="ArgumentException"></exception>
    public List<Node<T>> FindNode(List<Key> keys) {
        if (!CheckKeyDimensionsFromK(keys)) {
            throw new ArgumentException("Key List does not contain correct number of dimensions.");
        }

        if (_root is null) {
            return null;
        }

        bool itemsFound = false;
        var actualNode = _root;
        int actualCompNodeLevel = 0;
        int compResult;
        List<Node<T>> items = new List<Node<T>>();

        while(!itemsFound) {
            compResult = KDTree<T>.CompareKeys(actualCompNodeLevel % actualNode.Keys.Count, actualNode, keys);

            if(compResult <= 0) {
                if(compResult == 0 && KeysMatch(actualNode, keys)) {
                    items.Add(actualNode);
                }

                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    itemsFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    itemsFound = true;
                }
            }

            ++actualCompNodeLevel;
        }

        return items;

    }

    /// <summary>
    /// finds elements by given keys
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>List<T></returns>
    /// <exception cref="ArgumentException"></exception>
    public List<Node<T>> FindExactNode(List<Key> keys, T data) {
        if (!CheckKeyDimensionsFromK(keys)) {
            throw new ArgumentException("Key List does not contain correct number of dimensions.");
        }

        if (_root is null) {
            return null;
        }

        bool itemsFound = false;
        var actualNode = _root;
        int actualCompNodeLevel = 0;
        int compResult;
        List<Node<T>> items = new List<Node<T>>();

        while(!itemsFound) {
            compResult = KDTree<T>.CompareKeys(actualCompNodeLevel % actualNode.Keys.Count, actualNode, keys);

            if(compResult <= 0) {
                if(compResult == 0 && KeysMatch(actualNode, keys) && actualNode.Data.Equals(data)) {
                    items.Add(actualNode);
                }

                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    itemsFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    itemsFound = true;
                }
            }

            ++actualCompNodeLevel;
        }

        return items;

    }

    /// <summary>
    /// returns sorted list by keys of tree elements  in order
    /// </summary>
    /// <returns>List<typeparamref name="T"/></returns>
    public List<Node<T>> InOrder() {
        if (_root is null) {
            return null;
        }

        var actualNode = _root;
        bool allProcessed = false;
        List<Node<T>> items = new List<Node<T>>();

        while(!allProcessed) {

            mostLeft:
            while(actualNode.HasLeftSon()) {
                actualNode = actualNode.LeftN;
            }

            items.Add(actualNode);

            checkRight:
            if(actualNode.HasRightSon()) {
                actualNode = actualNode.RightN;
                goto mostLeft;
            }

            upstairs:
            if(actualNode.ImLeft) {
                actualNode = actualNode.Parent;
                items.Add(actualNode);
                goto checkRight;
            } else {
                actualNode = actualNode.Parent;
                if(!(actualNode == _root)) {
                    if(actualNode is not null) {
                        goto upstairs;
                    }
                }
                allProcessed = true;
            }

        }

        return items;
    }

    /// <summary>
    /// returns sorted list by keys of tree elements  in order
    /// </summary>
    /// <returns>List<typeparamref name="T"/></returns>
    public IEnumerable<T> InOrderIter() {
        if (_root is null) {
            yield break;
        }

        var actualNode = _root;
        bool allProcessed = false;

        while(!allProcessed) {

            mostLeft:
            while(actualNode.HasLeftSon()) {
                actualNode = actualNode.LeftN;
            }

            yield return actualNode.Data;

            checkRight:
            if(actualNode.HasRightSon()) {
                actualNode = actualNode.RightN;
                goto mostLeft;
            }

            upstairs:
            if(actualNode.ImLeft) {
                actualNode = actualNode.Parent;
                yield return actualNode.Data;
                goto checkRight;
            } else {
                actualNode = actualNode.Parent;
                if(!(actualNode == _root)) {
                    if(actualNode is not null) {
                        goto upstairs;
                    }
                }
                allProcessed = true;
            }

        }

    }

    /// <summary>
    /// returns sorted list by keys of tree elements  in level order
    /// </summary>
    /// <returns>List<typeparamref name="T"/></returns>
    public List<Node<T>> LevelOrder() {
         if (_root is null) {
            return null;
        }

        Node<T> actualNode;
        List<Node<T>> items = [_root];
        int indexITP = 0;

        while(indexITP < items.Count) {
            actualNode = items[indexITP];

            if(actualNode.HasLeftSon()) {
                items.Add(actualNode.LeftN);
            }

            if(actualNode.HasRightSon()) {
                items.Add(actualNode.RightN);
            }

            ++indexITP;
        }

        return items;
    }

    /// <summary>
    /// returns iteratble by keys of tree elements  in level order
    /// </summary>
    /// <returns>List<typeparamref name="T"/></returns>
    public IEnumerable<T> LevelOrderIter() {
         if (_root is null) {
            yield break;
        }

        Node<T> actualNode;
        List<Node<T>> items = [_root];
        int indexITP = 0;

        while(indexITP < items.Count) {
            actualNode = items[indexITP];
            yield return actualNode.Data;

            if(actualNode.HasLeftSon()) {
                items.Add(actualNode.LeftN);
            }

            if(actualNode.HasRightSon()) {
                items.Add(actualNode.RightN);
            }

            ++indexITP;
        }

    }

    /// <summary>
    /// finds minimum of given dimension
    /// </summary>
    /// <param name="dimension"></param>
    /// <param name="startNode"></param>
    /// <returns>Node<typeparamref name="T"/></returns>
    public List<Node<T>> FIndMinForDimension(int dimension, Node<T> startNode) {

        Node<T> minNode = startNode;
        Node<T> actualNode = startNode;
        List<Node<T>> minNodes = new List<Node<T>>();

        bool allProcessed = false;

        while(!allProcessed) {

            mostLeft:
            while(actualNode.HasLeftSon()) {
                actualNode = actualNode.LeftN;
            }

            if(actualNode.Keys[dimension].CompareTo(minNode.Keys[dimension]) < 0) {
                minNodes.Clear();
                minNodes.Add(actualNode);
                minNode = actualNode;
            } else if(actualNode.Keys[dimension].CompareTo(minNode.Keys[dimension]) == 0) {
                minNodes.Add(actualNode);
            }
            
            checkRight:
            if(actualNode.HasRightSon() && !(actualNode.Dimension == dimension)) {
                actualNode = actualNode.RightN;
                goto mostLeft;
            }

            upstairs:
            if(actualNode.ImLeft) {
                if(!(actualNode == startNode)) {
                    actualNode = actualNode.Parent;
                    if(actualNode.Keys[dimension].CompareTo(minNode.Keys[dimension]) < 0) {
                        minNodes.Clear();
                        minNodes.Add(actualNode);
                        minNode = actualNode;
                    } else if(actualNode.Keys[dimension].CompareTo(minNode.Keys[dimension]) == 0) {
                        minNodes.Add(actualNode);
                    }
                    goto checkRight;
                }
                allProcessed = true;
            } else {
                if(!(actualNode == startNode)) {
                    if(actualNode is not null) {
                        actualNode = actualNode.Parent;
                        goto upstairs;
                    }
                }
                allProcessed = true;
            }
        }

        return minNodes;
    }

    /// <summary>
    /// finds maximum of given dimension
    /// </summary>
    /// <param name="dimension"></param>
    /// <param name="startNode"></param>
    /// <returns>Node<typeparamref name="T"/></returns>
    public Node<T> FIndMaxForDimension(int dimension, Node<T> startNode) {

        Node<T> maxNode = startNode;
        Node<T> actualNode = startNode;
        bool allProcessed = false;

        while(!allProcessed) {

            mostRight:
            while(actualNode.HasRightSon()) {
                actualNode = actualNode.RightN;
            }

            if(actualNode.Keys[dimension].CompareTo(maxNode.Keys[dimension]) > 0) {
                maxNode = actualNode;
            }
            
            checkLeft:
            if(actualNode.HasLeftSon() && !(actualNode.Dimension == dimension)) {
                actualNode = actualNode.LeftN;
                goto mostRight;
            }

            upstairs:
            if(!actualNode.ImLeft) {
                if(!(actualNode == startNode)) {
                    actualNode = actualNode.Parent;
                    if(actualNode.Keys[dimension].CompareTo(maxNode.Keys[dimension]) > 0) {
                        maxNode = actualNode;
                    }
                    goto checkLeft;
                }
                allProcessed = true;
            } else {
                if(!(actualNode == startNode)) {
                    if(actualNode is not null) {
                        actualNode = actualNode.Parent;
                        goto upstairs;
                    }
                }
                allProcessed = true;
            }
        }

        return maxNode;
    }

    /// <summary>
    /// removes element from tree with given keys
    /// </summary>
    /// <param name="keys"></param>
    public void RemoveElement(List<Key> keys) {
        List<Node<T>> nodesToBeRemoved = new List<Node<T>>();
        nodesToBeRemoved = FindNode(keys);
        if(nodesToBeRemoved is not null) {
            foreach(var node in nodesToBeRemoved) {
                RemoveNode(node);
            }
        }
    }

    /// <summary>
    /// removes element from tree with given keys and data
    /// </summary>
    /// <param name="keys, data"></param>
    public void RemoveExactElement(List<Key> keys, T data) {
        List<Node<T>> nodesToBeRemoved = new List<Node<T>>();
        nodesToBeRemoved = FindExactNode(keys, data);
        if(nodesToBeRemoved is not null) {
            foreach(var node in nodesToBeRemoved) {
                RemoveNode(node);
            }
        }
    }

    /// <summary>
    /// removes given node from tree
    /// </summary>
    /// <param name="nodeToBeRemoved"></param>
    public void RemoveNode(Node<T> nodeToBeRemoved) {
        bool nodeRemoved = false;
        bool allNodesRemoved = false;
        Node<T> replacingNode;
        List<Node<T>> replacingNodes = new List<Node<T>>();
        List<Node<T>> nodesToRemove = new List<Node<T>>();
        int deleteNodeIndex = 0;

        while(!allNodesRemoved) {
            while(!nodeRemoved) {
                if(nodeToBeRemoved.ImLeaf()) {
                    if(nodeToBeRemoved.ImLeft) {
                        if(nodeToBeRemoved.HasParent()) {
                            nodeToBeRemoved.Parent.LeftN = null;
                        }
                        
                    } else {
                        if(nodeToBeRemoved.HasParent()) {
                            nodeToBeRemoved.Parent.RightN = null;
                        }
                    }

                    nodeToBeRemoved.Parent = null;
                    if(nodeToBeRemoved == _root) {
                        _root = null;
                    }
                    nodeRemoved = true;
                    continue;
                }

                if(nodeToBeRemoved.HasLeftSon()) {
                    replacingNode = FIndMaxForDimension( nodeToBeRemoved.Dimension, nodeToBeRemoved.LeftN);
                    ReplaceNodes(nodeToBeRemoved, replacingNode);
                } else {
                    replacingNodes = FIndMinForDimension(nodeToBeRemoved.Dimension, nodeToBeRemoved.RightN);
                    if(replacingNodes.Count > 1) {
                        for (int i = 1; i < replacingNodes.Count; ++i) {
                            if(!nodesToRemove.Contains(replacingNodes[i])) {
                                nodesToRemove.Add(replacingNodes[i]);
                            }
                        }
                    }
                    ReplaceNodes(nodeToBeRemoved, replacingNodes[0]);
                }
            }
            
            
            if(deleteNodeIndex < nodesToRemove.Count) {
                nodeRemoved = false;
                nodeToBeRemoved = nodesToRemove[deleteNodeIndex];
                ++deleteNodeIndex;
            } else {
                allNodesRemoved = true;
            }

        }

        foreach(var node in nodesToRemove) {
            AddNode(node);
        }
    }

    /// <summary>
    /// replace nodes in tree
    /// </summary>
    /// <param name="node1"></param>
    /// <param name="node2"></param>
    public void ReplaceNodes(Node<T> node1, Node<T> node2) {
        if(node1 == _root) {
            _root = node2;
        }
        
        Node<T> tempLeft = node1.LeftN;
        node1.LeftN = node2.LeftN;
        if(tempLeft == node2) {
            node2.LeftN = node1;
        } else {
            node2.LeftN = tempLeft;
        }

        Node<T> tempRight = node1.RightN;
        node1.RightN = node2.RightN;
        if(tempRight == node2) {
            node2.RightN = node1;
        } else {
            node2.RightN = tempRight;
        }

        Node<T> tempParent = node1.Parent;
        if(node1 == node2.Parent) {
            node1.Parent = node2;
        } else {
            node1.Parent = node2.Parent;
        }
        node2.Parent = tempParent;

        bool tempImLeft = node1.ImLeft;
        node1.ImLeft = node2.ImLeft;
        node2.ImLeft = tempImLeft;

        var tempDimension = node1.Dimension;
        node1.Dimension = node2.Dimension;
        node2.Dimension = tempDimension;

        if (node1.HasParent()) {
            if (node1.ImLeft) {
                node1.Parent.LeftN = node1;
            } else {
                node1.Parent.RightN = node1;
            }
        }

        if (node2.HasParent()) {
            if (node2.ImLeft) {
                node2.Parent.LeftN = node2;
            } else {
                node2.Parent.RightN = node2;
            }
        }

        if (node1.HasLeftSon()) {
            node1.LeftN.Parent = node1;
        }
        if (node1.HasRightSon()) {
            node1.RightN.Parent = node1;
        }
        if (node2.HasLeftSon()) {
            node2.LeftN.Parent = node2;
        }
        if (node2.HasRightSon()) {
            node2.RightN.Parent = node2;
        }

    }

    /// <summary>
    /// checks if the whole key is the same
    /// </summary>
    /// <param name="actualNode"></param>
    /// <param name="keys"></param>
    /// <returns>bool</returns>
    public bool KeysMatch(Node<T> actualNode, List<Key> keys) {

        for(int i = 0; i < keys.Count; ++i) {
            if(keys[i].CompareTo(actualNode.Keys[i]) != 0) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// compares keys of node that is to be insertes and node with which we are comparing to
    /// returns -1 if node's key to be added at given level is less than actual node's, 0 if they are the same and 1 if it is larger
    /// </summary>
    /// <param name="level"></param>
    /// <param name="actualNode"></param>
    /// <param name="nodeToBeAdded"></param>
    /// <returns>int</returns>
    public static int CompareNodeKeys(int level, Node<T> actualNode, Node<T> nodeToBeAdded) {
        return nodeToBeAdded.Keys[level].CompareTo(actualNode.Keys[level]);
    }

    /// <summary>
    /// compares keys of node that is to be insertes and node with which we are comparing to
    /// returns -1 if node's key to be added at given level is less than actual node's, 0 if they are the same and 1 if it is larger
    /// </summary>
    /// <param name="level"></param>
    /// <param name="actualNode"></param>
    /// <param name="keys"></param>
    /// <returns>int</returns>
    public static int CompareKeys(int level, Node<T> actualNode, List<Key> keys) {
        return keys[level].CompareTo(actualNode.Keys[level]);
    }

    /// <summary>
    /// checks if the key dimension of node that is to be added is the same as the dimension in root
    /// </summary>
    /// <param name="nodeToBeAdded"></param>
    /// <returns>bool</returns>
    public bool CheckKeyDimensions(Node<T> nodeToBeAdded) {
        if(_root is null) {
            return true;
        }

        return nodeToBeAdded.Keys.Count == _root.Keys.Count;
    }

    /// <summary>
    /// checks if the key dimension of given keys is the same as the dimension in root
    /// </summary>
    /// <param name="keys"></param>
    /// <returns>bool</returns>
    public bool CheckKeyDimensionsFromK(List<Key> keys) {
        if(_root is null) {
            return true;
        }

        return keys.Count == _root.Keys.Count;
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
    #endregion

}