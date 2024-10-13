using Microsoft.VisualBasic;

class KDTree<T>
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
            return;
        }

        var actualNode = _root;
        int actualCompNodeLevel = 0;
        bool spotFound = false;
        int compResult;

        while(!spotFound){
            int currentDimension = actualCompNodeLevel % actualNode.Keys.Count;

            compResult = KDTree<T>.CompareNodeKeys(currentDimension, actualNode, nodeToBeAdded);

            if(compResult <= 0) {
                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    actualNode.LeftN = nodeToBeAdded;
                    actualNode.LeftN.Parent = actualNode;
                    actualNode.LeftN.ImLeft = true;
                    actualNode.LeftN.Dimension = currentDimension;
                    spotFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    actualNode.RightN = nodeToBeAdded;
                    actualNode.RightN.Parent = actualNode;
                    actualNode.RightN.ImLeft = false;
                    actualNode.RightN.Dimension = currentDimension;
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
    /// returns sorted list by keys of tree elements 
    /// </summary>
    /// <returns>List<typeparamref name="T"/></returns>
    public List<T> InOrder() {
        if (_root is null) {
            return null;
        }

        var actualNode = _root;
        bool allProcessed = false;
        List<T> items = new List<T>();

        while(!allProcessed) {

            mostLeft:
            while(actualNode.HasLeftSon()) {
                actualNode = actualNode.LeftN;
            }

            items.Add(actualNode.Data);

            checkRight:
            if(actualNode.HasRightSon()) {
                actualNode = actualNode.RightN;
                goto mostLeft;
            }

            upstairs:
            if(actualNode.ImLeft) {
                actualNode = actualNode.Parent;
                items.Add(actualNode.Data);
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

    public Node<T> FIndMinForDimension(int dimension, Node<T> startNode) {

        Node<T> minNode = startNode;
        Node<T> actualNode = startNode;

    }

    /// <summary>
    /// removes element from tree with given keys
    /// </summary>
    /// <param name="keys"></param>
    public void RemoveElement(List<Key> keys) {

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
    #endregion

}