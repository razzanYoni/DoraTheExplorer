using DoraTheExplorer.Structure;
using System.Collections.Generic;

namespace DoraTheExplorer.DTO;

public class StateDTO
{
    public CoordinateDTO CurrentLocation { get; set; }
    public IList<CoordinateDTO>? VisitedLocations { get; set; }
    public IList<CoordinateDTO>? BacktrackLocations { get; set; }
    public IList<CoordinateDTO>? SavedVisitedLocations { get; set; }

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
        dto.SavedVisitedLocations = new List<CoordinateDTO>();
        foreach (Coordinate data in state.SavedVisitedLocations)
        {
            dto.SavedVisitedLocations.Add(CoordinateDTO.From(data));
        }
        return dto;
    }
}
