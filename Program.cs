internal class Program
{
    private static void Main(string[] args)
    {
        KDTree<int> tree = new KDTree<int>();

        List<Key> keys7 = [new Key(6), new Key(76)];
        tree.AddElement(keys7, 1);

        List<Key> keys8 = [new Key(52), new Key(45)];
        tree.AddElement(keys8, 2);

        List<Key> keys9 = [new Key(51), new Key(48)];
        tree.AddElement(keys9, 3);

        List<Key> keys10 = [new Key(44), new Key(75)];
        tree.AddElement(keys10, 4);

         List<Key> keys11 = [new Key(71), new Key(84)];
        tree.AddElement(keys11, 5);

        List<Key> keys12 = [new Key(63), new Key(94)];
        tree.AddElement(keys12, 6);

        List<Key> keys13 = [new Key(8), new Key(79)];
        tree.AddElement(keys13, 7);

        List<Key> keys14 = [new Key(42), new Key(85)];
        tree.AddElement(keys14, 8);
        
        
        
        List<Key> keys1 = [new Key(42), new Key(86)];
        tree.AddElement(keys1, 9);

        List<Key> keys2 = [new Key(43), new Key(96)];
        tree.AddElement(keys2, 10);
        
        List<Key> keys3 = [new Key(43), new Key(96)];
        tree.AddElement(keys3, 11);

        List<Node<int>> levelList = tree.LevelOrder();

        foreach(var node in levelList) {
            Console.WriteLine(node.Data);
        }

        all();
        //test(755);
       
    }

    public static void all() {
        int i = 1;
        while(true) {
            Test test = new Test(100000, 2, i);
            bool answer = test.TestOperations();
            Console.WriteLine(i);
            if(!answer) {
                throw new ArgumentException(i.ToString());
            }
            i++;
        }
    }

    public static void test(int num) {
         
        Test test = new Test(100000, 2, num);
        bool answer = test.TestOperations();
        Console.WriteLine(answer); 
        
    }


}