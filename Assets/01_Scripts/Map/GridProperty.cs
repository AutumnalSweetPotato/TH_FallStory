[System.Serializable]
public class GridProperty
{
    public GridCoordinate gridCoordinate;
    public GridBoolProperty gridBoolProperty;
    public bool gridBoolValue;

    public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue)
    {
        this.gridCoordinate = gridCoordinate;
        this.gridBoolProperty = gridBoolProperty;
        this.gridBoolValue = gridBoolValue;
    }
}