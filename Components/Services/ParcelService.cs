using System.Text;
using AusSemClient_1_.Components.Pages;

public class ParcelService
{
    private KDTree<Parcel> _parcelTree;
    private int _actualParcId;
    private HomeService _home;
    private readonly Random random;
    private readonly PropertyParcelMediator _mediator;

    public ParcelService(HomeService homeService, PropertyParcelMediator mediator)
    {
        _home = homeService;
        _parcelTree = new KDTree<Parcel>();
        _actualParcId = 0;
        random = new Random();
        _mediator = mediator;
    }

    internal KDTree<Parcel> GetAllParcels()
    {
        return _parcelTree;
    }

    internal void AddParcel(int parcNo, string parcDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Parcel parcel = new Parcel(_actualParcId, parcNo, parcDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);
        Key key1 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(0).Width, parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition));
        Key key2 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(0).Length, parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition));

        List<Key> keys = [key1, key2];
        _parcelTree.AddElement(keys, parcel); 

        Key key3 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(1).Width, parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition));
        Key key4 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(1).Length, parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition));

        List<Key> keys2 = [key3, key4];
        _parcelTree.AddElement(keys2, parcel);
        _home.AddParcel(_actualParcId, parcNo, parcDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);

        ++_actualParcId;
        List<Property> properties1 = new();
        List<Property> properties2 = new();

        properties1 = _mediator.SearchProperties(parcel.GpsPosHandler.GetGpsPosition(0).Width, parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition, parcel.GpsPosHandler.GetGpsPosition(0).Length, parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition);
        properties2 = _mediator.SearchProperties(parcel.GpsPosHandler.GetGpsPosition(1).Width, parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition, parcel.GpsPosHandler.GetGpsPosition(1).Length, parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition);
        
        foreach(var prop1 in properties1) {
            parcel.AddProperty(prop1);
            prop1.AddParcel(parcel);
        }

        foreach(var prop2 in properties2) {
            parcel.AddProperty(prop2);
            prop2.AddParcel(parcel);
        }
    }

    internal List<Parcel> SearchParcels(char width, double gpsWToSearch, char length, double gpsLToSearch) {

        List<Key> keys = [new Key(new Position(width, gpsWToSearch)),  new Key(new Position(length, gpsLToSearch))];

        List<Parcel> copyParcels = new List<Parcel>();

        var foundElements = _parcelTree.FindElement(keys);
        if (foundElements == null || !foundElements.Any())
        {
            return copyParcels;
        }

        foreach(var copyParc in foundElements) {
            if(copyParc is not null) {

                GpsPosHandler gpsHand = new GpsPosHandler();
                GpsPosition gps1 = new GpsPosition(copyParc.GpsPosHandler.GpsPositioons[0].Width, copyParc.GpsPosHandler.GpsPositioons[0].WidthPosition, copyParc.GpsPosHandler.GpsPositioons[0].Length, copyParc.GpsPosHandler.GpsPositioons[0].LengthPosition);
                gpsHand.GpsPositioons[0] = gps1;
                GpsPosition gps2 = new GpsPosition(copyParc.GpsPosHandler.GpsPositioons[1].Width, copyParc.GpsPosHandler.GpsPositioons[1].WidthPosition, copyParc.GpsPosHandler.GpsPositioons[1].Length, copyParc.GpsPosHandler.GpsPositioons[1].LengthPosition);
                gpsHand.GpsPositioons[1] = gps2;

                Parcel copyParcel = new Parcel(copyParc.ParcelId, copyParc.ParcNo, copyParc.ParcDesc, gpsHand.GpsPositioons);
                copyParcel.SetProperties(copyParc.GetProperties());
                copyParcels.Add(copyParcel);
            }
        }

        return copyParcels;
    }

    internal void RemoveParcel(Parcel parcel) {
        Key key1 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(0).Width, parcel.GpsPosHandler.GetGpsPosition(0).WidthPosition));
        Key key2 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(0).Length, parcel.GpsPosHandler.GetGpsPosition(0).LengthPosition));

        List<Key> keys = [key1, key2];

        _parcelTree.RemoveExactElement(keys, parcel);

        Key key3 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(1).Width, parcel.GpsPosHandler.GetGpsPosition(1).WidthPosition));
        Key key4 = new Key(new Position(parcel.GpsPosHandler.GetGpsPosition(1).Length, parcel.GpsPosHandler.GetGpsPosition(1).LengthPosition));

        List<Key> keys2 = [key3, key4];

        _parcelTree.RemoveExactElement(keys2, parcel);
        _home.RemoveParc(parcel);

        foreach(var prop in parcel.GetProperties()) {
            prop.RemoveParcelEq(parcel);
        }
    }
    
    internal void EditParcel(Parcel oldParc, Parcel newParc) {
        Key key1 = new Key(new Position(oldParc.GpsPosHandler.GetGpsPosition(0).Width, oldParc.GpsPosHandler.GetGpsPosition(0).WidthPosition));
        Key key2 = new Key(new Position(oldParc.GpsPosHandler.GetGpsPosition(0).Length, oldParc.GpsPosHandler.GetGpsPosition(0).LengthPosition));

        List<Key> keys = [key1, key2];

        List<Node<Parcel>> node1 = _parcelTree.FindExactNode(keys, oldParc);

        if(node1 is not null) {
            if(newParc.ParcNo != 0) {
                node1[0].Data.ParcNo = newParc.ParcNo;
            }
            if(newParc.ParcDesc != "") {
                node1[0].Data.ParcDesc = newParc.ParcDesc;
            }

            if(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition != newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition != newParc.GpsPosHandler.GetGpsPosition(1).LengthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width != newParc.GpsPosHandler.GetGpsPosition(0).Width ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width != newParc.GpsPosHandler.GetGpsPosition(1).Width ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length != newParc.GpsPosHandler.GetGpsPosition(0).Length ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).Length != newParc.GpsPosHandler.GetGpsPosition(1).Length) {

                Key key9 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition));
                Key key10 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition));

                List<Key> keys5 = [key9, key10];

                _parcelTree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition));
                Key key4 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(1).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition));

                List<Key> keys2 = [key3, key4];

                _parcelTree.RemoveExactElement(keys2, node1[0].Data);
                
                foreach(var prop in node1[0].Data.GetProperties()) {
                    prop.RemoveParcelEq(node1[0].Data);
                }

                node1[0].Data.SetProperties(new List<Property>());

                node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition = newParc.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition = newParc.GpsPosHandler.GetGpsPosition(1).LengthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width = newParc.GpsPosHandler.GetGpsPosition(0).Width;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newParc.GpsPosHandler.GetGpsPosition(1).Width;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length = newParc.GpsPosHandler.GetGpsPosition(0).Length;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).Length = newParc.GpsPosHandler.GetGpsPosition(1).Length;

                Key key5 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition));
                Key key6 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition));

                List<Key> keys3 = [key5, key6];

                _parcelTree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition));
                Key key8 = new Key(new Position(node1[0].Data.GpsPosHandler.GetGpsPosition(1).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition));

                List<Key> keys4 = [key7, key8];

                _parcelTree.AddElement(keys4, node1[0].Data);

                List<Property> properties3 = new();
                List<Property> properties4 = new();

                properties3 = _mediator.SearchProperties(node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);
                properties4 = _mediator.SearchProperties(node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width, node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(1).Length, node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);
                
                foreach(var prop1 in properties3) {
                    node1[0].Data.AddProperty(prop1);
                    prop1.AddParcel(node1[0].Data);
                }

                foreach(var prop2 in properties4) {
                    node1[0].Data.AddProperty(prop2);
                    prop2.AddParcel(node1[0].Data);
                }
            }
        }

        _home.EditParcel(oldParc, newParc);

    }

    internal List<List<Position>> GenerateParc(int num) {
        List<List<Position>> keys = new List<List<Position>>();
        char[] directionsW = { 'W', 'E' };
        char[] directionsL = { 'N', 'S'};

        for(int i = 0; i < num; ++i) {
            double gpsW1 = Math.Round(random.NextDouble() * 50,2);
            double gpsL1 = Math.Round(random.NextDouble() * 50,2);
            double gpsW2 = Math.Round(random.NextDouble() * 50,2);
            double gpsL2 = Math.Round(random.NextDouble() * 50,2);
            int parcNo = random.Next(50);

            int maxLeng = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder stringBuilder = new StringBuilder(maxLeng);

            for (int j = 0; j < maxLeng; j++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            string parcDesc = stringBuilder.ToString();
            char width1 = directionsW[random.Next(directionsW.Length)];
            char length1 = directionsL[random.Next(directionsL.Length)];
            char width2 = directionsW[random.Next(directionsW.Length)];
            char length2 = directionsL[random.Next(directionsL.Length)];
            AddParcel(parcNo, parcDesc, width1, gpsW1, length1, gpsL1, width2, gpsW2, length2, gpsL2);

            List<Position> keys1 = [new Position(width1, gpsW1), new Position(length1, gpsL1)];
            keys.Add(keys1);
        }

        return keys;
    }

    public void SaveState(string filePath) {
        using (StreamWriter writer = new StreamWriter(filePath)) {
            writer.WriteLine("PARCELID,PARCNO,PARCDESC,GPSW1,GPSWP1,GPSL1,GPSLP1,GPSW2,GPSWP2,GPSL2,GPSLP2");

            foreach (var parc in _parcelTree.LevelOrderIter()) {
                string line = parc.Serialize();
                writer.WriteLine(line);
            }
        }
    }


    public void LoadState(string filePath) {
        List<int> parcIds = new List<int>();
        using (StreamReader reader = new StreamReader(filePath)) {
            string? headerLine = reader.ReadLine();

            string? line;
            while ((line = reader.ReadLine()) != null) {
                Parcel parcel = new Parcel();
                parcel.DeSerialize(line);
                if (!parcIds.Contains(parcel.ParcelId)) {
                    parcIds.Add(parcel.ParcelId);
                    AddParcel(parcel.ParcNo, parcel.ParcDesc, parcel.GpsPosHandler.GpsPositioons[0].Width, parcel.GpsPosHandler.GpsPositioons[0].WidthPosition, parcel.GpsPosHandler.GpsPositioons[0].Length, parcel.GpsPosHandler.GpsPositioons[0].LengthPosition, parcel.GpsPosHandler.GpsPositioons[1].Width, parcel.GpsPosHandler.GpsPositioons[1].WidthPosition, parcel.GpsPosHandler.GpsPositioons[1].Length, parcel.GpsPosHandler.GpsPositioons[1].LengthPosition);
                }
            }
        }
    }

    internal List<Parcel> GetAll() {
        List<Parcel> copyParcels = new List<Parcel>();

        var foundElements = _parcelTree.InOrder();
        if (foundElements == null || !foundElements.Any())
        {
            return copyParcels;
        }

        foreach(var copyParc in foundElements) {
            if(copyParc is not null) {

                GpsPosHandler gpsHand = new GpsPosHandler();
                GpsPosition gps1 = new GpsPosition(copyParc.Data.GpsPosHandler.GpsPositioons[0].Width, copyParc.Data.GpsPosHandler.GpsPositioons[0].WidthPosition, copyParc.Data.GpsPosHandler.GpsPositioons[0].Length, copyParc.Data.GpsPosHandler.GpsPositioons[0].LengthPosition);
                gpsHand.GpsPositioons[0] = gps1;
                GpsPosition gps2 = new GpsPosition(copyParc.Data.GpsPosHandler.GpsPositioons[1].Width, copyParc.Data.GpsPosHandler.GpsPositioons[1].WidthPosition, copyParc.Data.GpsPosHandler.GpsPositioons[1].Length, copyParc.Data.GpsPosHandler.GpsPositioons[1].LengthPosition);
                gpsHand.GpsPositioons[1] = gps2;

                Parcel copyParcel = new Parcel(copyParc.Data.ParcelId, copyParc.Data.ParcNo, copyParc.Data.ParcDesc, gpsHand.GpsPositioons);
                copyParcels.Add(copyParcel);
            }
        }

        return copyParcels;
    }

    internal List<Property> GetProperties(Parcel parcel) {
        return parcel.GetProperties();
    }

}
