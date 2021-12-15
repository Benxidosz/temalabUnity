using System.Linq;
using Map;
using Unity.Netcode;
using UnityEngine;

public class Robber : NetworkBehaviour
{
    
    private TileController _tile;

    [ServerRpc(RequireOwnership = false)]
    private void MoveRobberServerRPC(Vector3 position)
    {
        var tiles = GameObject.FindGameObjectsWithTag("Tile").ToList();
        var newTile = tiles.Find(go => Vector3.Distance(go.transform.position, position) < 1f);
        transform.position = newTile.transform.position + new Vector3(0, 0.05f, 0);
        if (_tile != null)
        {
            _tile.GetComponent<TileController>().Block = false;
        }

        var ctr = newTile.GetComponent<TileController>();
        ctr.Block = true;
        _tile = ctr;
    }
    
    public void ChangeTile(GameObject newTile)
    {
        MoveRobberServerRPC(newTile.transform.position);
    }
}