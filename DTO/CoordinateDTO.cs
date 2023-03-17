using DoraTheExplorer.Structure;

namespace DoraTheExplorer.DTO;

public struct CoordinateDTO
{
    public int x { get; set; }
    public int y { get; set; }

    public static CoordinateDTO From(Coordinate coord)
    {
        CoordinateDTO dto = new CoordinateDTO();
        dto.x = coord.x;
        dto.y = coord.y;
        return dto;
    }
}
