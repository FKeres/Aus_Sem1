using System.Linq.Expressions;
using System.Runtime.CompilerServices;

class Test
{
    #region Attributes
    private static readonly Random random = new Random();
    private KDTree<int> _tree;
    private List<int> _list;
    private int _operationsNum;
    private int _treeDimension;

    #endregion

    #region Constructor 
    public Test(int operationsNum, int treeDimension) {
        _tree = new KDTree<int>();
        _list = new List<int>();
        _operationsNum = operationsNum;
        _treeDimension = treeDimension;
    }
    #endregion

    #region Get/Set
    internal KDTree<int> Tree { get => _tree; set => _tree = value; }
    #endregion

    #region Methods

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
            } else if (operation == 0 ){
                continue;
            } else {
                continue;
            }
        } 

        return true;
    }
    public int GenerateOperation() {
        if (random.NextDouble() >= 0.5) {
            return 1;
        }
        return 0;
    }

    public Key GenerateKey() {
        return new Key(random.NextInt64(100));
    }

    
    #endregion
    
}