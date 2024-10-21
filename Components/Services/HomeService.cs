public class HomeService
{
    private KDTree<PropParcHolder> _tree;

    public HomeService()
    {
        _tree = new KDTree<PropParcHolder>();
    }

    internal KDTree<PropParcHolder> GetAllParcels()
    {
        return _tree;
    }

    internal void AddParcel(Parcel parcel)
    {
        PropParcHolder holder = new PropParcHolder();
        holder.Parcel = parcel;

        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _tree.AddElement(keys, holder); 

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _tree.AddElement(keys2, holder);
    }

    internal void AddProperty(Property property)
    {
        PropParcHolder holder = new PropParcHolder();
        holder.Property = property;

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _tree.AddElement(keys, holder); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _tree.AddElement(keys2, holder);
    }

    internal List<PropParcHolder> SearchAll(double gpsWToSearch, double gpsLToSearch) {
        Key key1 = new Key(gpsWToSearch);
        Key key2 = new Key(gpsLToSearch);

        List<Key> keys = [key1, key2];

        return _tree.FindElement(keys);
    }
}
