using System.Linq.Expressions;
using System.Runtime.CompilerServices;

class Test
{
    #region Attributes
    private readonly Random random;
    private KDTree<int> _tree;
    private List<Node<int>> _list;
    private int _operationsNum;
    private int _treeDimension;

    #endregion

    #region Constructor 
    public Test(int operationsNum, int treeDimension, int seed) {
        _tree = new KDTree<int>();
        _list = new List<Node<int>>();
        _operationsNum = operationsNum;
        _treeDimension = treeDimension;
        random = new Random(seed);
    }

    public Test(int operationsNum, int treeDimension) {
        _tree = new KDTree<int>();
        _list = new List<Node<int>>();
        _operationsNum = operationsNum;
        _treeDimension = treeDimension;
        random = new Random();
    }
    #endregion

    #region Get/Set
    internal KDTree<int> Tree { get => _tree; set => _tree = value; }
    internal List<Node<int>> List { get => _list; set => _list = value; }
    #endregion

    #region Methods

    /// <summary>
    /// method that tests basic operations of Tree
    /// </summary>
    /// <returns>bool</returns>
    public bool TestOperations() {
        int operation;

        for(int i = 0; i < _operationsNum; ++i){
            operation = GenerateOperation();
            if(operation == 1) {
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }

                Node<int> node = new Node<int>(keys, i, null, null, null);

                _tree.AddNode(node);
                _list.Add(node);

            } else if (operation == 0 ){
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                _tree.FindElement(keys);
            } else {
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                List<Node<int>> deletion;
                deletion = _tree.FindNode(keys);
                _tree.RemoveElement(keys);

                if(deletion is not null) {
                    foreach(var del in deletion) {
                        _list.Remove(del);
                    }
                }

            }
        }

        
        List<Node<int>> treeList = _tree.InOrder();

        if((_list is not null && treeList is not null)){ 
            if(_list.Count == treeList.Count) {
                return true;
            }
        }else if ((_list is  null && treeList is  null) || (_list.Count == 0 && treeList is null)) {
            return true;
        } else {
            return false;
        }

        return false;
    }

    /// <summary>
    /// Generates operation
    /// </summary>
    /// <returns>int</returns>
    public int GenerateOperation() {
        double number = random.NextDouble();
        
        if(number > 0.5) {
            return 1;
        } else if(number <= 0.5 && number > 0.1) {
            return -1;
        } else {
            return 0;
        }
    }

    /// <summary>
    /// Generates keys for tree
    /// </summary>
    /// <returns>Key</returns>
    public Key GenerateKey() {
        return new Key(random.NextInt64(100));
    }

    #endregion
    
}