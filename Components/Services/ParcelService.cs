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
    
    internal void EditParcel(Parcel oldParc, Parcel newParc) {
        Key key1 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _parcelTree.RemoveExactElement(keys, oldParc);

        Key key3 = new Key(oldParc.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(oldParc.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _parcelTree.RemoveExactElement(keys2, oldParc);

        oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition;
        oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition;
        oldParc.GpsPosHandler.GetGpsPosition(1).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition;
        oldParc.GpsPosHandler.GetGpsPosition(1).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(1).LengthPosition;

        Key key5 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key6 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys3 = [key5, key6];

        _parcelTree.AddElement(keys3, oldParc);

        Key key7 = new Key(oldParc.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key8 = new Key(oldParc.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys4 = [key7, key8];

        _parcelTree.AddElement(keys4, oldParc);
    }
}
