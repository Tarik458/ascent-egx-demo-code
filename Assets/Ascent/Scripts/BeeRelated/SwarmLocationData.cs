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
        public Transform Location;
        public bool isOccupied;
    }

    [SerializeField]
    [Header("Use empty game objects to set transform positions.")]
    [Header("0th element broken dont use.")]
    private List<LocationAndActive> Locations;

    /// <summary>
    /// Returns a random unoccupied location from Locations list and sets that location to be occupied.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetEmptyLocationAndSetOccupied()
    {
        List<Transform> availableLocations = new();
        Transform returnLocation;

        // i = 1 bc of broken 0th inspector element.
        for (int i = 1; i < Locations.Count; i++)
        {
            if (Locations[i].isOccupied == false && Locations[i].Location != null)
            {
                availableLocations.Add(Locations[i].Location);
            }
        }

        returnLocation = availableLocations[Random.Range(0, availableLocations.Count)];
        SetLocationState(returnLocation.position, true);

        return returnLocation.position;
    }

    /// <summary>
    /// Looks through Locations list for match and sets isOccupied to passed value for that location.
    /// </summary>
    /// <param name="_location"></param>
    /// <param name="_isOccupied"></param>
    public void SetLocationState(Vector3 _location, bool _isOccupied)
    {
        // i = 1 bc of broken 0th inspector element.
        for (int i = 1; i < Locations.Count; i++)
        {
            if (_location == Locations[i].Location.position && Locations[i].Location != null)
            {
                Locations[i].isOccupied = _isOccupied;
            }
        }
    }




}
