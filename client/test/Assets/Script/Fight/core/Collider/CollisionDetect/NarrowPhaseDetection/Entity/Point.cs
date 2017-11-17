using FixedPointy;

public class Point
{
    //
    // Summary:
    //     Gets or sets the System.Windows.Point.X-coordinate value of this System.Windows.Point
    //     structure.
    //
    // Returns:
    //     The System.Windows.Point.X-coordinate value of this System.Windows.Point structure.
    //     The default value is 0.
    public Fix X { get; set; }
    //
    // Summary:
    //     Gets or sets the System.Windows.Point.Y-coordinate value of this System.Windows.Point.
    //
    // Returns:
    //     The System.Windows.Point.Y-coordinate value of this System.Windows.Point structure.
    //     The default value is 0.
    public Fix Y { get; set; }
    public Point(Fix x, Fix y)
    {
        this.X = x;
        this.Y = y;
    }

    public Point()
    {
    }
}
