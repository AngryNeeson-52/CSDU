using UnityEngine;

public interface IServerObject
{
    public void SetPos(Vector3 _pos, float _yaw);
    public void StopMove();

    public void Kill();
}