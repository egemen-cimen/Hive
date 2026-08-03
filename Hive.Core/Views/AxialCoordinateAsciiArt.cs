using Hive.Core.Models;
using System.Text;

namespace Hive.Core.Views
{
    public static class AxialCoordinateAsciiArt
    {
        const string INDENT = "      ";
        const int COORDINATE_COMPONENT_WIDTH = 4;

        public static string GenerateAsciiArt(AxialCoordinateSystem coordinateSystem)
        {

            var stringBuilder = new StringBuilder();
            var allCoordinates = coordinateSystem.GetAllCoordinates();

            var topMostRow = allCoordinates.Min(c => c.row);
            var bottomMostRow = allCoordinates.Max(c => c.row);

            // Calculate the indentations for the ascii art
            IEnumerable<(int column, int row, int indent)> allCoordinatesAndIndents = allCoordinates.Select(c
                => (c.column, c.row, 2 * c.column + c.row));
            // Normalize indents to start from 0
            var minIndent = allCoordinatesAndIndents.Min(ci => ci.indent);
            allCoordinatesAndIndents = allCoordinatesAndIndents.Select(c => (c.column, c.row, c.indent - minIndent));

            for (var row = topMostRow; row <= bottomMostRow; row++)
            {
                stringBuilder.AppendLine();
                var coordinatesInThisRow = allCoordinatesAndIndents.Where(ci => ci.row == row);
                var largestIndentForThisRow = coordinatesInThisRow.Max(ci => ci.indent);

                foreach (var coordinateIndent in coordinatesInThisRow)
                {
                    for (var i = 0; i < coordinateIndent.indent; i++)
                    {
                        stringBuilder.Append(INDENT);
                    }

                    if (allCoordinates.Contains((coordinateIndent.column, row)))
                    {
                        stringBuilder.Append('[');
                        stringBuilder.Append($"{coordinateIndent.column,COORDINATE_COMPONENT_WIDTH}");
                        stringBuilder.Append(',');
                        stringBuilder.Append($"{row,COORDINATE_COMPONENT_WIDTH}");
                        if (coordinateIndent.indent == largestIndentForThisRow)
                        {
                            stringBuilder.Append(']');
                        }
                        else
                        {
                            stringBuilder.Append("] ");
                        }
                    }
                    else
                    {
                        stringBuilder.Append(INDENT);
                        stringBuilder.Append(INDENT);
                    }
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}
