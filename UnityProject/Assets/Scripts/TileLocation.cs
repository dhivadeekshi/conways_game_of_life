public class TileLocation
{
    public int row;
    public int col;

    public TileLocation(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public override string ToString()
    {
        return "(" + row + "," + col + ")";
    }

    public override bool Equals(object obj)
    {
        return ((TileLocation)obj).row == this.row &&
            ((TileLocation)obj).col == this.col;
    }
}
