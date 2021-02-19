using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField]
    private List<Nuzlon> _wildNuzlonList;

    public Nuzlon GetRandomWildNuzlon()
    {
        Nuzlon wildNuzlon = _wildNuzlonList[Random.Range(0, _wildNuzlonList.Count)];
        wildNuzlon.Initialize();
        return wildNuzlon;
    }
}
