public class PropertyParcelMediator
{
    private readonly IServiceProvider _serviceProvider;

    public PropertyParcelMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal List<Property> SearchProperties(double widthPosition, double lengthPosition)
    {
        var propertyService = _serviceProvider.GetService<PropertyService>();
        return propertyService.SearchProperties(widthPosition, lengthPosition);
    }

    internal List<Parcel> SearchParcels(double widthPosition, double lengthPosition)
    {
        var parcelService = _serviceProvider.GetService<ParcelService>();
        return parcelService.SearchParcels(widthPosition, lengthPosition);
    }
}
