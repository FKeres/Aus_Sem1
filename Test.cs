using System.Runtime.CompilerServices;

class Test<T>
{
    #region Attributes
    private static readonly Random random = new Random();
    private KDTree<T> _tree;
    private List<T> _list;

    #endregion


    #region Constructor 
    public Test(int elementsNum, KDTree<T> tree, List<T> list) {
        _tree = tree;
        _list = list;
    }
    #endregion

    #region Methods

    public bool TestOperations(int operation) {
        return true;
    }
    public int GenerateOperation() {

        if (random.NextDouble() >= 0.5) {
            return 1;
        }
        return 0;
    }

    
    #endregion
    
}