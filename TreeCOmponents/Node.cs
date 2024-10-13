using System.Runtime.InteropServices;

class Node<T>
{
    #region Attributes

    private List<Key> _keys;
    private T _data;
    private Node<T> _leftN;
    private Node<T> _rightN;
    private Node<T> _parent;
    private bool _imLeft;
    private int _dimension;

    #endregion

    #region Constructor

    public Node(List<Key> keys, T data, Node<T> leftN, Node<T> rightN, Node<T> parent)
    {
        _keys = keys;
        _data = data;
        _leftN = leftN;
        _rightN = rightN;
        _parent = parent;
    }

    public Node(Node<T> other) {
        _keys = other.Keys;
        _data = other.Data;
        _leftN = other.LeftN;
        _rightN = other.RightN;
        _parent = other._parent;
        _imLeft = other._imLeft;
        _dimension = -1;
    }

    #endregion

    #region Get/Set

    internal List<Key> Keys { 
        get => _keys; 
        set
        {
            if (!(value is IComparable))
            {
                throw new ArgumentException("Key must implement IComparable.");
            }
            _keys = value;
        }
     }
    internal Key GetKey(int position) {
        if(!CheckCorrectPos(position)){
            throw new ArithmeticException("Key on this position does not exists " + position);
        }
        return _keys[position];
        
    }
    internal T Data { get => _data; set => _data = value; }
    internal Node<T> LeftN { get => _leftN; set => _leftN = value; }
    internal Node<T> RightN { get => _rightN; set => _rightN = value; }
    internal Node<T> Parent { get => _parent; set => _parent = value; }
    internal bool ImLeft {get => _imLeft; set => _imLeft = value; }
    public int Dimension { get => _dimension; set => _dimension = value; }

    #endregion

    #region Methods
    /// <summary>
    /// checks if the given position is not out of bounds
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckCorrectPos(int position) {
        return position > 0 && position < _keys.Capacity;
    }

    /// <summary>
    /// Adds Key to the node
    /// </summary>
    /// <param name="key"></param>
    public void AddExisitingKey(Key key) {
        _keys.Add(key);
    }

    /// <summary>
    /// adds element creates key and inserts into key list 
    /// </summary>
    /// <param name="key"></param>
    public void AddNewKey(IComparable key) {
        Key _key = new Key(key);
        _keys.Add(_key);
    }

    /// <summary>
    /// checks if actual node has left son
    /// </summary>
    /// <returns>bool</returns>
    public bool HasLeftSon() {
        return _leftN is not null;
    }

    /// <summary>
    /// checks if actual node has right son
    /// </summary>
    /// <returns>bool</returns>
    public bool HasRightSon() {
        return _rightN is not null;
    }

    /// <summary>
    /// checks is actual node has parent
    /// </summary>
    /// <returns>bool</returns>
    public bool HasParent() {
        return _parent is not null;
    }

    /// <summary>
    /// tells if current node is leaf or not
    /// </summary>
    /// <returns>bool</returns>
    public bool ImLeaf() {
        return _leftN is null && _rightN is null;
    }
    #endregion
}