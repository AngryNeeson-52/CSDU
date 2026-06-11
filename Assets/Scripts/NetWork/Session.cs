using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Session
{
    public static Session instance { get; private set; }
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 8080;
    public int id = 0;
    public uint snapShot;

    public TCP tcp;
    public UDP udp;

    public Session()
    {
        instance = this;

        tcp = new TCP();
        udp = new UDP();

        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        NetworkStream stream;
        Packet receivedData;
        byte[] receivedBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receivedBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallBack, socket);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"{_ex} - TCP sending Error");
            }
        }

        void ConnectCallBack(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
                return;

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receivedBuffer, 0, dataBufferSize, ReceiveCallBack, null);
        }

        void ReceiveCallBack(IAsyncResult _result)
        {
            try
            {
                int datalength = stream.EndRead(_result);

                if (datalength <= 0)
                {
                    instance.Disconnect();
                    return;
                }


                byte[] data = new byte[datalength];
                Array.Copy(receivedBuffer, data, datalength);

                receivedData.CheckCompletion(HandleData(data));

                stream.BeginRead(receivedBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"{_ex} - TCP receiving");
                instance.Disconnect();
            }
        }

        public bool HandleData(byte[] _data)
        {
            receivedData.SetBytes(_data);

            int packetLength = 0;
            if (receivedData.UnReadLength() >= 4)
            {
                packetLength = receivedData.ReadInt();

                if (packetLength <= 0)
                {
                    return true;
                }
            }

            while (packetLength > 0 && packetLength <= receivedData.UnReadLength())
            {
                byte[] packetBytes = receivedData.ReadBytes(packetLength);

                ServerUpdater.instance.TaskRequest.Enqueue(() =>
                {
                    using (Packet newPacket = new Packet(packetBytes))
                    {
                        int packetId = newPacket.ReadInt();
                        PacketDispatcher.packetHandlers[packetId](newPacket);
                    }
                });

                packetLength = 0;
                if (receivedData.UnReadLength() >= 4)
                {
                    packetLength = receivedData.ReadInt();

                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (packetLength <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Disconnect()
        {
            socket.Close();
            stream = null;
            receivedData = null;
            receivedBuffer = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        UdpClient udpClient;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _port)
        {
            udpClient = new UdpClient(_port);

            udpClient.Connect(endPoint);
            udpClient.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.id);

                udpClient.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"{_ex} - UDP sending Error");
            }
        }

        void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] data = udpClient.EndReceive(_result, ref endPoint);
                udpClient.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4)
                {
                    return;
                }

                HandleData(data);
            }
            catch
            {
                Debug.Log($"UDP Receiving Error");
            }
        }

        public void HandleData(byte[] _data)
        {
            using (Packet packet = new Packet(_data))
            {
                int length = packet.ReadInt();

                if (length <= 0 || length > packet.UnReadLength())
                    return;

                byte[] packetBytes = packet.ReadBytes(length);


                ServerUpdater.instance.TaskRequest.Enqueue(() =>
                {
                    using (Packet newPacket = new Packet(packetBytes))
                    {
                        int packetId = newPacket.ReadInt();
                        PacketDispatcher.packetHandlers[packetId](newPacket);
                    }
                });
            }
        }

        public void Disconnect()
        {
            endPoint = null;
            udpClient = null;
        }
    }


    public void Disconnect()
    {
        Debug.Log($"Disconnected");

        tcp.Disconnect();
        udp.Disconnect();
    }

}
