using DoraTheExplorer.Structure;

namespace DoraTheExplorer.DTO;

public class CellDTO
{
    public CoordinateDTO Coordinate { get; set; }
    public bool Visitable { get; set; }

    public static CellDTO From(Cell cell)
    {
        CellDTO dto = new CellDTO();
        dto.Coordinate = CoordinateDTO.From(cell.Coord);
        dto.Visitable = cell.Visitable;
        return dto;
    }
}
