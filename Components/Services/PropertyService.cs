using System.CodeDom.Compiler;
using System.Text;

public class PropertyService
{
    private KDTree<Property> _propertyTree;
    private int _actualPropId;
    private HomeService _home;
    private readonly Random random;
    private readonly PropertyParcelMediator _mediator;

    public PropertyService(HomeService homeService, PropertyParcelMediator mediator)
    {
        _home = homeService;
        _propertyTree = new KDTree<Property>();
        _actualPropId = 0;
        random = new Random();
        _mediator = mediator;
    }

    internal KDTree<Property> GetAllProperties()
    {
        return _propertyTree;
    }

    internal void AddProperty(int inventNo, string propDesc, char gps1Width, double gps1WidthPosition, char gps1Length, double gps1LengthPosition, char gps2Width, double gps2WidthPosition, char gps2Length, double gps2LengthPosition)
    {
        Property property = new Property(_actualPropId, inventNo, propDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);

        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];
        _propertyTree.AddElement(keys, property); 

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];
        _propertyTree.AddElement(keys2, property);
        
        _home.AddProperty(_actualPropId, inventNo, propDesc, gps1Width, gps1WidthPosition, gps1Length, gps1LengthPosition, gps2Width, gps2WidthPosition, gps2Length, gps2LengthPosition);
        ++_actualPropId;

        List<Parcel> parcels1 = new();
        List<Parcel> parcels2 = new();

        parcels1 = _mediator.SearchParcels(property.GpsPosHandler.GetGpsPosition(0).WidthPosition, property.GpsPosHandler.GetGpsPosition(0).LengthPosition);
        parcels2 = _mediator.SearchParcels(property.GpsPosHandler.GetGpsPosition(1).WidthPosition, property.GpsPosHandler.GetGpsPosition(1).LengthPosition);
        
        foreach(var parc1 in parcels1) {
            property.AddParcel(parc1);
        }

        foreach(var parc2 in parcels2) {
            property.AddParcel(parc2);
        }
    }

    internal List<Property> SearchProperties(double gpsWToSearch, double gpsLToSearch) {
        
        List<Key> keys = [new Key(gpsWToSearch),  new Key(gpsLToSearch)];

        List<Property> copyProperties = new List<Property>();

        var foundElements = _propertyTree.FindElement(keys);
        if (foundElements == null || !foundElements.Any())
        {
            return copyProperties;
        }

        foreach(var copyProp in foundElements) {
            if(copyProp is not null) {
                GpsPosHandler gpsHand = new GpsPosHandler();
                GpsPosition gps1 = new GpsPosition(copyProp.GpsPosHandler.GpsPositioons[0].Width, copyProp.GpsPosHandler.GpsPositioons[0].WidthPosition, copyProp.GpsPosHandler.GpsPositioons[0].Length, copyProp.GpsPosHandler.GpsPositioons[0].LengthPosition);
                gpsHand.GpsPositioons[0] = gps1;
                GpsPosition gps2 = new GpsPosition(copyProp.GpsPosHandler.GpsPositioons[1].Width, copyProp.GpsPosHandler.GpsPositioons[1].WidthPosition, copyProp.GpsPosHandler.GpsPositioons[1].Length, copyProp.GpsPosHandler.GpsPositioons[1].LengthPosition);
                gpsHand.GpsPositioons[1] = gps2;
                
                Property copyProperty = new Property(copyProp.PropertyId, copyProp.InventNo, copyProp.PropDesc, gpsHand.GpsPositioons);
                copyProperties.Add(copyProperty);
            }
        }

        return copyProperties;
    }

    internal void RemoveProperty(Property property) {
        Key key1 = new Key(property.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(property.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        _propertyTree.RemoveExactElement(keys, property);

        Key key3 = new Key(property.GpsPosHandler.GetGpsPosition(1).WidthPosition);
        Key key4 = new Key(property.GpsPosHandler.GetGpsPosition(1).LengthPosition);

        List<Key> keys2 = [key3, key4];

        _propertyTree.RemoveExactElement(keys2, property);
        _home.RemoveProp(property);

        List<Parcel> parcels1 = new();
        List<Parcel> parcels2 = new();

        parcels1 = _mediator.SearchParcels(property.GpsPosHandler.GetGpsPosition(0).WidthPosition, property.GpsPosHandler.GetGpsPosition(0).LengthPosition);
        parcels2 = _mediator.SearchParcels(property.GpsPosHandler.GetGpsPosition(1).WidthPosition, property.GpsPosHandler.GetGpsPosition(1).LengthPosition);
        
        foreach(var parc1 in parcels1) {
            int i = 0;
            foreach(var prop1 in parc1.GetProperties()) {
                if(prop1.Equals(property)) {
                    parc1.RemoveProperty(i);
                }
                ++i;
            }
        }

        foreach(var parc2 in parcels2) {
            int i = 0;
            foreach(var prop2 in parc2.GetProperties()) {
                if(prop2.Equals(property)) {
                    parc2.RemoveProperty(i);
                }
                ++i;
            }
        }
    }

    internal void EditProperty(Property oldProp, Property newProp) {

        Key key1 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).WidthPosition);
        Key key2 = new Key(oldProp.GpsPosHandler.GetGpsPosition(0).LengthPosition);

        List<Key> keys = [key1, key2];

        List<Node<Property>> node1 = _propertyTree.FindExactNode(keys, oldProp);

        if(node1 is not null) {
            if(newProp.InventNo != 0) {
                node1[0].Data.InventNo = newProp.InventNo;
            }
            if(newProp.PropDesc != "") {
                node1[0].Data.PropDesc = newProp.PropDesc;
            }
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Width = newProp.GpsPosHandler.GetGpsPosition(0).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;
            node1[0].Data.GpsPosHandler.GetGpsPosition(0).Length = newProp.GpsPosHandler.GetGpsPosition(0).Length;
            node1[0].Data.GpsPosHandler.GetGpsPosition(1).Width = newProp.GpsPosHandler.GetGpsPosition(1).Width;

            if(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition != newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition ||
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition != newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition) {

                Key key9 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key10 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys5 = [key9, key10];

                _propertyTree.RemoveExactElement(keys5, node1[0].Data);

                Key key3 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key4 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys2 = [key3, key4];

                _propertyTree.RemoveExactElement(keys2, node1[0].Data);

                List<Parcel> parcels1 = new();
                List<Parcel> parcels2 = new();

                parcels1 = _mediator.SearchParcels(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);
                parcels2 = _mediator.SearchParcels(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);
                
                foreach(var parc1 in parcels1) {
                    int i = 0;
                    foreach(var prop1 in parc1.GetProperties()) {
                        if(prop1.Equals(node1[0].Data)) {
                            parc1.RemoveProperty(i);
                        }
                        ++i;
                    }
                }

                foreach(var parc2 in parcels2) {
                    int i = 0;
                    foreach(var prop2 in parc2.GetProperties()) {
                        if(prop2.Equals(node1[0].Data)) {
                            parc2.RemoveProperty(i);
                        }
                        ++i;
                    }
                }

                node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(0).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(0).LengthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition = newProp.GpsPosHandler.GetGpsPosition(1).WidthPosition;
                node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition = newProp.GpsPosHandler.GetGpsPosition(1).LengthPosition;

                Key key5 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition);
                Key key6 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);

                List<Key> keys3 = [key5, key6];

                _propertyTree.AddElement(keys3, node1[0].Data);

                Key key7 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition);
                Key key8 = new Key(node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);

                List<Key> keys4 = [key7, key8];

                _propertyTree.AddElement(keys4, node1[0].Data);
                List<Parcel> parcels3 = new();
                List<Parcel> parcels4 = new();

                parcels1 = _mediator.SearchParcels(node1[0].Data.GpsPosHandler.GetGpsPosition(0).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(0).LengthPosition);
                parcels2 = _mediator.SearchParcels(node1[0].Data.GpsPosHandler.GetGpsPosition(1).WidthPosition, node1[0].Data.GpsPosHandler.GetGpsPosition(1).LengthPosition);
                
                foreach(var parc1 in parcels3) {
                    node1[0].Data.AddParcel(parc1);
                }

                foreach(var parc2 in parcels4) {
                    node1[0].Data.AddParcel(parc2);
                }
            }
        }
        _home.EditProperty(oldProp, newProp);
    }

    internal void GenerateProp(int num, double perc, List<List<double>> keys) {
        char[] directions = { 'N', 'S', 'W', 'E' };
        int keyIndex;
        perc = perc/100;

        for(int i = 0; i < num; ++i) {
            double gpsW1 = Math.Round(random.NextDouble() * 50,2);
            double gpsL1 = Math.Round(random.NextDouble() * 50,2);
            double gpsW2 = Math.Round(random.NextDouble() * 50,2);
            double gpsL2 = Math.Round(random.NextDouble() * 50,2);
            int inventNo = random.Next(50);

            Console.WriteLine(perc);
            double cover = random.NextDouble();
            Console.WriteLine(cover);
            if(cover <= perc) {
                keyIndex =random.Next(keys.Count);
                gpsW1 = keys[keyIndex][0];
                gpsL1 = keys[keyIndex][1];
                Console.WriteLine($"keys W - {gpsW1} L - {gpsL1}");
            }

            int maxLeng = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder stringBuilder = new StringBuilder(maxLeng);

            for (int j = 0; j < maxLeng; j++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            string propDesc = stringBuilder.ToString();
            AddProperty(inventNo, propDesc, directions[random.Next(directions.Length)], gpsW1, directions[random.Next(directions.Length)], gpsL1, directions[random.Next(directions.Length)], gpsW2, directions[random.Next(directions.Length)], gpsL2);
        }
    }

    public void SaveState(string filePath) {
        using (StreamWriter writer = new StreamWriter(filePath)) {
            //writer.WriteLine($"ACTUALPARCID:{_actualParcId};");
            foreach(var prop in _propertyTree.LevelOrderIter()) {
                string line = prop.Serialize();
                writer.WriteLine(line);
            }
        }
    }

    public void LoadState(string filePath) {
        List<int> propIds = new List<int>();
        using (StreamReader reader = new StreamReader(filePath)) {
            //string? actualParcIdLine = reader.ReadLine();

            //if (actualPropIdLine != null && actualPropIdLine.StartsWith("ACTUALPROPID:")) {
                //_actualPropId = int.Parse(actualPropIdLine.Split(':')[1].TrimEnd(';'));
            //}

            string? line;
            while ((line = reader.ReadLine()) != null) {
                Property property = new Property();
                property.DeSerialize(line);
                if (!propIds.Contains(property.PropertyId)) {
                    propIds.Add(property.PropertyId);
                    AddProperty(property.InventNo, property.PropDesc, property.GpsPosHandler.GpsPositioons[0].Width, property.GpsPosHandler.GpsPositioons[0].WidthPosition, property.GpsPosHandler.GpsPositioons[0].Length, property.GpsPosHandler.GpsPositioons[0].LengthPosition, property.GpsPosHandler.GpsPositioons[1].Width, property.GpsPosHandler.GpsPositioons[1].WidthPosition, property.GpsPosHandler.GpsPositioons[1].Length, property.GpsPosHandler.GpsPositioons[1].LengthPosition);
                }
            }
        }
    }
}
