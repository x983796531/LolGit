using FixedPointy;

public class Line
{
    public Line(Fix x1,Fix y1, Fix x2, Fix y2)
    {
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
    }
    public Line()
    {

    }
    //
    // Summary:
    //     Gets or sets the x-coordinate of the System.Windows.Shapes.Line start point.
    //
    // Returns:
    //     The x-coordinate for the start point of the line. The default is 0.

    public Fix X1 { get; set; }
    //
    // Summary:
    //     Gets or sets the x-coordinate of the System.Windows.Shapes.Line end point.
    //
    // Returns:
    //     The x-coordinate for the end point of the line. The default is 0.

    public Fix X2 { get; set; }
    //
    // Summary:
    //     Gets or sets the y-coordinate of the System.Windows.Shapes.Line start point.
    //
    // Returns:
    //     The y-coordinate for the start point of the line. The default is 0.

    public Fix Y1 { get; set; }
    //
    // Summary:
    //     Gets or sets the y-coordinate of the System.Windows.Shapes.Line end point.
    //
    // Returns:
    //     The y-coordinate for the end point of the line. The default is 0.

    public Fix Y2 { get; set; }
}
