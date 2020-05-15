using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class W_EnemyBoid_AI : MonoBehaviour
{
    public GameObject player;
    public float maxBoidSpeed = 18;
    public float maxCenterSpeed = 5;
    public Vector2 centerLimits = new Vector2(5, 3);
    public float forwardSpeed = 20;
    public int maxBoids = 5;
    private List<GameObject> boidList;
    public GameObject boidProto;
    public CinemachineDollyCart dolly;
    [Space]
    [Header("Flocking Parameters")]
    public float alignStr = 1;
    public float cohesStr = 1;
    public float sepStr = 1;
    public float randomnessStr = 0.5f;
    public float flockRad = 10;
    public float boidRad = 2;



    private Vector3 flockPos;
    private Vector3 avgDir;

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        boidList = new List<GameObject>();
        //spawn maxboids # of boids and put them in the list
        for (int i = 0; i < maxBoids; i++)
        {
            Vector3 spawnPos = boidProto.transform.position; //new Vector3(Random.Range(-3.5f, 3.5f), Random.Range(-3.5f, 3.5f), 0);
            Vector3 spawnSpeed = Random.insideUnitCircle * maxBoidSpeed;
            GameObject currBoid = Object.Instantiate(boidProto, spawnPos, Quaternion.identity, transform);
            currBoid.GetComponent<W_SingleBoid_AI>().setVelocity(spawnSpeed);
            boidList.Add(currBoid);
            
        }
        dolly.m_Speed = forwardSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posTotal = Vector3.zero;
        Vector3 dirTotal = Vector3.zero;
        for (int i = 0; i < boidList.Count; i++)
        {
            posTotal += boidList[i].transform.localPosition;
            dirTotal += boidList[i].GetComponent<W_SingleBoid_AI>().getVelocity();
        }
        flockPos = posTotal / boidList.Count;
        avgDir = dirTotal / boidList.Count;

        //move average position to target player
        Vector3 targetPos = player.transform.position;
        targetPos = transform.InverseTransformPoint(targetPos);
        targetPos = targetPos - flockPos;
        flockPos += new Vector3(targetPos.x, targetPos.y, 0) * maxCenterSpeed * Time.deltaTime;
        if (flockPos.x > centerLimits.x)
        {
            flockPos.x = centerLimits.x;
        }
        else if (flockPos.x < -centerLimits.x)
        {
            flockPos.x = -centerLimits.x;
        }

        if (flockPos.y > centerLimits.y)
        {
            flockPos.y = centerLimits.y;
        }
        else if (flockPos.y < -centerLimits.y)
        {
            flockPos.y = -centerLimits.y;
        }

        Vector3 alignVect;
       
        alignVect = calcAlignmentVect();


        for (int i = 0; i < boidList.Count; i++)
        {
            GameObject currBoid = boidList[i];
            Vector3 randomVect;
            Vector3 cohesVect;
            Vector3 sepVect;

            randomVect = Random.insideUnitCircle * maxBoidSpeed;
            randomVect *= randomnessStr;

            cohesVect = calcCohesionVect(i);
            sepVect = calcSeparationVect(i);


            Vector3 accel = Vector3.zero;
            accel += alignVect;
            accel += cohesVect;
            accel += sepVect;
            accel += randomVect;

            accel *= (maxBoidSpeed * Time.deltaTime);

            Vector3 boidVelocity = currBoid.GetComponent<W_SingleBoid_AI>().getVelocity();
            boidVelocity += accel;
            if (boidVelocity.magnitude > maxBoidSpeed)
            {
                boidVelocity.Normalize();
                boidVelocity *= maxBoidSpeed;
            }

            currBoid.GetComponent<W_SingleBoid_AI>().setVelocity(boidVelocity);
            currBoid.GetComponent<W_SingleBoid_AI>().localMove(maxBoidSpeed);
        }

    }


    private Vector3 calcAlignmentVect()
    {
        Vector3 alignVec = avgDir / maxBoidSpeed;
        if (alignVec.magnitude < 1)
        {
            alignVec.Normalize();
        }
        alignVec *= alignStr;
        return alignVec;
    }

    private Vector3 calcCohesionVect(int boidIndex)
    {
        Vector3 cohVect = flockPos - boidList[boidIndex].transform.localPosition;
        float dist = cohVect.magnitude;
        cohVect.Normalize();

        if (dist < flockRad)
        {
            cohVect *= (dist / flockRad);
        }
        cohVect *= cohesStr;
        return cohVect;
    }

    private Vector3 calcSeparationVect(int boidIndex)
    {
        Vector3 sepVect = Vector3.zero;
        for (int i = 0; i < boidList.Count; i++)
        {
            if (boidIndex != i)
            {
                Vector3 betweenVect = boidList[boidIndex].transform.localPosition -
                    boidList[i].transform.localPosition;
                float dist = betweenVect.magnitude;
                if (dist < (boidRad * 2))
                {
                    betweenVect.Normalize();
                    betweenVect *= (((boidRad * 2) - dist) / (boidRad * 2));
                    sepVect += betweenVect;
                }
            }
        }

        if (sepVect.magnitude > 1)
        {
            sepVect.Normalize();
        }

        sepVect *= sepStr;
        return sepVect;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(flockPos, 0.5f);
    }
    // alignment 
    // cohesion
    // separation
}
