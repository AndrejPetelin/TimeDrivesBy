using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class AN_SimpleCityGenerator : MonoBehaviour
{
    public int CityZoneCount = 200, BigSectorCount = 500, SqareLength = 20;
    public Vector3Int CityCentre;
    public AN_CitySample[] List1x1;
    public int List1x1Chance;
    public AN_CitySample[] List1x2;
    public int List1x2Chance;
    public AN_CitySample[] ListAngle;
    public int ListAngleChance;
    public AN_CitySample[] ListSqare;
    public int listSquareChance;

    int MapLength, CurrentBigSectorCount;
    int[,] IntMap;
    List<Vector2Int> Vacant;

    public void Generate() // use it in your games !
    {
        Vacant = new List<Vector2Int>();

        Debug.Log("Generating in game object : " + gameObject.name);
        MapLength = Mathf.FloorToInt(Mathf.Sqrt( CityZoneCount ) );
        IntMap = new int[MapLength, MapLength];
       // IntMap[MapLength / 2, MapLength / 2] = 1;

        Debug.Log("Length of sqare map : " + MapLength + ", city centre : " + MapLength / 2 + " : " + MapLength / 2);

        CurrentBigSectorCount = BigSectorCount;
        // CheckNeighbour(MapLength / 2, MapLength / 2);

        //  CalculateCityZone();
        // CalculateBigSectors();

        CalculateCity();
        BuildCity(); // StartCoroutine( BuildCity() );
    }

    public void CalculateCityZone() // array IntMap is a city territory
    {
        for (int i = 0; i < CityZoneCount; i++)
        {
            if (Vacant.Count <= 0) break;
            Vector2Int Pos = Vacant[Random.Range(0, Vacant.Count-1)];
           // Debug.Log("POS: " + Pos);
            Vacant.Remove(Pos);

            IntMap[Pos.x, Pos.y] = 1;
            CheckNeighbour(Pos.x, Pos.y);
        }
        Debug.Log("City territory calculated");
    }

    public void CalculateCity()
    {
        int[] weights = { List1x1Chance, List1x2Chance, ListAngleChance, listSquareChance };
        int totalWeights = 0;
        foreach (int x in weights) totalWeights += x;
        
        for (int i = 0; i < MapLength; ++i)
        {
            for (int j = 0; j < MapLength; ++j)
            {
            
                int x = Random.Range(0, totalWeights);
                Debug.Log("LENGTH: " + weights.Length);
                for (int k = 0; k < weights.Length; ++k)
                {
                    if (x < weights[k])
                    {
                        Debug.Log("IS IT SMALLER? ");
                        x = k;
                        break;
                    }
                    x -= weights[k];
                }
                if (IntMap[i, j] != 0) continue;
                Debug.Log("X: " + x);
                switch (x)
                {
                    case (3):
                        {
                            if (InsertSqare(new Vector2Int(i,j)))
                                break;
                            else goto case 2;
                        }
                    case (2):
                        {
                            if (InsertAngle(new Vector2Int(i, j)))
                                break;
                            else goto case 1;
                        }
                    case (1):
                        {
                            if (InsertLong1x2(new Vector2Int(i, j)))
                                break;
                            else goto case 0;
                        }
                    case (0):
                        {
                            if (IntMap[i, j] == 0 ) IntMap[i, j] = 1;
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }

  /*  public void CalculateBigSectors()
    {
        // scaning city zone
        for (int x = 0; x < IntMap.GetLength(0); x++)
        {
            for (int y = 0; y < IntMap.GetLength(1); y++)
            {
                if (IntMap[x, y] == 1) Vacant.Add(new Vector2Int(x, y));
            }
        }

        // random instantiate big sectors in array
        int limit = CityZoneCount;
        while (CurrentBigSectorCount > 0 && limit > 0)
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case (0):
                    {
                        if (InsertSqare())
                            break;
                        else goto case 1;
                    }
                case (1):
                    {
                        if (InsertAngle())
                            break;
                        else goto case 2;
                    }
                case (2):
                    {
                        InsertLong1x2();
                        break;
                    }
                default:
                    break;
            }
            limit--;
            if (limit < 1) Debug.Log("Big Sectors Limit achieved");
        }
        Debug.Log("Big sectors calculated");
    }*/

    #region InsertBigSectorsVoids

    public bool InsertLong1x2(Vector2Int Pos)
    {
        int k = Random.Range(0, Vacant.Count - 1);
       // Vector2Int Pos = Vacant[k];
        bool rotate = Random.Range(0, 2) == 1; // 90 degrees if true
        if (IntMap[Pos.x, Pos.y] == 0 && Pos.x + 1 < MapLength && IntMap[Pos.x + 1, Pos.y] == 0 && !rotate)
        
        {
           // if (Pos.x + 1 >= MapLength) return false;
            IntMap[Pos.x, Pos.y] = 21;
            IntMap[Pos.x + 1, Pos.y] = 9;
           /* Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x + 1, Pos.y));*/
            CurrentBigSectorCount--;
            return true;
        }
        else if (IntMap[Pos.x, Pos.y] == 0 && Pos.y + 1< MapLength && IntMap[Pos.x, Pos.y + 1] == 0 && rotate)
        {
           // if (Pos.y + 1 >= MapLength) return false;
            IntMap[Pos.x, Pos.y] = 22;
            IntMap[Pos.x, Pos.y + 1] = 9;
          /*  Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x, Pos.y + 1));*/
            CurrentBigSectorCount--;
            return true;
        }
        return false;
    }

    public bool InsertAngle(Vector2Int Pos)
    {
        int k = Random.Range(0, Vacant.Count - 1);
       // Vector2Int Pos = Vacant[k];
        bool rotate = Random.Range(0, 2) == 1; // -90 degrees if true
        if (IntMap[Pos.x, Pos.y] == 0 && Pos.x +1 < MapLength && Pos.y +1 < MapLength &&
            IntMap[Pos.x + 1, Pos.y] == 0 && IntMap[Pos.x, Pos.y + 1] == 0 && !rotate)
        {
            //if (Pos.x + 1 >= MapLength || Pos.y + 1 >= MapLength) return false;
            IntMap[Pos.x, Pos.y] = 31;
            IntMap[Pos.x + 1, Pos.y] = 9;
            IntMap[Pos.x, Pos.y + 1] = 9;
          /*  Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x + 1, Pos.y));
            Vacant.Remove(new Vector2Int(Pos.x, Pos.y + 1));*/
            CurrentBigSectorCount--;
            return true;
        }
        else if (IntMap[Pos.x, Pos.y] == 0 && Pos.x -1 >= 0 && Pos.y +1 < MapLength &&
            IntMap[Pos.x, Pos.y + 1] == 0 && IntMap[Pos.x - 1, Pos.y] == 1 && rotate)
        {
           // if (Pos.x - 1 < 0 || Pos.y + 1 >= MapLength) return false;
            IntMap[Pos.x, Pos.y] = 32;
            IntMap[Pos.x, Pos.y + 1] = 9;
            IntMap[Pos.x - 1, Pos.y] = 9;
          /*  Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x, Pos.y + 1));
            Vacant.Remove(new Vector2Int(Pos.x - 1, Pos.y));*/
            CurrentBigSectorCount--;
            return true;
        }
        return false;
    }

    public bool InsertSqare(Vector2Int Pos)
    {
        int k = Random.Range(0, Vacant.Count - 1);
       // Vector2Int Pos = Vacant[k];
        bool rotate = Random.Range(0, 2) == 1; // -90 degrees if true
        if (IntMap[Pos.x, Pos.y] == 0 && Pos.x + 1 < MapLength && Pos.y +1 < MapLength &&
            IntMap[Pos.x + 1, Pos.y] == 0 && IntMap[Pos.x, Pos.y + 1] ==0 && IntMap[Pos.x + 1, Pos.y + 1] == 0 && !rotate)
        {
            IntMap[Pos.x, Pos.y] = 41;
            IntMap[Pos.x + 1, Pos.y] = 9;
            IntMap[Pos.x, Pos.y + 1] = 9;
            IntMap[Pos.x + 1, Pos.y + 1] = 9;
          /*  Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x + 1, Pos.y));
            Vacant.Remove(new Vector2Int(Pos.x, Pos.y + 1));
            Vacant.Remove(new Vector2Int(Pos.x + 1, Pos.y + 1));*/
            CurrentBigSectorCount--;
            return true;
        }
        else if (IntMap[Pos.x, Pos.y] == 0 && Pos.x -1 >= 0 && Pos.y -1 >= 0 &&
            IntMap[Pos.x - 1, Pos.y] == 0 && IntMap[Pos.x, Pos.y - 1] == 0 && IntMap[Pos.x - 1, Pos.y - 1] == 0 && rotate)
        {
            IntMap[Pos.x, Pos.y] = 42;
            IntMap[Pos.x - 1, Pos.y] =9;
            IntMap[Pos.x, Pos.y - 1] = 9;
            IntMap[Pos.x - 1, Pos.y - 1] = 9;
           /* Vacant.Remove(Pos);
            Vacant.Remove(new Vector2Int(Pos.x, Pos.y - 1));
            Vacant.Remove(new Vector2Int(Pos.x - 1, Pos.y));
            Vacant.Remove(new Vector2Int(Pos.x - 1, Pos.y - 1));*/
            CurrentBigSectorCount--;
            return true;
        }
        return false;
    }
    #endregion

    public void BuildCity() // instantiate finished city version // IEnumerator BuildCity()
    {
        for (int x = 0; x < IntMap.GetLength(0) ; x++)
        {
            for (int y = 0; y < IntMap.GetLength(1) ; y++)
            {
                Debug.Log("AT: " + x + ", " + y + ": " + IntMap[x, y]);
                int selected;
                switch (IntMap[x, y])
                {
                    case (1): // 1x1
                        {
                            selected = SelectPrefab(List1x1);
                            Instantiate( List1x1[selected].gameObject, new Vector3Int(x , 0, y ) * SqareLength + CityCentre, Quaternion.Euler(0, 90 * Random.Range(0,4), 0) );
                            break;
                        }
                    case (21): // 1x2
                        {
                            selected = SelectPrefab(List1x2);
                            Instantiate(List1x2[selected].gameObject, 
                                new Vector3Int(x * SqareLength + SqareLength - SqareLength/2, 0, y  * SqareLength)  + CityCentre, 
                                Quaternion.Euler(0, 0, 0));
                            break;
                        }
                    case (22): // 1x2 rotate -90
                        {
                            selected = SelectPrefab(List1x2);
                            Instantiate(List1x2[selected].gameObject, 
                                new Vector3Int(x  * SqareLength , 0, y  * SqareLength + SqareLength / 2)  + CityCentre,
                                Quaternion.Euler(0, -90, 0));
                            break;
                        }
                    case (31): // angle
                        {
                            selected = SelectPrefab(ListAngle);
                            Instantiate(ListAngle[selected].gameObject,
                                new Vector3Int(x * SqareLength + SqareLength / 2, 0, y * SqareLength + SqareLength / 2) + CityCentre,
                                Quaternion.Euler(0, 0, 0));
                            break;
                        }
                    case (32): // angle rotate -90
                        {
                            selected = SelectPrefab(ListAngle);
                            Instantiate(ListAngle[selected].gameObject,
                                new Vector3Int(x * SqareLength , 0, y * SqareLength + SqareLength / 2) + CityCentre,
                                Quaternion.Euler(0, -90, 0));
                            break;
                        }
                    case (41): // sqare
                        {
                            selected = SelectPrefab(ListSqare);
                            Instantiate(ListSqare[selected].gameObject,
                                new Vector3Int(x  * SqareLength + SqareLength / 2, 0, y  * SqareLength + SqareLength / 2) + CityCentre,
                                Quaternion.Euler(0, 0, 0));
                            break;
                        }
                    case (42): // sqare shift
                        {
                            selected = SelectPrefab(ListSqare);
                            Instantiate(ListSqare[selected].gameObject, 
                                new Vector3Int(x  * SqareLength + SqareLength /2 , 0, y  * SqareLength + SqareLength /2)   + CityCentre,
                                Quaternion.Euler(0, 180, 0));
                            break;
                        }
                    default:
                        break;
                }
            }
            // yield return new WaitForSeconds(.001f);
        }
        Debug.Log("City was built");
    }

    public void CheckNeighbour(int x, int y)
    {
        if (y + 1 < MapLength && IntMap[x, y + 1] == 0) Vacant.Add(new Vector2Int(x, y + 1)); else Vacant.Remove(new Vector2Int(x, y + 1));
        if (y -1 > 0 && IntMap[x, y - 1] == 0) Vacant.Add(new Vector2Int(x, y - 1)); else Vacant.Remove(new Vector2Int(x, y - 1));
        if (x +1 < MapLength && IntMap[x + 1, y] == 0) Vacant.Add(new Vector2Int(x + 1, y)); else Vacant.Remove(new Vector2Int(x + 1, y));
        if (x -1 > 0 && IntMap[x - 1, y] == 0) Vacant.Add(new Vector2Int(x - 1, y)); else Vacant.Remove(new Vector2Int(x - 1, y));
    }

    public int SelectPrefab(AN_CitySample[] List)
    {
        int VeritySumm = 0;
        for (int k = 0; k < List.Length; k++)
        {
            VeritySumm += List[k].Verity;
        }

        int CheckSumm = 0, i = 0;
        int IntRandom = Random.Range(1, VeritySumm);
        while (CheckSumm < IntRandom && i < List.Length)
        {
            CheckSumm += List[i].Verity;
            i++;
        }
        i--;
        return i;
    
    }
}