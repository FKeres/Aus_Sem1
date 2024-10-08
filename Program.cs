internal class Program
{
    private static void Main(string[] args)
    {

        Property property1 = new Property(92, "Filipov Domov", 'S', 50.0, 'Z', 90.0, 'V', 60.0, 'J', 10.0);
        Parcel parcel1 = new Parcel(93, "Vedla Filipov Domov", 'S', 40.0, 'Z', 99.0, 'V', 50.6, 'J', 15.0);
        Parcel parcel2 = new Parcel(93, "Vedla Filipov Domov", 'S', 51.5, 'Z', 99.0, 'V', 60.5, 'J', 15.0);


        property1.AddParcel(parcel1);
        parcel1.AddProperty(property1);   

        Key key = new Key(parcel1.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel1.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key, key2];

        Key key3 = new Key(parcel1.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel1.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        KDTree<Parcel> tree = new KDTree<Parcel>();
        
        tree.AddElement(keys, parcel1);  //50.0  90.0
        tree.AddElement(keys2, parcel1); //60.0  10.0
        
        if(tree.Root is not null) {
            Console.WriteLine(tree.Root.Data.ParcNo);
            if(tree.Root.HasLeftSon()) {
                Console.WriteLine("i am left " + tree.Root.LeftN.Data.ParcNo);
            }

            if(tree.Root.HasRightSon()) {
                Console.WriteLine("i am right " + tree.Root.RightN.Data.ParcNo);
            }
        }
    }
}