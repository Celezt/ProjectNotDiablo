using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    [Range(3, 64)]
    public int width = 3;

    [Range(3, 64)]
    public int height = 3;

    public List<Vector2Int> connections;

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos.y += 0.5f;

        Vector3 rotAxis;
        float rotAngle;
        transform.rotation.ToAngleAxis(out rotAngle, out rotAxis);
        float w;
        float h;
        if (Mathf.Abs(rotAngle) % 180.0f > 89.99f) {
            w = height;
            h = width;
        }
        else {
            w = width;
            h = height;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(pos, new Vector3(w, 1.0f, h));
        
        Gizmos.color = Color.green;
        for (int i = 0; i < connections.Count; i++) {
            pos = new Vector3(connections[i].x, 0.0f, connections[i].y);
            pos = transform.position + transform.rotation * pos;

            Gizmos.DrawWireCube(pos, new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    public Vector2Int GetRotatedBounds(float angle) {
        Vector2Int ret = new Vector2Int();
        if (Mathf.Abs(angle) % 180.0f > 89.9f) {
            ret.x = height;
            ret.y = width;
        }
        else {
            ret.x = width;
            ret.y = height;
        }

        return ret;
    }

    public List<Vector2Int> GetRotatedConnections(float angle) {
        List<Vector2Int> ret = new List<Vector2Int>(connections.Count);
        Quaternion rot = Quaternion.AngleAxis(angle,
            new Vector3(0.0f, 1.0f, 0.0f));

        Vector3 pos;
        for (int i = 0; i < connections.Count; i++) {
            pos = new Vector3(connections[i].x, 0.0f, connections[i].y);
            pos = rot * pos;

            ret.Add(new Vector2Int((int)pos.x, (int)pos.z));
        }

        return ret;
    }

}
