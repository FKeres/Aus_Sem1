public class ParcelService
{
    private KDTree<Parcel> _parcelTree;

    public ParcelService()
    {
        _parcelTree = new KDTree<Parcel>();
    }

    internal KDTree<Parcel> GetAllParcels()
    {
        return _parcelTree;
    }

    internal void AddParcel(Parcel parcel)
    {
        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _parcelTree.AddElement(keys, parcel); 

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _parcelTree.AddElement(keys2, parcel);
    }

    internal List<Parcel> SearchParcels(double gpsWToSearch, double gpsLToSearch) {
        Key key1 = new Key(gpsWToSearch);
        Key key2 = new Key(gpsLToSearch);

        List<Key> keys = [key1, key2];

        return _parcelTree.FindElement(keys);
    }

    internal void RemoveParcel(Parcel parcel) {
        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _parcelTree.RemoveExactElement(keys, parcel);

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _parcelTree.RemoveExactElement(keys2, parcel);
    }

    internal void EditParcel(Parcel parcel) {

    }
    
}
