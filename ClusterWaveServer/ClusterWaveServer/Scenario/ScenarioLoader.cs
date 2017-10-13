using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ClusterWaveServer.Scenario
{
    delegate void OnScenarioLoaderPacket(byte[] data);

    /// <summary>Used to load scenarios, check if they are valid, check basic properties and if you want to, create a Scenario</summary>
    class ScenarioLoader : IDisposable
    {
        private float _width, _height;
        private byte _v1, _v2, _bgType;
        private int _pcount;
        private String _name;
        private Vector2 powerupPos;
        private FileStream file;
        private bool _ok;

        public String Name { get { return _name; } }
        public String FileName { get { return file.Name; } }
        public int PlayerCount { get { return _pcount; } }
        public float Width { get { return _width; } }
        public float Height { get { return _height; } }
        public byte BackgroundType { get { return _bgType; } }
        /// <summary>Returns wether the reading was a success or has failed (true = success, false = fail)</summary>
        public bool IsOk { get { return _ok; } }

        public ScenarioLoader(String fileName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    this.file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch
                {
                    _ok = false;
                    return;
                }
                _ok = _Start();
            }
            else
                _ok = false;
        }

        private bool _Start()
        {
            //222 111 41 231 60
            if (file.Length < 30)
                return false;

            if (file.ReadByte() == 222 && file.ReadByte() == 111 && file.ReadByte() == 41 && file.ReadByte() == 231 && file.ReadByte() == 60)
            {
                _v1 = (byte)file.ReadByte();
                _v2 = (byte)file.ReadByte();

                char[] nameChars = new char[file.ReadByte()];
                if (file.Length - file.Position < nameChars.Length + 18)
                    return false;

                byte[] str1data = new byte[18+nameChars.Length];
                file.Read(str1data, 0, str1data.Length);
                ByteStream str1 = new ByteStream(str1data);
                for (int i = 0; i < nameChars.Length; i++)
                    nameChars[i] = (char)str1.ReadByte();
                _name = new String(nameChars);
                _bgType = str1.ReadByte();
                _width = str1.ReadFloat();
                _height = str1.ReadFloat();
                powerupPos = str1.ReadVector2();
                _pcount = str1.ReadByte();
                return (file.Length - file.Position) >= _pcount;
            }
            return false;
        }

        /// <summary>Creates a scenario from the information on the file. If creation fails, returns null.</summary>
        public Scenario CreateScenario()
        {
            if (_ok)
            {
                byte[] strdata = new byte[file.Length - file.Position];
                file.Read(strdata, 0, strdata.Length);
                file.Close();
                file.Dispose();
                ByteStream stream = new ByteStream(strdata);

                Vector2[] players = new Vector2[_pcount];
                for (int i = 0; i < players.Length; i++)
                    players[i] = stream.ReadVector2();

                Scenario sc = new Scenario(_width, _height, _bgType, powerupPos, players, _name);

                List<ShapeData> shapeDatas = new List<ShapeData>(100);

                while (stream.HasNext())
                {
                    switch (stream.ReadByte())
                    {
                        case 0:
                            #region Linegroup
                            if (stream.HasNextVector2Array())
                            {
                                Vector2[] arr = stream.ReadVector2Array();
                                sc.AddLinegroup(arr);
                                shapeDatas.Add(new ShapeData(0, arr));
                            }
                            else
                            {
                                _ok = false;
                                return null;
                            }
                            #endregion
                            break;

                        case 1:
                            #region Polygon
                            if (stream.HasNextVector2Array())
                            {
                                Vector2[] arr = stream.ReadVector2Array();
                                sc.AddPolygon(arr);
                                shapeDatas.Add(new ShapeData(1, arr));
                            }
                            else
                            {
                                _ok = false;
                                return null;
                            }
                            #endregion
                            break;

                        case 2:
                            #region Rectangle
                            if (stream.HasNext(16))
                            {
                                Vector2 a = stream.ReadVector2(), b = stream.ReadVector2();
                                sc.AddRectangle(a, b);
                                shapeDatas.Add(new ShapeData(a, b));
                            }
                            else
                            {
                                _ok = false;
                                return null;
                            }
                            #endregion
                            break;

                        default:
                            _ok = false;
                            return null;
                    }
                }

                if (OnCreationPacket != null)
                {
                    #region SendData
                    byte[] b = new byte[14];
                    b[0] = 3; // "this is scenario data"
                    b[1] = _bgType;
                    byte[] get = BitConverter.GetBytes(_width);
                    b[2] = get[0];
                    b[3] = get[1];
                    b[4] = get[2];
                    b[5] = get[3];

                    get = BitConverter.GetBytes(_height);
                    b[6] = get[0];
                    b[7] = get[1];
                    b[8] = get[2];
                    b[9] = get[3];

                    get = BitConverter.GetBytes(shapeDatas.Count);
                    b[10] = get[0];
                    b[11] = get[1];
                    b[12] = get[2];
                    b[13] = get[3];

                    OnCreationPacket(b);
                    #endregion

                    #region SendName
                    b = new byte[_name.Length + 2];
                    b[0] = 4; // "this is a name"
                    b[1] = (byte)_name.Length;
                    for (int i = 2; i < b.Length; i++)
                        b[i] = (byte)_name[i - 2];
                    OnCreationPacket(b);
                    #endregion

                    #region SendShapes
                    for (int i = 0; i < shapeDatas.Count; i++)
                    {
                        ShapeData d = shapeDatas[i];
                        if (d.type == 2)
                        {
                            #region Rectangle
                            b = new byte[17];
                            b[0] = 2;
                            int index = 1;
                            for (int c = 0; c < 2; c++)
                            {
                                get = BitConverter.GetBytes(d.data[c].X);
                                b[index++] = get[0];
                                b[index++] = get[1];
                                b[index++] = get[2];
                                b[index++] = get[3];

                                get = BitConverter.GetBytes(d.data[c].Y);
                                b[index++] = get[0];
                                b[index++] = get[1];
                                b[index++] = get[2];
                                b[index++] = get[3];
                            }
                            #endregion
                        }
                        else
                        {
                            #region Linegroup or Polygon
                            b = new byte[d.data.Length * 8 + 5];
                            //1 byte for type, 4 for length(int), 8 bytes for each Vector2
                            b[0] = d.type;
                            get = BitConverter.GetBytes(d.data.Length);
                            b[1] = get[0];
                            b[2] = get[1];
                            b[3] = get[2];
                            b[4] = get[3];

                            int index = 5;
                            for (int c = 0; c < d.data.Length; c++)
                            {
                                get = BitConverter.GetBytes(d.data[c].X);
                                b[index++] = get[0];
                                b[index++] = get[1];
                                b[index++] = get[2];
                                b[index++] = get[3];

                                get = BitConverter.GetBytes(d.data[c].Y);
                                b[index++] = get[0];
                                b[index++] = get[1];
                                b[index++] = get[2];
                                b[index++] = get[3];
                            }
                            #endregion
                        }
                        OnCreationPacket(b);
                    }
                    #endregion

                    #region SendEnd
                    b = new byte[1];
                    b[0] = 254;
                    OnCreationPacket(b);
                    #endregion
                }

                return sc;
            }
            return null;
        }

        /// <summary>Disposes the FileStream (call this once you're done loading a map or checking it's info)</summary>
        public void Dispose()
        {
            file.Dispose();
        }

        /// <summary>This event is called during CreateScenario() to send the packages needed for the client to create the scenario. Won't be called if IsOk is false</summary>
        public event OnScenarioLoaderPacket OnCreationPacket;
    }

    struct ShapeData
    {
        public byte type;
        public Vector2[] data;

        /// <summary>for rectangles</summary>
        public ShapeData(Vector2 from, Vector2 to)
        {
            type = 2;
            data = new Vector2[2] { from, to };
        }

        public ShapeData(byte type, Vector2[] data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
