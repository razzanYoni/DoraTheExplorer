using DoraTheExplorer.Structure;
using System.Collections.Generic;

namespace DoraTheExplorer.DTO;

public class StateDTO
{
    public IList<CoordinateDTO>? VisitedLocations { get; set; }
    public IList<CoordinateDTO>? BacktrackLocations { get; set; }
    public CoordinateDTO CurrentLocation { get; set; }
    public int Step { get; set; }

    public static StateDTO From(State state)
    {
        StateDTO dto = new StateDTO();
        dto.VisitedLocations = new List<CoordinateDTO>();
        foreach (Coordinate data in state.VisitedLocations)
        {
            dto.VisitedLocations.Add(CoordinateDTO.From(data));
        }
        dto.BacktrackLocations = new List<CoordinateDTO>();
        foreach (Coordinate data in state.BacktrackLocations)
        {
            dto.BacktrackLocations.Add(CoordinateDTO.From(data));
        }
        dto.CurrentLocation = CoordinateDTO.From(state.CurrentLocation);
        dto.Step = state.Step;
        return dto;
    }
}
