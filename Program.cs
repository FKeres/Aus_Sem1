internal class Program
{
    private static void Main(string[] args)
    {
        /*
        Property property1 = new Property(92, "Filipov Domov", 'S', 50.0, 'Z', 100.0, 'V', 60.0, 'J', 10.0);
        Parcel parcel1 = new Parcel(93, "Vedla Filipov Domov", 'S', 40.0, 'Z', 102.0, 'V', 50.6, 'J', 12.0);
        Parcel parcel2 = new Parcel(93, "Vedla Filipov Domov", 'S', 30.5, 'Z', 99.0, 'V', 70.5, 'J', 20.0);


        property1.AddParcel(parcel1);
        parcel1.AddProperty(property1);   

        Key key = new Key(parcel1.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel1.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key, key2];

        Key key3 = new Key(parcel1.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel1.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        Key key5 = new Key(parcel2.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key6 = new Key(parcel2.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys3 = [key5, key6];

        Key key7 = new Key(parcel2.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key8 = new Key(parcel2.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys4 = [key7, key8];
        */
        
        KDTree<string> tree = new KDTree<string>();
        /*
        List<Key> keys1 = [new Key(23), new Key(35)];
        tree.AddElement(keys1, "Nitra");

        List<Key> keys2 = [new Key(20), new Key(33)];
        tree.AddElement(keys2, "Sereď");
        
        List<Key> keys3 = [new Key(25), new Key(36)];
        tree.AddElement(keys3, "Topoľčany");

        List<Key> keys4 = [new Key(16), new Key(31)];
        tree.AddElement(keys4, "Galanta");

        List<Key> keys5 = [new Key(14), new Key(39)];
        tree.AddElement(keys5, "Senica");

        List<Key> keys6 = [new Key(28), new Key(34)];
        tree.AddElement(keys6, "Tlmače");

        List<Key> keys7 = [new Key(24), new Key(40)];
        tree.AddElement(keys7, "Bošany");

        List<Key> keys8 = [new Key(13), new Key(32)];
        tree.AddElement(keys8, "Bratislava");

        List<Key> keys9 = [new Key(12), new Key(41)];
        tree.AddElement(keys9, "Hodonín");

        List<Key> keys10 = [new Key(17), new Key(42)];
        tree.AddElement(keys10, "Trnava");

        List<Key> keys11 = [new Key(26), new Key(35)];
        tree.AddElement(keys11, "Moravce");

        List<Key> keys12 = [new Key(30), new Key(33)];
        tree.AddElement(keys12, "Levice");

        List<Key> keys13 = [new Key(29), new Key(46)];
        tree.AddElement(keys13, "Bojnice");

        List<Key> keys14 = [new Key(27), new Key(43)];
        tree.AddElement(keys14, "Nováky");
        */
        
        List<Key> keys1 = [new Key(20), new Key(30)];
        tree.AddElement(keys1, "13");

        List<Key> keys2 = [new Key(19), new Key(50)];
        tree.AddElement(keys2, "7");
        
        List<Key> keys3 = [new Key(22), new Key(20)];
        tree.AddElement(keys3, "16");

        List<Key> keys4 = [new Key(23), new Key(19)];
        tree.AddElement(keys4, "15");

        List<Key> keys5 = [new Key(23), new Key(19)];
        tree.AddElement(keys5, "14");

        List<Key> keys6 = [new Key(22), new Key(30)];
        tree.AddElement(keys6, "17");

        List<Key> keys7 = [new Key(17), new Key(30)];
        tree.AddElement(keys7, "5");

        List<Key> keys8 = [new Key(19), new Key(55)];
        tree.AddElement(keys8, "10");

        List<Key> keys9 = [new Key(15), new Key(20)];
        tree.AddElement(keys9, "1");

        List<Key> keys10 = [new Key(15), new Key(25)];
        tree.AddElement(keys10, "3");

        List<Key> keys11 = [new Key(14), new Key(25)];
        tree.AddElement(keys11, "2");

        List<Key> keys12 = [new Key(16), new Key(30)];
        tree.AddElement(keys12, "4");

        List<Key> keys13 = [new Key(18), new Key(40)];
        tree.AddElement(keys13, "6");

        List<Key> keys14 = [new Key(19), new Key(55)];
        tree.AddElement(keys14, "8");

        List<Key> keys15 = [new Key(19), new Key(57)];
        tree.AddElement(keys15, "9");

        List<Key> keys16 = [new Key(20), new Key(60)];
        tree.AddElement(keys16, "12");

        List<Key> keys17 = [new Key(20), new Key(60)];
        tree.AddElement(keys17, "11");

        List<string> list = tree.InOrder();

        foreach(var element in list) {
            Console.WriteLine(element);
        }

        Node<string> startNode = tree.FindNode(keys2)[0];

        Node<string> minX = tree.FIndMinForDimension(0, startNode.RightN);
        Node<string> minY = tree.FIndMinForDimension(1, startNode.RightN);


        /*
        Test test = new Test(10000, 1);
        if(test.TestOperations()) {
            Console.WriteLine("Test done ok");
        } else {
            Console.WriteLine("Test not done ok");
        }
        */
    }


}