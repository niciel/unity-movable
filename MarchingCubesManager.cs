using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChunkSystem
{
    public class MarchingCubesManager 
    {

        public Mesh GenerateMesh(Vector3 position,  BlockData[,,] blocks)
        {

            Mesh mesh = new Mesh();

            List<Vector3> vertices;
            int xSize, ySize, zSize;
            xSize = blocks.GetLength(0)-1;
            ySize = blocks.GetLength(1)-1;
            zSize = blocks.GetLength(2)-1;
            BlockData[] data = new BlockData[8];

            for (int x = 0 ; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    for (int z = 0; z<zSize;z++)
                    {
                        
                    }
                }
            }


            return mesh;
        }



        #region generation

        public static readonly Vector3[] CornersPosition = new Vector3[] {
                new Vector3(1,-1,1),
                new Vector3(1,-1,-1),
                new Vector3(-1,-1,-1),
                new Vector3(-1,-1,1),

                new Vector3(1,1,1),
                new Vector3(1,1,-1),
                new Vector3(-1,1,-1),
                new Vector3(-1,1,1)
            };


        byte[] tab = new byte[] {
        0x1,0x2,0x4,0x8,
            0x10,0x20,0x40,0x80};


        private MerchBlockVertex[] final = new MerchBlockVertex[256];
        HashSet<byte> kurwa = new HashSet<byte>(new byte[] { 145, 251, 129, 219 });


        //public float[] corners = new float[8];

        private HashSet<byte> set = new HashSet<byte>();

        public void generateDataArray()
        {
            //grid = new float[elementCount, elementCount, elementCount];

            //List<data> list = new List<data>();
            set.Clear();

            data nd;
            int[] order;
            Vector3[] poits;
            Vector3[] corners;

            // o brak
            nd = new data()
            {
                corners = new Vector3[0],
                points = new Vector3[0],
                order = new int[0]
            };
            dodajZInversja(nd);

            //1
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , -1 , 0),
                new Vector3(0 , -1 , -1)
            };
            order = new int[]
            {
                0,1,2
            };
            corners = new Vector3[] { getCorner(2) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);

            //2
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(1 , 0 , -1),
                new Vector3(1 , -1 , 0),
                new Vector3(-1 , -1 , 0)
            };
            order = new int[]
            {
                0,3,1,1,3,2
            };
            corners = new Vector3[] { getCorner(1), getCorner(2) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);


            //3
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , -1 , 0),
                new Vector3(0 , -1 , -1),
                new Vector3(0 , 1 , -1),
                new Vector3(1 ,  0, -1),
                new Vector3(1 , 1 , 0)
            };
            order = new int[]
            {
                0,1,2,
                3,4,5
            };
            corners = new Vector3[] { getCorner(2), getCorner(5) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            //dodajZInversja(nd);
            dodaj8scianrotacja360(nd);


            //4
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , -1 , 0),
                new Vector3(0 , -1 , -1),
                new Vector3(0,1 , 1),
                new Vector3(1 , 1 , 0),
                new Vector3(1 , 0 , 1)
            };
            order = new int[]
            {
                0,1,2,3,4,5
            };
            corners = new Vector3[] { getCorner(2), getCorner(4) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);


            //5
            poits = new Vector3[] {
                new Vector3(-1 , 0 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , 0 , -1),
                new Vector3(0 , -1 , -1),
                new Vector3(-1 , -1 ,0)
            };
            order = new int[]
            {
                0,1,2,0,2,3,0,3,4
            };
            corners = new Vector3[] { getCorner(0), getCorner(1), getCorner(3) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);



            //6
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(1 , 0 , -1),
                new Vector3(1 , -1 , 0),
                new Vector3(-1 , -1 , 0),
                new Vector3(0,1 , 1),
                new Vector3(1 , 1 , 0),
                new Vector3(1 , 0 , 1)
            };
            order = new int[]
            {
                0,3,1,1,3,2,4,5,6

            };
            corners = new Vector3[] { getCorner(1), getCorner(2), getCorner(4) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodaj8scianrotacja360(nd);


            //7
            poits = new Vector3[] {
                new Vector3(0,1 , 1),
                new Vector3(1 , 1 , 0),
                new Vector3( 1 , 0 , 1),
                new Vector3(1 , 0 , -1),
                new Vector3(1 , -1 , 0),
                new Vector3(0,-1 , -1),
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , 1 , 0),
                new Vector3(0 , 1 , -1)
            };
            order = new int[]
            {
                0,1,2,3,5,4,6,8,7

            };
            corners = new Vector3[] { getCorner(1), getCorner(4), getCorner(6) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);

            //8
            poits = new Vector3[] {
                new Vector3(-1 , 0 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , 0 , -1),
                new Vector3(-1 , 0 , -1)
            };
            order = new int[]
            {
                0,1,3,3,1,2

            };
            corners = new Vector3[] { getCorner(0), getCorner(1), getCorner(2), getCorner(3) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);



            //9
            poits = new Vector3[] {
                new Vector3(-1, 1, 0),
                new Vector3(0 , 1 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , -1 , 0),
                new Vector3(0 , -1 , -1),
                new Vector3(-1 , 0 , -1)
            };
            order = new int[]
            {
                0,1,5,5,1,2,2,4,5,2,3,4

            };
            corners = new Vector3[] { getCorner(0), getCorner(2), getCorner(3), getCorner(7) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);



            //10
            poits = new Vector3[] {
                new Vector3(-1 , 1 ,0),
                new Vector3(0,1,-1),
                new Vector3(0,-1 ,-1),
                new Vector3(-1 , -1,0),

                new Vector3(0,1 , 1),
                new Vector3(1 , 1 , 0),
                new Vector3(1 , -1 , 0),
                new Vector3(0,-1 , 1)
            };
            order = new int[]
            {
                0,3,2,2,1,0,
                4,5,6,6,7,4

            };
            corners = new Vector3[] { getCorner(0), getCorner(2), getCorner(4), getCorner(6) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);


            //11
            poits = new Vector3[] {
                new Vector3(1 , 1 , 0),
                new Vector3(1 , -1 , 0),
                new Vector3(0 , -1 ,-1),
                new Vector3(-1 , 0 ,-1),
                new Vector3(-1 , 0 , 1),
                new Vector3(0 ,1 , 1)
            };
            order = new int[]
            {
                0,4,5,0,2,4,0,1,2,4,2,3

            };
            corners = new Vector3[] { getCorner(0), getCorner(2), getCorner(3), getCorner(4) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);

            //12
            poits = new Vector3[] {
                new Vector3(-1 , 1 , 0),
                new Vector3(0 , 1 , -1),
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , 0 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , 0 , -1),
                new Vector3(0 , - 1 , -1),
                new Vector3(-1 , - 1 , 0)
            };
            order = new int[]
            {
                0,2,1,
                3,4,5,3,5,7,7,5,6
            };
            corners = new Vector3[] { getCorner(0), getCorner(1), getCorner(3), getCorner(6) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodaj8scianrotacja360(nd);

            //13
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , -1 , 0),
                new Vector3(0 , -1 , -1),
                new Vector3(0 , 1 , -1),
                new Vector3(1 ,  0, -1),
                new Vector3(1 , 1 , 0),

                new Vector3(-1 , 1, 0),
                new Vector3(0 , 1 , 1),
                new Vector3(-1 , 0 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , -1 , 0),
                new Vector3(0 , -1 , 1),
            };
            order = new int[]
            {
                0,1,2,3,4,5,
                6,7,8,9,10,11
            };
            corners = new Vector3[] { getCorner(0), getCorner(2), getCorner(5), getCorner(7) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);

            //14
            poits = new Vector3[] {
                new Vector3(-1 , 1 ,0),
                new Vector3(0 , 1 , 1),
                new Vector3(1 , 0 , 1),
                new Vector3(1 , 0 , -1),
                new Vector3(0 , -1 , -1),
                new Vector3(-1 , -1 , 0)
            };
            order = new int[]
            {
                0,1,2,
                2,3,4,
                4,0,2,
                0,4,5
            };
            corners = new Vector3[] { getCorner(0), getCorner(1), getCorner(3), getCorner(7) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodajZInversja(nd);


            //inversja 2 (chyba dzila)
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(0,1 , -1),
                new Vector3(1 , 1 , 0),
                new Vector3(1 , 0 , -1),
                new Vector3(0 , -1 , -1),
                new Vector3(-1 , -1 ,0)
            };
            order = new int[]
            {
                0,1,5,5,1,2,
                5,2,3,3,4,5
            };
            corners = new Vector3[] { getCorner(0), getCorner(1), getCorner(3), getCorner(4), getCorner(6), getCorner(7) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodaj8scianrotacja360(nd);

            //inversja 6 
            poits = new Vector3[] {
                new Vector3(-1 , 0 , -1),
                new Vector3(-1 , 0 , 1),
                new Vector3(0 , 1 ,1),
                new Vector3(0 , 1 , -1),
                new Vector3(1 , 0 , -1),
                new Vector3(1 , -1 , 0),
                new Vector3(0,-1 , -1)
            };
            order = new int[]
            {
                0,1,6,6,1,2,2,4,6,2,3,4,4,5,6


            };
            corners = new Vector3[] { getCorner(0), getCorner(2), getCorner(3), getCorner(4), getCorner(5) };
            nd = new data()
            {
                points = poits,
                order = order,
                corners = corners
            };
            dodaj8scianrotacja360(nd);

            Debug.Log("setSize: " + set.Count);
        }

        private void dodajZInversja(data d)
        {
            data data = d;
            dodajporotacji(data);


            data = rotuj(d, Vector3.right, 90);
            dodajporotacji(data);
            data = rotuj(d, Vector3.right, 180);
            dodajporotacji(data);
            data = rotuj(d, Vector3.right, 270);
            dodajporotacji(data);


            data = rotuj(d, Vector3.forward, 90);
            dodajporotacji(data);
            data = rotuj(d, Vector3.forward, 180);
            dodajporotacji(data);
            data = rotuj(d, Vector3.forward, 270);
            dodajporotacji(data);
        }
        private static Vector3 getCorner(int i)
        {
            return CornersPosition[i];
        }

        private void dodajporotacji(data d, Vector3 rotationAxis, params float[] angle)
        {
            data ndata;
            foreach (float f in angle)
            {
                ndata = rotuj(d, rotationAxis, f);
                dodaj(ndata);
            }
        }

        private void dodajporotacji(data dat)
        {
            data d;
            for (int i = 0; i < 4; i++)
            {
                d = rotuj(dat, Vector3.up, 90 * i);
                dodaj(d);
            }
            //Debug.Log("przed: " + dat.points.Length + " corners: " + dat.corners.Length);
            dat = invers(dat);
            //Debug.Log("po   : " + dat.points.Length + " corners: " + dat.corners.Length);

            for (int i = 0; i < 4; i++)
            {
                d = rotuj(dat, Vector3.up, 90 * i);
                dodaj(d);
            }
        }
        private data invers(data d)
        {
            data ret = new data();
            HashSet<Vector3> contains = new HashSet<Vector3>(d.corners);
            List<Vector3> ncorners = new List<Vector3>();
            foreach (Vector3 cp in CornersPosition)
            {
                if (contains.Contains(cp))
                    continue;
                else
                    ncorners.Add(cp);
            }
            int[] noreder = new int[d.order.Length];
            for (int i = 0; i < noreder.Length; i++)
            {
                int a = i / 3;
                int b = i % 3;
                int id = a * 3 + (2 - b);
                noreder[i] = d.order[id];
            }
            Vector3[] npoints = new Vector3[d.points.Length];
            for (int i = 0; i < d.points.Length; i++)
                npoints[i] = d.points[i];

            ret.corners = ncorners.ToArray();
            ret.order = noreder;
            ret.points = npoints;
            return ret;
        }

        private byte setBit(int pos, byte value)
        {
            return (byte)(value | tab[pos]);
        }
        private void dodaj(data d)
        {
            byte b = 0x0;
            for (int j = 0; j < d.corners.Length; j++)
            {
                b = setBit(getConrnerID(d.corners[j]), b);
            }
            if (set.Contains(b))
            {
                //Debug.LogWarning("istnieje :D! " + b);
                return;
            }
            else if (kurwa.Contains(b))
            {
                string s = "info: " + b + "corners ";
                foreach (Vector3 v in d.corners)
                    s += ", " + getConrnerID(v);
                Debug.Log(s);
            }
            set.Add(b);
            int[] ore = new int[d.order.Length];
            Vector3[] p = new Vector3[d.points.Length];
            for (int i = 0; i < d.order.Length; i++)
                ore[i] = d.order[i];
            for (int i = 0; i < d.points.Length; i++)
                p[i] = d.points[i];

            //final[b] = new MerchBlockVertex(p, ore);
        }


        private static int getConrnerID(Vector3 vec)
        {
            for (int i = 0; i < CornersPosition.Length; i++)
            {
                if (CornersPosition[i] == vec)
                {

                    return i;
                }
            }
            return -1;
        }

        private void dodaj8scianrotacja360(data nd)
        {
            data ndn;
            dodajporotacji(nd, Vector3.up, 0, 90, 180, 270);
            ndn = rotuj(nd, Vector3.right, 180);
            dodajporotacji(ndn, Vector3.up, 0, 90, 180, 270);

            ndn = rotuj(nd, Vector3.right, 90);
            dodajporotacji(ndn, Vector3.forward, 0, 90, 180, 270);
            ndn = rotuj(nd, Vector3.right, -90);
            dodajporotacji(ndn, Vector3.forward, 0, 90, 180, 270);

            ndn = rotuj(nd, Vector3.forward, 90);
            dodajporotacji(ndn, Vector3.right, 0, 90, 180, 270);
            ndn = rotuj(nd, Vector3.forward, -90);
            dodajporotacji(ndn, Vector3.right, 0, 90, 180, 270);
        }
        private data rotuj(data data, Vector3 rotationAxis, float angle)
        {
            data a = data;
            Vector3[] ncorner = new Vector3[data.corners.Length];
            Vector3[] npoints = new Vector3[data.points.Length];

            Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);
            for (int i = 0; i < data.corners.Length; i++)
            {
                ncorner[i] = (rotation * data.corners[i]).Round(3);
            }
            for (int i = 0; i < data.points.Length; i++)
            {
                npoints[i] = (rotation * data.points[i]).Round(3);
            }
            a.corners = ncorner;
            a.points = npoints;
            int[] ord = new int[data.order.Length];
            for (int i = 0; i < ord.Length; i++)
                ord[i] = data.order[i];
            a.order = ord;
            return a;
        }




        struct data
        {
            public Vector3[] points;
            public int[] order;
            public Vector3[] corners;
        }


        #endregion
    }
}
