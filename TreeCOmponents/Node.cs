using System.Runtime.InteropServices;

class Node<T>
{
    #region Attributes

    private List<Key> _keys;
    private T _data;
    private Node<T> _leftN;
    private Node<T> _rightN;

    #endregion

    #region Constructor

    public Node(List<Key> keys, T data, Node<T> leftN, Node<T> rightN)
    {
        _keys = keys;
        _data = data;
        _leftN = leftN;
        _rightN = rightN;
    }

    public Node(Node<T> other) {
        _keys = other.Keys;
        _data = other.Data;
        _leftN = other.LeftN;
        _rightN = other.RightN;
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

    #endregion

    #region Methods
    public bool CheckCorrectPos(int position) {
        return position > 0 && position < _keys.Capacity;
    }

    public void AddExisitingKey(Key key) {
        _keys.Add(key);
    }

    public void AddNewKey(Object key) {
        Key _key = new Key(key);
        _keys.Add(_key);
    }

    public bool HasLeftSon() {
        return this._leftN is not null;
    }

    public bool HasRightSon() {
        return this._rightN is not null;
    }
    #endregion
}