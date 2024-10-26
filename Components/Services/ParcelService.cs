public class ParcelService
{
    private KDTree<Parcel> _parcelTree;
    private int _actualParcId;

    public ParcelService()
    {
        _parcelTree = new KDTree<Parcel>();
        _actualParcId = 0;
    }

    public ParcelService(int actualParcId)
    {
        _parcelTree = new KDTree<Parcel>();
        _actualParcId = actualParcId;
    }

    internal KDTree<Parcel> GetAllParcels()
    {
        return _parcelTree;
    }

    internal void AddParcel(int parcNo, string parcDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Parcel parcel = new Parcel(_actualParcId, parcNo, parcDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);
        Key key1 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _parcelTree.AddElement(keys, parcel); 

        Key key3 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _parcelTree.AddElement(keys2, parcel);
        ++_actualParcId;
    }

    internal List<Parcel> SearchParcels(double gpsWToSearch, double gpsLToSearch) {
        Key key1 = new Key(gpsWToSearch);
        Key key2 = new Key(gpsLToSearch);

        List<Key> keys = [key1, key2];

        List<Parcel> copyParcels = new List<Parcel>();
        foreach(var copyParc in _parcelTree.FindElement(keys)) {
            Parcel copyParcel = new Parcel(copyParc.ParcelId, copyParc.ParcNo, copyParc.ParcDesc, copyParc.GpsPosHandler.GpsPositioons);
            copyParcels.Add(copyParcel);
        }

        return copyParcels;
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
    
    internal void EditParcel(Parcel oldParc, Parcel newParc) {
        Key key1 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        List<Node<Parcel>> node1 = _parcelTree.FindExactNode(keys, oldParc);

        if(node1 is not null) {
            if(newParc.ParcNo != 0) {
                node1[0].Data.ParcNo = newParc.ParcNo;
            }
            if(newParc.ParcDesc != "") {
                node1[0].Data.ParcDesc = newParc.ParcDesc;
            }
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width = newParc.GpsPosHandler.GetGpsPosition(0).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newParc.GpsPosHandler.GetGpsPosition(1).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length = newParc.GpsPosHandler.GetGpsPosition(0).Length;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newParc.GpsPosHandler.GetGpsPosition(1).Width;

            if(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition) {

                Key key9 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key10 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys5 = [key9, key10];

                _parcelTree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key4 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys2 = [key3, key4];

                _parcelTree.RemoveExactElement(keys2, node1[0].Data);

                node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(1).LengthPosition;

                Key key5 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key6 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys3 = [key5, key6];

                _parcelTree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key8 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys4 = [key7, key8];

                _parcelTree.AddElement(keys4, node1[0].Data);
            }
        }

    }
}
