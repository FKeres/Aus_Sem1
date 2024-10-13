using System.Linq.Expressions;
using System.Runtime.CompilerServices;

class Test
{
    #region Attributes
    private static readonly Random random = new Random();
    private KDTree<int> _tree;
    //private List<Node<int>> _list;
    private List<int> _list;
    private int _operationsNum;
    private int _treeDimension;

    #endregion

    #region Constructor 
    public Test(int operationsNum, int treeDimension) {
        _tree = new KDTree<int>();
        //_list = new List<Node<int>>();
        _list = new List<int>();
        _operationsNum = operationsNum;
        _treeDimension = treeDimension;
    }
    #endregion

    #region Get/Set
    internal KDTree<int> Tree { get => _tree; set => _tree = value; }
    //internal List<Node<int>> List { get => _list; set => _list = value; }
    internal List<int> List { get => _list; set => _list = value; }
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
                _tree.AddElement(keys, i);
               // _list.Add(new Node<int>(keys, i, null, null, null));
               _list.Add(i);
            } else if (operation == 0 ){
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                _tree.FindElement(keys);
            } else {
                continue;
            }
        }

        
        List<int> treeList = _tree.InOrder();

        bool same = _list == treeList;

        if(_list.Count == treeList.Count && same) {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Generates operation
    /// </summary>
    /// <returns>int</returns>
    public int GenerateOperation() {
        if (random.NextDouble() >= 0.5) {
            return 1;
        }
        return 1;
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