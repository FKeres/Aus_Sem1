internal class Program
{
    private static void Main(string[] args)
    {

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

        KDTree<Parcel> tree = new KDTree<Parcel>();
        
        tree.AddElement(keys, parcel1);
        tree.AddElement(keys2, parcel1);
        
        tree.AddElement(keys3, parcel2);
        tree.AddElement(keys4, parcel2);
    }
}