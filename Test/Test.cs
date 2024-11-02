using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

class Test
{
    #region Attributes
    private readonly Random random;
    private KDTree<int> _tree;
    private List<Node<int>> _list;
    private KDTree<string> _treeDel;
    private List<Node<string>> _listDel;
    private int _operationsNum;
    private int _treeDimension;

    #endregion

    #region Constructor 
    public Test(int operationsNum, int treeDimension, int seed) {
        _tree = new KDTree<int>();
        _list = new List<Node<int>>();
        _treeDel = new KDTree<string>();
        _listDel = new List<Node<string>>();
        _operationsNum = operationsNum;
        _treeDimension = treeDimension;
        random = new Random(seed);
    }

    public Test(int operationsNum, int treeDimension) {
        _tree = new KDTree<int>();
        _list = new List<Node<int>>();
        _treeDel = new KDTree<string>();
        _listDel = new List<Node<string>>();
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
        
        if((_list is not null && treeList is not null)){ 
            if(_list.Count == treeList.Count) {
                foreach(var listItem in _list) {
                    if(_tree.FindNode(listItem.Keys)[0] is null) {
                        return false;
                    }
                }
                return true;
            }
        }else if ((_list is  null && treeList is  null) || (_list.Count == 0 && treeList is null)) {
            return true;
        } else {
            return false;
        }

        return false;
    }

