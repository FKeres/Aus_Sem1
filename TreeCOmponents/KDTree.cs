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

        Node<T> nodeToBeAdded = new Node<T>(keys, data, null, null);

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

            compResult = KDTree<T>.CompareKeys(actualCompNodeLevel % actualNode.Keys.Count, actualNode, nodeToBeAdded);

            if(compResult <= 0) {
                if (actualNode.HasLeftSon()) {
                    actualNode = actualNode.LeftN;
                } else {
                    actualNode.LeftN = nodeToBeAdded;
                    spotFound = true;
                }

            } else {
                if (actualNode.HasRightSon()) {
                    actualNode = actualNode.RightN;
                } else {
                    actualNode.RightN = nodeToBeAdded;
                    spotFound = true;
                }
            }

            ++actualCompNodeLevel;
        }
    }

    public static int CompareKeys(int level, Node<T> actualNode, Node<T> nodeToBeAdded) {
        return nodeToBeAdded.Keys[level].CompareTo(actualNode.Keys[level]);
    }

    public bool CheckKeyDimensions(Node<T> nodeToBeAdded) {
        if(_root is null) {
            return true;
        }

        return nodeToBeAdded.Keys.Count == _root.Keys.Count;
    }

    #endregion


}