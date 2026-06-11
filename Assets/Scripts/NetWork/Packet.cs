using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ServerPackets
{
    Connected,
    NickNameResult,
    PlayerUpdate,
    SnapShot,
    GetHit,
    Dead,
}

public enum ClientPackets
{
    NickNameCheck,
    PlayersRequest,
    Move,
    Fire,
    Resapwn,
    QuitGame,
}

public class Packet : IDisposable
{
    List<byte> buffer = new List<byte>();
    byte[] readableBuffer = null;
    int readPos = 0;

    private bool disposed = false;



    #region initialize

    public Packet()
    {
        buffer = new List<byte>();
        readPos = 0;
    }

    public Packet(int _id)
    {
        Write(_id);
    }

    public Packet(byte[] _data)
    {
        SetBytes(_data);
    }
    #endregion

    #region Functions

    public void SetBytes(byte[] _data)
    {
        Write(_data);
        readableBuffer = buffer.ToArray();
    }

    public void WriteLength()
    {
        buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
    }

    public byte[] ToArray()
    {
        readableBuffer = buffer.ToArray();
        return readableBuffer;
    }

    public int Length()
    {
        return buffer.Count;
    }

    public int UnReadLength()
    {
        return Length() - readPos;
    }

    public void CheckCompletion(bool _complete)
    {
        if (_complete)
        {
            Reset();
        }
        else
        {
            readPos -= 4;
        }
    }

    public void Reset()
    {
        buffer.Clear();
        readableBuffer = null;
        readPos = 0;
    }

    public void InsertInt(int _data)
    {
        buffer.InsertRange(0, BitConverter.GetBytes(_data));
    }

    #endregion

    #region Write Data

    public void Write(byte[] _data)
    {
        buffer.AddRange(_data);
    }

    public void Write(int _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }
    public void Write(uint _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }

    public void Write(float _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }

    public void Write(string _data)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(_data);

        Write(bytes.Length);

        buffer.AddRange(bytes);
    }

    public void Write(bool _data)
    {
        buffer.AddRange(BitConverter.GetBytes(_data));
    }

    public void Write(Vector3 _data)
    {
        Write(_data.x);
        Write(_data.y);
        Write(_data.z);
    }
    public void Write(Quaternion _data)
    {
        Write(_data.x);
        Write(_data.y);
        Write(_data.z);
        Write(_data.w);
    }

    #endregion

    #region Read Data

    public byte[] ReadBytes(int _length)
    {
        if (buffer.Count > readPos)
        {
            byte[] _data = buffer.GetRange(readPos, _length).ToArray();

            readPos += _length;

            return _data;
        }
        else
        {
            throw new Exception("Can't Read - byte[]");
        }
    }

    public int ReadInt()
    {
        if (buffer.Count >= readPos + 4)
        {
            int _data = BitConverter.ToInt32(readableBuffer, readPos);

            readPos += 4;

            return _data;
        }
        else
        {
            throw new Exception("Can't Read - int");
        }
    }

    public uint ReadUInt()
    {
        if (buffer.Count >= readPos + 4)
        {
            uint _data = BitConverter.ToUInt32(readableBuffer, readPos);

            readPos += 4;

            return _data;
        }
        else
        {
            throw new Exception("Can't Read - uint");
        }
    }

    public float ReadFloat()
    {
        if (buffer.Count >= readPos + 4)
        {
            float _data = BitConverter.ToSingle(readableBuffer, readPos);

            readPos += 4;

            return _data;
        }
        else
        {
            throw new Exception("Can't Read - float");
        }
    }

    public string ReadString()
    {
        try
        {
            int _length = ReadInt();

            string _data = Encoding.UTF8.GetString(readableBuffer, readPos, _length);

            readPos += _length;

            return _data;
        }
        catch
        {
            throw new Exception("Can't Read - string");
        }
    }

    public bool ReadBool()
    {
        if (buffer.Count > readPos)
        {
            bool _data = BitConverter.ToBoolean(readableBuffer, readPos);

            readPos += 1;

            return _data;
        }
        else
        {
            throw new Exception("Can't Read - bool");
        }
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }
    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    #endregion


    protected virtual void Dispose(bool _disposing)
    {
        if (!disposed)
        {
            if (_disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPos = 0;
            }

            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
