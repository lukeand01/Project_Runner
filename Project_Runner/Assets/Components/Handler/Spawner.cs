using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Timeline;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Separator("CONTAINER")]
    [SerializeField] Transform spawnContainer;

    [Separator("OBSTACLE")]
    [SerializeField] GameObject template;
    [SerializeField] float obstacleRecoverSpawn;
    [SerializeField] float obstaclecurrentSpawn;
    [SerializeField] float obstacletotalSpawn;

    [Separator("COMET")]
    [SerializeField] float cometRecoverSpawn;
    [SerializeField] float cometCurrentSpawn;
    [SerializeField] float cometTotalSpawn;
    [SerializeField] Transform cometOriginalPos;
    [SerializeField] GameObject cometTemplate;

    [Separator("POWER")]
    [SerializeField] GameObject powerTemplate;

    bool spawnStarted;
    int phase;

    public void ChangePhase(int phase)
    {
        if(phase > this.phase)
        {
            this.phase = phase;
        }       
    }

    public void StartSpawn()
    {
        spawnStarted = true;
        obstacleRecoverSpawn = 0;
        cometRecoverSpawn = 0;
        phase = 0;
        powerSpawnChanceModifier = 0;
    }
    public void StopSpawn()
    {
        spawnStarted = false;
    }
   
    private void Update()
    {
        
        if (!spawnStarted) return;

        HandleObstacle();
        if(phase > 0) HandleComet();
        
    }

    public void KillAll()
    {
        for (int i = 0; i < spawnContainer.childCount; i++)
        {
            Destroy(spawnContainer.GetChild(i).gameObject);
        }
    }

    #region OBSTACLEE

    void HandleObstacle()
    {
        obstacleRecoverSpawn += Time.deltaTime * 0.01f;
        obstacleRecoverSpawn = Mathf.Clamp(obstacleRecoverSpawn, 0, obstacletotalSpawn / 2);

        if (obstacletotalSpawn > obstaclecurrentSpawn)
        {
            obstaclecurrentSpawn += Time.deltaTime;
        }
        else
        {
            Spawn();
            obstaclecurrentSpawn = obstacleRecoverSpawn;

        }
    }

    List<int> GetPatternList()
    {
        //two typese of patteern: random and collums.
        //we need ot know the number of obstacles beforehand which is supposed to be based on the meters running.
        List<int> newList = new();
        int spawnQuantity = Random.Range(2,5);
        if (phase == 3) spawnQuantity += 1;

        bool isRandom = false;
        int firstRoll = Random.Range(0, 10);
        
        if (firstRoll > 7) isRandom = true;
        
        //also i want to mayb spawn a buff here.
        if (isRandom) newList = CreateRandom(GetRandomNumber(), spawnQuantity);
        else newList = CreateCollum(GetRandomNumber(), spawnQuantity);
   
        return newList;
    }
   

    List<int> CreateCollum2(int firstNumber, int spawnQuantity)
    {
        List<int> newList = new();
        int currentSpawnQuantity = spawnQuantity;
        int currentNumber = firstNumber;

        //there can be at most one buff. 

        newList.Add(firstNumber);
        currentSpawnQuantity--;
        while(currentSpawnQuantity > 0)
        {




        }



        return newList;
    }


    int GetRandomNumberInList(List<int> list)
    {
        int number = -1;

        int brake = 0;

        while(number == -1)
        {
            int random = Random.Range(-4, 5);
            brake++;
            if(brake > 10000)
            {
                Debug.Log("broke off");
                break;
            }

            foreach (var item in list)
            {
                if(item == random)
                {
                    return random;
                }
            }

        }

        Debug.LogError("SOMETHING WRONG");
        return -1;

    }

    List<int> CreateCollum(int firstNumber, int spawnQuantity)
    {
        List<int> newList = new();
        int currentSpawnQuantity = spawnQuantity;
        int currentNumber = firstNumber;

        newList.Add(firstNumber);
        currentSpawnQuantity--;

        while(currentSpawnQuantity > 0)
        {
            
            if (currentNumber == 4)
            {
                int low = GetLowestNumber(currentNumber, newList) - 2;
                newList.Add(low);
                currentNumber = low; 
                currentSpawnQuantity--;
                continue;
            }

            if(currentNumber == -4)
            {
                int high = GetHighestNumber(currentNumber, newList) + 2;
                newList.Add(high);
                currentNumber = high;
                currentSpawnQuantity--;
                continue;
            }


            int dir = Random.Range(0, 2);

            if(dir == 0)
            {
                currentNumber += 2;
                newList.Add(currentNumber);
                currentSpawnQuantity--;
                continue;
            }
            if(dir == 1)
            {
                currentNumber -= 2;
                newList.Add(currentNumber);
                currentSpawnQuantity--;
                continue;
            }


            if (dir == 2) Debug.LogError("Somthing wrong");

        }

        if(newList.Count > 4)
        {
            //then we remove a random fella.
            int random = Random.Range(0, newList.Count - 1);
            newList.RemoveAt(random);
            Debug.Log("force removee");
        }

        return newList;
    }
    List<int> CreateRandom(int firstNumber, int spawnQuantity)
    {
        List<int> newList = new();
        int currentSpawnQuantity = spawnQuantity;

        while (currentSpawnQuantity > 0)
        {
            int random = GetRandomNumber();

            if(!HasThisNumber(random, newList))
            {
                //then we add this numbr
                newList.Add(random);
                currentSpawnQuantity--;
            }

        }

        return newList;
    }

    float powerSpawnChanceModifier = 0;
    float powerSpawnChanceBase = 90;
    void Spawn()
    {
        List<int> patternLists = GetPatternList();

        float powerChance = Random.Range(0, 100);
        bool shouldSpawnPower = false;
        if (powerChance > powerSpawnChanceBase - powerSpawnChanceModifier)
        {
            powerSpawnChanceModifier = 0;
            shouldSpawnPower = true;
            Debug.Log("rolled for power");
        }
        else
        {
            Debug.Log("no power");
            powerSpawnChanceModifier = 5;
        }

        
        foreach (var item in patternLists)
        {
            if (shouldSpawnPower)
            {
                GameObject powerObject = Instantiate(powerTemplate, transform.position + new Vector3(0, item, 0), Quaternion.identity);
                powerObject.SetActive(true);
                powerObject.transform.parent = spawnContainer;
                shouldSpawnPower = false;
            }
            else
            {
                GameObject newObject = Instantiate(template, transform.position + new Vector3(0, item, 0), Quaternion.identity);
                newObject.SetActive(true);
                newObject.name = Random.Range(0, 1000).ToString();
                newObject.transform.parent = spawnContainer;
            }
            
        }

    }
    #endregion

    #region COMETS
    void HandleComet()
    {

        cometRecoverSpawn += Time.deltaTime * 0.01f;
        cometRecoverSpawn = Mathf.Clamp(cometRecoverSpawn, 0, cometTotalSpawn/ 2);

        if (cometTotalSpawn > cometCurrentSpawn)
        {
            cometCurrentSpawn += Time.deltaTime;
        }
        else
        {
            StartCoroutine(CometProcess());
            cometCurrentSpawn = cometRecoverSpawn;

        }
    }

    IEnumerator CometProcess()
    {
        int random = Random.Range(0, 3);
        int pos = 0;
        if (random == 1) pos = 2;

        if (random == 2) pos = -2;

        UIHolder.instance.cometWarner.CometWarn(pos);

        yield return new WaitForSeconds(0.6f);

        SpawnComet(pos);

    }

    void SpawnComet(int pos)
    {
        //get one of threee positions randomly and let it fall down.
        
       GameObject newObject = Instantiate(cometTemplate, cometOriginalPos.position + new Vector3(pos, 0, 0), Quaternion.identity);
        newObject.SetActive(true);
        newObject.transform.parent = spawnContainer;
    }



    #endregion

    #region UTILS
    int GetHighestNumber(int currentNumber, List<int> list)
    {
        int highestNumber = currentNumber;
        foreach (var item in list)
        {
            if (item > highestNumber) highestNumber = item;
        }
        return highestNumber;
    }
    int GetLowestNumber(int currentNumber, List<int> list)
    {
        int lowestNumber = 0;
        foreach (var item in list)
        {
            if (item < lowestNumber) lowestNumber = item;
        }

        return lowestNumber;
    }

    bool HasThisNumber(int number, List<int> list)
    {
        foreach (var item in list)
        {
            if (item == number) return true;
        }
        return false;
    }
    int GetRandomNumber()
    {
        int random = Random.Range(0, 5);

        if (random == 1) return 2;
        if (random == 2) return 4;
        if (random == 3) return -2;
        if (random == 4) return -4;

        return 0;
    }
    #endregion

    
}
