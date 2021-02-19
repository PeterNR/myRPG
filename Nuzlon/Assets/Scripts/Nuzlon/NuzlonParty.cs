using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class NuzlonParty : MonoBehaviour
{
    [SerializeField]
    private List<Nuzlon> _nuzlon;

    public List<Nuzlon> NuzlonList
    {
        get
        {
            return _nuzlon;
        }
    }

    private void Start()
    {
        foreach(Nuzlon nuzlon in _nuzlon )
        {
            nuzlon.Initialize();
        }
    }

    public Nuzlon GetHealthyNuzlon()
    {
        return _nuzlon.Where(x => x.CurrentHP >0).FirstOrDefault();
    }
}
