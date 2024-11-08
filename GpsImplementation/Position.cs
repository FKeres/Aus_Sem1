class Position :  IComparable<Position>, IComparable
{
    private char _direction;
    private double _dirPos;

    public char Direction { get => _direction; set => _direction = value; }
    public double DirPos { get => _dirPos; set => _dirPos = value; }

    public Position(char direction, double dirPos) {
        _direction = direction;
        _dirPos = dirPos;
    }

    public int CompareTo(Position? other)
    {
        double dirOther = other.Direction == 'S' || other.Direction == 'W' ? -1*other.DirPos : other.DirPos;
        double dirThis = _direction == 'S' || _direction == 'W' ? -1*_dirPos : _dirPos;

        return dirThis.CompareTo(dirOther);
    }

    public int CompareTo(object? obj)
    {
        if (obj is Position otherPosition)
        {
            return CompareTo(otherPosition);
        }

        throw new ArgumentException("Object is not a Position");
    }
}