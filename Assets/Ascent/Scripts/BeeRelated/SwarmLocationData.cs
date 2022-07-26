using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mount this on the parent object of the bee swarms.
/// </summary>
public class SwarmLocationData : MonoBehaviour
{
    [System.Serializable]
    public class LocationAndActive
    {
        public Vector3 Location;
        public bool isOccupied;
    }

    [SerializeField]
    private List<LocationAndActive> Locations;

    /// <summary>
    /// Returns a random unoccupied location from Locations list and sets that location to be occupied.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEmptyLocationAndSetOccupied()
    {
        List<Vector3> availableLocations = new();
        Vector3 returnLocation;

        for (int i = 0; i < Locations.Count; i++)
        {
            if (Locations[i].isOccupied == false)
            {
                availableLocations.Add(Locations[i].Location);
            }
        }

        returnLocation = availableLocations[Random.Range(0, availableLocations.Count)];
        SetLocationState(returnLocation, true);

        return returnLocation;
    }

    /// <summary>
    /// Looks through Locations list for match and sets isOccupied to passed value for that location.
    /// </summary>
    /// <param name="_location"></param>
    /// <param name="_isOccupied"></param>
    public void SetLocationState(Vector3 _location, bool _isOccupied)
    {
        for(int i = 0; i < Locations.Count; i++)
        {
            if (_location == Locations[i].Location)
            {
                Locations[i].isOccupied = _isOccupied;
            }
        }
    }




}
