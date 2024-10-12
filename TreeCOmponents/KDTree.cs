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

            compResult = KDTree<T>.CompareNodeKeys(actualCompNodeLevel % actualNode.Keys.Count, actualNode, nodeToBeAdded);

            if(compResult <= 0) {
                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    actualNode.LeftN = nodeToBeAdded;
                    actualNode.LeftN.Parent = actualNode;
                    actualNode.LeftN.ImLeft = true;
                    spotFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    actualNode.RightN = nodeToBeAdded;
                    actualNode.RightN.Parent = actualNode;
                    actualNode.RightN.ImLeft = false;
                    spotFound = true;
                }
            }

            ++actualCompNodeLevel;
        }
    }

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

    public bool KeysMatch(Node<T> actualNode, List<Key> keys) {

        for(int i = 0; i < keys.Count; ++i) {
            if(keys[i].CompareTo(actualNode.Keys[i]) != 0) {
                return false;
            }
        }
        return true;
    }

    public static int CompareNodeKeys(int level, Node<T> actualNode, Node<T> nodeToBeAdded) {
        return nodeToBeAdded.Keys[level].CompareTo(actualNode.Keys[level]);
    }

    public static int CompareKeys(int level, Node<T> actualNode, List<Key> keys) {
        return keys[level].CompareTo(actualNode.Keys[level]);
    }

    public bool CheckKeyDimensions(Node<T> nodeToBeAdded) {
        if(_root is null) {
            return true;
        }

        return nodeToBeAdded.Keys.Count == _root.Keys.Count;
    }

    public bool CheckKeyDimensionsFromK(List<Key> keys) {
        if(_root is null) {
            return true;
        }

        return keys.Count == _root.Keys.Count;
    }
    #endregion


}