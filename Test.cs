using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

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

        Stopwatch stopwatch = new Stopwatch();

        List<List<Key>> keyList = new List<List<Key>>();

        for(int i = 0; i < _operationsNum; ++i){
            operation = GenerateOperation();
            if(operation == 1) {
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }

                Node<int> node = new Node<int>(keys, i, null, null, null);
                keyList.Add(keys);
                stopwatch.Start();
                _tree.AddNode(node);
                stopwatch.Stop();
                //Console.WriteLine("insert - " + stopwatch.Elapsed + " " + i + " Node " + node.Data + " key 0 " + node.Keys[0].KeyAttr + " key 1 " + node.Keys[1].KeyAttr);
                _list.Add(node);

            } else if (operation == 0 ){
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                stopwatch.Start();
                _tree.FindElement(keys);
                stopwatch.Stop();
                //Console.WriteLine("find - " + stopwatch.Elapsed  + " " + i + " key 0 " + keys[0].KeyAttr + "key 1 " + keys[1].KeyAttr);
            } else {
                
                List<Key> keys = new List<Key>();
                /*
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                */
                if(keyList.Count != 0) {
                    keys = keyList[random. Next(keyList.Count)];
                    List<Node<int>> deletion;
                    deletion = _tree.FindNode(keys);
                    stopwatch.Start();
                    _tree.RemoveElement(keys);
                    stopwatch.Stop();
                    //Console.WriteLine("remove - " + stopwatch.Elapsed  + " " + i + " key 0 " + keys[0].KeyAttr + "key 1 " + keys[1].KeyAttr);

                    if(deletion is not null) {
                        foreach(var del in deletion) {
                            _list.Remove(del);
                        }
                    }
                }

            }
        }

        Console.WriteLine("inorder start");
        stopwatch.Start();
        List<Node<int>> treeList = _tree.InOrder();
        stopwatch.Stop();
        Console.WriteLine("inorder done - " + stopwatch.Elapsed);

        List<Node<int>> levelList = _tree.LevelOrder();

        if((_list is not null && treeList is not null)){ 
            if(_list.Count == treeList.Count && _list.Count == levelList.Count) {
                return true;
            }
        }else if ((_list is  null && treeList is  null) || (_list.Count == 0 && treeList is null)) {
            return true;
        } else {
            return false;
        }

        return false;
    }

    public TimeSpan TestInsertionTime()
    {
        Stopwatch stopwatch = new Stopwatch();
        
        stopwatch.Start();

        for (int i = 0; i < _operationsNum; i++)
        {
            List<Key> keys = new List<Key>();
            for(int j = 0; j < _treeDimension; ++j) {
                keys.Add(GenerateKey());
            }
            _tree.AddElement(keys, i);
        }

        stopwatch.Stop();

        return stopwatch.Elapsed;
    }

    /// <summary>
    /// Generates operation
    /// </summary>
    /// <returns>int</returns>
    public int GenerateOperation() {
        double number = random.NextDouble();
        
        if(number < 0.7) {
            return 1;
        } else if(number >= 0.7 && number < 0.9) {
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
        return new Key(random.NextInt64(50));
    }

    #endregion
    
}