    public bool TestOperationsDuplicated() {
        int operation;

        Stopwatch stopwatch = new Stopwatch();

        List<List<Key>> keyList = new List<List<Key>>();
        List<int> dataList = new List<int>();

        for(int i = 0; i < _operationsNum; ++i){
            operation = GenerateOperation();
            if(operation == 1) {
                List<Key> keys = new List<Key>();
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }

                Node<int> node = new Node<int>(keys, i, null, null, null);
                keyList.Add(keys);
                dataList.Add(i);
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
            } else if (operation == 2) 
            {
                if(_list.Count != 0) {
                    int rand = random.Next(_list.Count);
                    Node<int> node = new Node<int>(_list[rand].Keys, i, null, null, null);
                    keyList.Add(_list[rand].Keys);
                    dataList.Add(i);
                    stopwatch.Start();
                    _tree.AddNode(node);
                    stopwatch.Stop();
                    //Console.WriteLine("insert - " + stopwatch.Elapsed + " " + i + " Node " + node.Data + " key 0 " + node.Keys[0].KeyAttr + " key 1 " + node.Keys[1].KeyAttr);
                    _list.Add(node);
                }
            }
            else {
                
                List<Key> keys = new List<Key>();
                int data;
                /*
                for(int j = 0; j < _treeDimension; ++j) {
                   keys.Add(GenerateKey());
                }
                */
                if(keyList.Count != 0) {
                    int rand = random.Next(keyList.Count);
                    keys = keyList[rand];
                    data = dataList[rand];
                    List<Node<int>> deletion;
                    deletion = _tree.FindExactNode(keys, data);
                    stopwatch.Start();
                    _tree.RemoveExactElement(keys, data);
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
        
        if((_list is not null && treeList is not null)){ 
            if(_list.Count == treeList.Count) {
                foreach(var listItem in _list) {
                    if(_tree.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                        return false;
                    }
                }
                return true;
            }
        }else if ((_list is  null && treeList is  null) || (_list.Count == 0 && treeList is null)) {
            return true;
        } else {
            return false;
        }

        return false;
    }

    public async Task<bool> TestOperationsKont2() {
        int operation;

        Stopwatch stopwatch = new Stopwatch();

        List<List<Key>> keyList = new List<List<Key>>();
        List<int> dataList = new List<int>();

        for(int i = 0; i < _operationsNum; ++i){
            operation = GenerateOperation();
            if(operation == 1) {
                List<Key> keys = GenerateKeysForKont();

                Node<int> node = new Node<int>(keys, i, null, null, null);
                keyList.Add(keys);
                dataList.Add(i);
                stopwatch.Start();
                _tree.AddNode(node);
                stopwatch.Stop();
                Console.WriteLine("insert : " + stopwatch.Elapsed + " - Data - " + i);
                foreach(var keyVar in node.Keys) {
                    Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                }

                Console.WriteLine();
                _list.Add(node);

            } else if (operation == 0 ){
                List<Key> keys = new List<Key>();
                int data;
                if(keyList.Count > 0) {
                    int rand = random.Next(keyList.Count);
                    keys = keyList[rand];
                    data = dataList[rand];
                    stopwatch.Start();
                    _tree.FindExactNode(keys, data);
                    stopwatch.Stop();
                    Console.WriteLine("find : " + stopwatch.Elapsed + " - Data - " + data);
                    foreach(var keyVar in keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }

                    Console.WriteLine();
                }
            } else if(operation == 2) {
                if(_list.Count != 0) {
                    int rand = random.Next(_list.Count);
                    Node<int> node = new Node<int>(_list[rand].Keys, i, null, null, null);
                    keyList.Add(_list[rand].Keys);
                    dataList.Add(i);
                    stopwatch.Start();
                    _tree.AddNode(node);
                    stopwatch.Stop();
                    Console.WriteLine("insert duplicate  : " + stopwatch.Elapsed + " - Data - " + i);
                    foreach(var keyVar in node.Keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }
                    Console.WriteLine();

                    _list.Add(node);
                }
            } else {
                
                List<Key> keys = new List<Key>();
                int data;

                if(keyList.Count != 0) {
                    int rand = random.Next(keyList.Count);
                    keys = keyList[rand];
                    data = dataList[rand];
                    List<Node<int>> deletion;
                    deletion = _tree.FindExactNode(keys, data);
                    stopwatch.Start();
                    _tree.RemoveExactElement(keys, data);
                    stopwatch.Stop();
                    Console.WriteLine("remove : " + stopwatch.Elapsed + " - Data - " + data);
                    foreach(var keyVar in keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }
                    Console.WriteLine();

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

        /*
        foreach(var inorderIt in _tree.InOrderIter()) {
            Console.WriteLine(inorderIt.ToString());
        }
        */
        
        if(_list is not null && treeList is not null){ 
            if(_list.Count == treeList.Count) {
                foreach(var listItem in _list) {
                    if(_tree.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                        return false;
                    }
                }
                return true;
            }
        }else if ((_list is  null && treeList is  null) || (_list.Count == 0 && treeList is null)) {
            return true;
        } else {
            return false;
        }

        return false;
    }
    
    public async Task<bool> TestOperDelivery() {
        
        int operation;
        Stopwatch stopwatch = new Stopwatch();

        List<List<Key>> keyList = new List<List<Key>>();
        List<string> dataList = new List<string>();


        for (int t = 0; t < 20000; t++) {
            List<Key> keys = GenerateKeysForDeliv();
            int maxLeng = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder stringBuilder = new StringBuilder(maxLeng);

            for (int d = 0; d < maxLeng; d++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            string data = stringBuilder.ToString();

            Node<string> node = new Node<string>(keys, data, null, null, null);
            keyList.Add(keys);
            dataList.Add(data);
            stopwatch.Start();
            _treeDel.AddNode(node);
            stopwatch.Stop();
            Console.WriteLine("insert : " + stopwatch.Elapsed + " - Data - " + data);
            foreach(var keyVar in node.Keys) {
                Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
            }

            Console.WriteLine();
            _listDel.Add(node);
        }

        for(int i = 0; i < _operationsNum; ++i){
            operation = GenerateOperation();
            Console.WriteLine("operation - " + operation);
            if(operation == 1) {
                List<Key> keys = GenerateKeysForDeliv();

                int maxLeng = 10;
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                StringBuilder stringBuilder = new StringBuilder(maxLeng);

                for (int d = 0; d < maxLeng; d++)
                {
                    int index = random.Next(chars.Length);
                    stringBuilder.Append(chars[index]);
                }

                string data = stringBuilder.ToString();

                Node<string> node = new Node<string>(keys, data, null, null, null);
                keyList.Add(keys);
                dataList.Add(data);
                stopwatch.Start();
                _treeDel.AddNode(node);
                stopwatch.Stop();
                Console.WriteLine("operation " + i + " :  insert : " + stopwatch.Elapsed + " - Data - " + data);
                foreach(var keyVar in node.Keys) {
                    Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                }

                Console.WriteLine();
                _listDel.Add(node);

                List<Node<string>> treeListAdd = _treeDel.InOrder();
                if(_listDel is not null && treeListAdd is not null){ 
                    if(_listDel.Count == treeListAdd.Count) {
                        foreach(var listItem in _listDel) {
                            if(_treeDel.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                                throw new ArgumentException("Count of elements is not the same");
                            }
                        }
                    }
                } else {
                    throw new ArgumentException("Count of elements is not the same");
                }

            } else if (operation == 0 ){
                List<Key> keys = new List<Key>();
                string data;
                if(_listDel.Count > 0) {
                    int rand = random.Next(_listDel.Count);
                    keys = _listDel[rand].Keys;
                    data = _listDel[rand].Data;
                    stopwatch.Start();
                    if(_treeDel.FindExactNode(keys, data)[0] is null) {
                        throw new ArgumentException("Element not found in tree");
                    }
                    stopwatch.Stop();
                    Console.WriteLine("operation " + i + " find : " + stopwatch.Elapsed + " - Data - " + data);
                    foreach(var keyVar in keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }

                    Console.WriteLine();
                    List<Node<string>> treeListFind = _treeDel.InOrder();
                    if(_listDel is not null && treeListFind is not null){ 
                        if(_listDel.Count == treeListFind.Count) {
                            foreach(var listItem in _listDel) {
                                if(_treeDel.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                                    throw new ArgumentException("Count of elements is not the same");
                                }
                            }
                        }
                    } else {
                        throw new ArgumentException("Count of elements is not the same");
                    }
                }
            } else if(operation == 2) {
                if(_listDel.Count != 0) {
                    int rand = random.Next(_listDel.Count);

                    int maxLeng = 10;
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    StringBuilder stringBuilder = new StringBuilder(maxLeng);

                    for (int d = 0; d < maxLeng; d++)
                    {
                        int index = random.Next(chars.Length);
                        stringBuilder.Append(chars[index]);
                    }

                    string data = stringBuilder.ToString();

                    Node<string> node = new Node<string>(_listDel[rand].Keys, data, null, null, null);
                    keyList.Add(_listDel[rand].Keys);
                    dataList.Add(data);
                    stopwatch.Start();
                    _treeDel.AddNode(node);
                    stopwatch.Stop();
                    Console.WriteLine("operation " + i + " insert duplicate  : " + stopwatch.Elapsed + " - Data - " + i);
                    foreach(var keyVar in node.Keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }
                    Console.WriteLine();

                    _listDel.Add(node);

                    List<Node<string>> treeListDupl = _treeDel.InOrder();
                    if(_listDel is not null && treeListDupl is not null){ 
                        if(_listDel.Count == treeListDupl.Count) {
                            foreach(var listItem in _listDel) {
                                if(_treeDel.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                                    throw new ArgumentException("Count of elements is not the same");
                                }
                            }
                        }
                    } else {
                        throw new ArgumentException("Count of elements is not the same");
                    }
                }
            } else {
                
                List<Key> keys = new List<Key>();
                string data;

                if(keyList.Count != 0) {
                    int rand = random.Next(keyList.Count);
                    keys = keyList[rand];
                    data = dataList[rand];
                    List<Node<string>> deletion;
                    deletion = _treeDel.FindExactNode(keys, data);
                    stopwatch.Start();
                    _treeDel.RemoveExactElement(keys, data);
                    stopwatch.Stop();
                    Console.WriteLine("operation " + i + " remove : " + stopwatch.Elapsed + " - Data - " + data);
                    foreach(var keyVar in keys) {
                        Console.Write(" " + keyVar.KeyAttr.ToString() + " ");
                    }
                    Console.WriteLine();

                    if(deletion is not null) {
                        foreach(var del in deletion) {
                            _listDel.Remove(del);
                        }
                    }
                }

                List<Node<string>> treeListRem = _treeDel.InOrder();
                if(_listDel is not null && treeListRem is not null){ 
                    if(_listDel.Count == treeListRem.Count) {
                        foreach(var listItem in _listDel) {
                            if(_treeDel.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                                throw new ArgumentException("Count of elements is not the same");
                            }
                        }
                    }
                } else {
                    throw new ArgumentException("Count of elements is not the same");
                }

            }
        }

        Console.WriteLine("inorder start");
        stopwatch.Start();
        List<Node<string>> treeListDel = _treeDel.InOrder();
        stopwatch.Stop();
        Console.WriteLine("inorder done - " + stopwatch.Elapsed);

        if(_listDel is not null && treeListDel is not null){ 
            if(_listDel.Count == treeListDel.Count) {
                foreach(var listItem in _listDel) {
                    if(_treeDel.FindExactNode(listItem.Keys, listItem.Data)[0] is null) {
                        return false;
                    }
                }
                return true;
            }
        }else if ((_listDel is  null && treeListDel is  null) || (_listDel.Count == 0 && treeListDel is null)) {
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
        Console.WriteLine("number - " + number);
        
        if(number < 0.5) {
            return 1;
        }
        else if(number >= 0.5 && number < 0.7) 
        {
            return 2;
        }
        else if(number >= 0.7 && number < 0.9) {
            return -1;
        } else {
            return 0;
        }
    }

    public List<Key> GenerateKeysForKont() {

        double a = Math.Round(random.NextDouble() * 50,2);
        double d = Math.Round(random.NextDouble() * 50,2);
        int c = random.Next(50);

        int maxLeng = 10;
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        StringBuilder stringBuilder = new StringBuilder(maxLeng);

        for (int i = 0; i < maxLeng; i++)
        {
            int index = random.Next(chars.Length);
            stringBuilder.Append(chars[index]);
        }

        string b = stringBuilder.ToString();

        Level1 level1 = new Level1(a, b);
        Level2 level2 = new Level2(c);
        Level3 level3 = new Level3(d);
        Level4 level4 = new Level4(b, c);

        List<Key> keys = [new Key(level1), new Key(level2), new Key(level3), new Key(level4)];
        
        return keys;
    }

    public List<Key> GenerateKeysForDeliv() {

        double x = random.Next(50);
        double y = random.Next(50);

        List<Key> keys = [new Key(x), new Key(y)];
        
        return keys;
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


public class Level1 : IComparable<Level1>, IComparable
{
    private double _a;
    private string _b;

    public Level1(double a, string b)
    {
        _a = a;
        _b = b;
    }

    public double A { get => _a; set => _a = value; }
    public string B { get => _b; set => _b = value; }


    public int CompareTo(Level1? other)
    {
        if (other == null)
        {
            return 1;
        }

        int result = _a.CompareTo(other._a);
        if (result == 0)
        {
            result = string.Compare(_b, other._b, StringComparison.Ordinal);
        }

        return result;
    }

     public int CompareTo(object? obj)
    {
        if (obj is Level1 other)
        {
            return CompareTo(other);
        }
        throw new ArgumentException("Object is not a Level1");
    }

    public override string ToString()
    {
        return $"Level1(B: {_a}, C: {_b})";
    }
}

public class Level2 : IComparable<Level2>, IComparable
{
    private int _c;

    public Level2(int c)
    {
        _c = c;
    }

    public int C { get => _c; set => _c = value; }

    public int CompareTo(Level2? other)
    {
        if (other == null)
        {
            return 1;
        }

        return  _c.CompareTo(other._c);
    }

     public int CompareTo(object? obj)
    {
        if (obj is Level2 other)
        {
            return CompareTo(other);
        }
        throw new ArgumentException("Object is not a Level2");
    }

    public override string ToString()
    {
        return $"Level2(C: {_c})";
    }
}

public class Level3 : IComparable<Level3>, IComparable
{
    private double _d;

    public Level3(double d)
    {
        _d = d;
    }

    public double D { get => _d; set => _d = value; }

    public int CompareTo(Level3? other)
    {
        if (other == null)
        {
            return 1;
        }

        return  _d.CompareTo(other._d);
    }

     public int CompareTo(object? obj)
    {
        if (obj is Level3 other)
        {
            return CompareTo(other);
        }
        throw new ArgumentException("Object is not a Level3");
    }

    public override string ToString()
    {
        return $"Level3(D: {_d})";
    }
}

public class Level4 : IComparable<Level4>, IComparable
{
    private string _b;
    private int _c;

    public Level4(string b, int c)
    {
        _b = b;
        _c = c;
    }

    public string B { get => _b; set => _b = value; }
    public int C { get => _c; set => _c = value; }

    public int CompareTo(Level4? other)
    {
        if (other == null)
        {
            return 1;
        }

        int result = string.Compare(_b, other._b, StringComparison.Ordinal);
        if (result == 0)
        {
            result = _c.CompareTo(other._c);
        }

        return result;
    }

    public int CompareTo(object? obj)
    {
        if (obj is Level4 other)
        {
            return CompareTo(other);
        }
        throw new ArgumentException("Object is not a Level4");
    }

    public override string ToString()
    {
        return $"Level4(B: {_b}, C: {_c})";
    }
}