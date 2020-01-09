using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    private int count = 0;
    [SerializeField]
    private Texture good;
    private Fighter[] fighters;
    public GameObject testDummy1, testDummy2, testDummy3;
    private enum Stance
    {
        IDLE,
        TAILORING,
        ENGAGING,
        FORMATION,
        DODGING,
        SHAKING,
        GAINDISTANCE,
        SEMITAILORING,
        AVOIDCOLLISION
    };

    private struct Fighter {
        private GameObject identity;
        private Stance stance;
        private GameObject target;
        private bool nationality;
        private bool active;


        public Fighter(GameObject identity, bool nationality)
        {
            this.identity = identity;
            this.stance = Stance.IDLE;
            this.target = null;
            this.nationality = nationality;
            this.active = true;
        }

        public bool isActive()
        {
            return active;
        }
        public void setActive(bool val)
        {
            if (val == false) this.active = false;
        }
        public Stance getStance()
        {
            return this.stance;
        }
        public void setStance(Stance newStance)
        {
            this.stance = newStance;
        }
        public GameObject getIdentity()
        {
            return this.identity;
        }
        public GameObject getTarget()
        {
            return this.target;
        }
        public void setTarget(GameObject newTarget)
        {
            this.target = newTarget;
        }
    };
    // Start is called before the first frame update
    void Start()
    {
        //need2rework
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Plane");
        fighters = new Fighter[arr.Length+1];
        fighters[0] = new Fighter(GameObject.Find("Player"), true);
        for(int i=0; i<arr.Length; i++)
        {
            fighters[i+1] = new Fighter(arr[i], arr[i].GetComponent<Texture>().Equals(good));
        }

    }

    // Update is called once per frame
    void Update()
    {
        count++;
        /*if(count == 10)
        {

            Transform t = testDummy1.transform;
            for (int i = -10; i < 10; i++)
            {
                for (int j = -10; j < 10; j++)
                {
                    for (int k = -10; k < 10; k++)
                    {
                        t.position = new Vector3(i * 10, j * 10, k * 10) + fighters[0].getIdentity().transform.position;
                        GameObject o = Instantiate(testDummy1, t.position, t.rotation);
                        fighters[0].setTarget(o);
                        if (!targetInDeadSpot(fighters[0])) Destroy(o);
                        else
                        {
                            //o.AddComponent<DestroyYourself>().setTime(0.00001f);
                        }
                    }
                }
            }
        }*/
        for (int i=1; i<fighters.Length; i++)
        {
            if (!fighters[i].isActive()) continue;
            fighters[i].setTarget(findTarget(fighters[i]));
            fighters[i].getIdentity().GetComponent<PlaneBehavior>().setShooting(true);
            Stance stance = analyseSituation(fighters[i]);
            executeStance(stance, fighters[i]);
        }
    }

    private void executeStance(Stance stance, Fighter plane)
    {
        if (stance == Stance.TAILORING)
        {
            tailoring(plane);
        }
        else if (stance == Stance.SHAKING)
        {
            shaking(plane);
        }
        else if (stance == Stance.FORMATION)
        {
            formation(plane);
        }
        else if (stance == Stance.ENGAGING)
        {
            engaging(plane);
        }
        else if (stance == Stance.DODGING)
        {
            dodging(plane);
        }
        else if (stance == Stance.GAINDISTANCE)
        {
            gainDistance(plane);
        }
        else if (stance == Stance.SEMITAILORING)
        {
            semiTailoring(plane);
        }
        else if(stance == Stance.AVOIDCOLLISION)
        {
            avoidCollision(plane);
        }
        else
        {
            idle(plane);
        }
    }

    private void semiTailoring(Fighter plane)
    {

    }

    private void avoidCollision(Fighter plane)
    {

    }

    private void gainDistance(Fighter plane)
    {

    }

    private float flyTowards(Vector3 position, Fighter plane, float tolerance)
    {
        Quaternion direction = Quaternion.LookRotation(position - plane.getIdentity().transform.position, Vector3.forward);
        // direction = angle from the plane's position towards the target's position
        float inAim = Quaternion.Angle(direction, Quaternion.LookRotation(plane.getIdentity().transform.rotation * Vector3.forward, Vector3.forward));
        // inAim = difference (degrees) between the angle the plane is aiming at and the direction towards the target -> 0 <= inAim <= 180

        Quaternion testProbe = plane.getIdentity().transform.rotation * Quaternion.AngleAxis(90, Vector3.forward);
        Quaternion desiredRotation = Quaternion.LookRotation(plane.getIdentity().transform.rotation * Vector3.forward, direction * Vector3.forward);
        // desiredRotation = rotation of the plane from where y-movement leads to aiming at the target


        float realDif = Quaternion.Angle(plane.getIdentity().transform.rotation, desiredRotation);
        // realDif = difference (degrees) between current rotation and the desiredRotation -> 0 <= realDif <= 180
        float probeDif = Quaternion.Angle(testProbe, desiredRotation);
        int inverse = 1;
        int factor = -1;
        float maxSpeedX = plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityX();
        float maxSpeedY = plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityY();
        float steeringX = 0;
        if (realDif > 90)
        {
            inverse = 1;
            if (probeDif > 90)
            {
                //Debug.Log("L, >");
                //plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * (realDif - 180) / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX)));
                steeringX = -factor * (realDif -180) / ((tolerance * maxSpeedX) == 0? 1 : (tolerance * maxSpeedX));
            }
            else
            {
                //Debug.Log("L, <");
                //plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * (realDif - 180) / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX)));
                steeringX = factor * (realDif - 180) / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX));
            }
        }
        else
        {
            inverse = -1;
            if (probeDif > 90)
            {
                //Debug.Log("T, >");
                //plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * realDif / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX)));
                steeringX = -factor * realDif / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX));
            }
            else
            {
                //Debug.Log("T, <");
                //plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * realDif / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX)));
                steeringX = factor * realDif / ((tolerance * maxSpeedX) == 0 ? 1 : (tolerance * maxSpeedX));
            }
        }
        //plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedY(Mathf.Min(1, Mathf.Max(0, 80 - Mathf.Min(realDif, 180 - realDif)) / 90) * inverse * inAim / ((tolerance * maxSpeedY)==0?1: (tolerance * maxSpeedY)));
        float steeringY = Mathf.Min(1, Mathf.Max(-1, ((90 - Mathf.Min(realDif, 180 - realDif)) / 90) * inverse * inAim / ((tolerance * maxSpeedY) == 0 ? 1 : (tolerance * maxSpeedY))));

        Debug.Log(steeringY +
            " : " + targetInDeadSpot(plane, Mathf.Max(-1, Mathf.Min(1, steeringX))*plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityX(), Mathf.Max(-1, Mathf.Min(1, steeringY)) * plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityY()) +
            " : " + Mathf.Min(1, Mathf.Max(-1, steeringX)) * plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityX() +
            " : " + steeringY * plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityY() +
            " : " + plane.getIdentity().GetComponent<PlaneBehavior>().getForwardV() +
            " : " +getDeadSpotRadius(steeringX, steeringY, plane.getIdentity().GetComponent<PlaneBehavior>().getForwardV(), getDeadSpotAngle(steeringX, steeringY)));

        /*plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(steeringX);
        if(!targetInDeadSpot(plane, steeringX, steeringY))
        {
            plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedY(steeringY);
        }*/

        return inAim;

    }
    private bool targetInDeadSpot(Fighter plane, float x, float y)
    {
        if (y == 0)
        {
            Debug.Log("y == 0");
            return false;
        }
        Vector3 targetPos = new Vector3(plane.getTarget().transform.position.x, plane.getTarget().transform.position.y, plane.getTarget().transform.position.z);
        targetPos -= plane.getIdentity().transform.position;
        targetPos = Quaternion.Inverse(plane.getIdentity().transform.rotation) * targetPos;
        testDummy1.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float angle = getDeadSpotAngle(x, y);
        targetPos = Quaternion.EulerAngles(0, (x * y > 0 ? 1 : -1) * (0.5f * Mathf.PI - angle), 0) * targetPos;
        testDummy2.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float radius = getDeadSpotRadius(x, y, plane.getIdentity().GetComponent<PlaneBehavior>().getForwardV(), angle);
        targetPos += new Vector3(0, (y > 0 ? -1 : 1) * radius, 0);
        testDummy3.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        return targetPos.x * targetPos.x + targetPos.y * targetPos.y <= radius * radius * 1.2f;
    }

    private bool targetInDeadSpot(Fighter plane)
    {
        float y = plane.getIdentity().GetComponent<PlaneBehavior>().getUp();
        y = (y > 0 ? -1 : 1) * plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityY();
        float x = plane.getIdentity().GetComponent<PlaneBehavior>().getLeft();
        x = (x > 0 ? 1 : -1) * plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityX();
        if (y == 0)
        {
            //return false;
        }
        Vector3 targetPos = new Vector3(plane.getTarget().transform.position.x, plane.getTarget().transform.position.y, plane.getTarget().transform.position.z);
        targetPos -= plane.getIdentity().transform.position;
        targetPos = Quaternion.Inverse(plane.getIdentity().transform.rotation) * targetPos;
        float angle = getDeadSpotAngle(x, y);
        targetPos = Quaternion.EulerAngles(0, (x * y > 0 ? 1 : -1) * (0.5f*Mathf.PI - angle), 0) * targetPos;
        float radius = getDeadSpotRadius(x, y, plane.getIdentity().GetComponent<PlaneBehavior>().getForwardV(), angle);
        targetPos += new Vector3(0, (y > 0 ? -1 : 1) * radius, 0);
        return targetPos.x * targetPos.x + targetPos.y * targetPos.y <= radius * radius * 1.2f;


        /*float y = 80;
        float x = 80;
        Vector3 targetPos = new Vector3(plane.getTarget().transform.position.x, plane.getTarget().transform.position.y, plane.getTarget().transform.position.z);
        targetPos -= plane.getIdentity().transform.position;
        targetPos = Quaternion.Inverse(plane.getIdentity().transform.rotation) * targetPos;
        testDummy1.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float angle = getDeadSpotAngle(x, y);
        targetPos = Quaternion.EulerAngles(0, (x * y > 0 ? 1 : -1) * (0.5f * Mathf.PI - angle), 0) * targetPos;
        //Debug.Log(angle * 180 / Mathf.PI);
        testDummy2.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float radius = getDeadSpotRadius(x, y, 60, angle);
        targetPos += new Vector3(0, (y > 0 ? -1 : 1) * radius, 0);
        testDummy3.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        return targetPos.x * targetPos.x + targetPos.y * targetPos.y <= radius * radius * 1.2f;*/
    }

    private void tailoring(Fighter plane)
    {
        Vector3 tail = plane.getTarget().transform.position + 60 * (plane.getTarget().transform.rotation * Vector3.back);
        if (((Vector3.Distance(plane.getIdentity().transform.position, tail) > 20 && Vector3.Distance(plane.getIdentity().transform.position, plane.getTarget().transform.position) > 60) || Vector3.Distance(plane.getIdentity().transform.position, plane.getTarget().transform.position) < 40))
        {
            plane.getIdentity().GetComponent<PlaneBehavior>().setForwardV(plane.getIdentity().GetComponent<PlaneBehavior>().getMaxForwardV());
            flyTowards(tail, plane, 1);
        } else
        {
            float inAim = flyTowards(plane.getTarget().transform.position, plane, 1);
            if (inAim < 5) {
                plane.getIdentity().GetComponent<PlaneBehavior>().setForwardV(Mathf.Min(plane.getIdentity().GetComponent<PlaneBehavior>().getMaxForwardV(), Mathf.Max(plane.getIdentity().GetComponent<PlaneBehavior>().getMaxForwardV() / 2, plane.getTarget().GetComponent<PlaneBehavior>().getForwardV())));
            } else
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setForwardV(plane.getIdentity().GetComponent<PlaneBehavior>().getMaxForwardV() / 2);
            }
        }
    }

    private void shaking(Fighter plane)
    {
        plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(1);
        plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedY(0.2f);
    }

    private void formation(Fighter plane)
    {

    }

    private void dodging(Fighter plane)
    {

    }

    private void engaging(Fighter plane)
    {

    }

    private void idle(Fighter plane)
    {

    }

    private Stance analyseSituation(Fighter plane)
    {

        return Stance.TAILORING;
    }

    private GameObject findTarget(Fighter plane)
    {
        //need2rework
        return fighters[0].getIdentity();
    }

    public void setInactive(GameObject plane)
    {
        for(int i=0; i<fighters.Length; i++)
        {
            if (fighters[i].getIdentity() == plane)
            {
                fighters[i].setActive(false);
                return;
            }
        }
    }

    public float getDeadSpotRadius(float speedX, float speedY, float speedForward, float deadSpotAngle)
    {
        //deadSpotAngle has to be in radians!
        return 360.0f / Mathf.Sqrt(speedX * speedX + speedY * speedY) * speedForward * Mathf.Cos(deadSpotAngle) / (2*Mathf.PI);
    }

    public float getDeadSpotAngle(float speedX, float speedY)
    {
        //in radians
        //returns 0 if plane is performing looping
        return Mathf.Min(Vector3.Angle(new Vector3(speedX, speedY, 0), new Vector3(0, 1, 0)), Vector3.Angle(new Vector3(speedX, speedY, 0), new Vector3(0, -1, 0)))/180*Mathf.PI;
    }

    public void oldAI(GameObject plane)
        {
            //important part >>
            /*
            GameObject target = findTarget(false, 0);
            //Quaternion direction = Quaternion.FromToRotation(plane.transform.position, target.transform.position);
            Quaternion direction = Quaternion.LookRotation(target.transform.position - plane.transform.position, Vector3.forward);
            float inAim = Quaternion.Angle(direction, Quaternion.LookRotation(plane.transform.rotation * Vector3.forward, Vector3.forward));
            if (inAim < 10 && Vector3.Distance(plane.transform.position, target.transform.position) < 300)
            {
                //Debug.Log("Tally Ho! "+ Quaternion.Angle(direction, plane.transform.rotation));
                plane.GetComponent<PlaneBehavior>().setShooting(true);
            }
            else
            {
                //Debug.Log("got him! " + Vector3.Angle(direction*Vector3.forward, plane.transform.rotation * Vector3.forward));
                plane.GetComponent<PlaneBehavior>().setShooting(false);

            }


            Quaternion testProbe = plane.transform.rotation * Quaternion.AngleAxis(90, Vector3.forward);
            Quaternion desiredRotation = Quaternion.LookRotation(plane.transform.rotation * Vector3.forward, direction * Vector3.forward);
            float realDif = Quaternion.Angle(plane.transform.rotation, desiredRotation);
            float probeDif = Quaternion.Angle(testProbe, desiredRotation);
            testDummy1.transform.rotation = plane.transform.rotation;
            testDummy2.transform.rotation = desiredRotation;
            testDummy3.transform.rotation = testProbe;
            int inverse = 1;
            int factor = -1; //just for quick changes
            Debug.Log("Enemy Location " + realDif + "     " + probeDif);*/
            // <<important part
            //seperated part
            /*if (realDif > 0.1f && realDif < 179.9f)
            {*/
            //important part >>
            /*
            if (realDif >= 90)
            {
                inverse = 1;
                if (probeDif > 90)
                {
                    plane.GetComponent<PlaneBehavior>().setSpeedX(-factor * (realDif - 180) / 2);
                    Debug.Log("Enemy down right");
                }
                else
                {
                    plane.GetComponent<PlaneBehavior>().setSpeedX(factor * (realDif - 180) / 2);
                    Debug.Log("Enemy down left");
                }
            }
            else
            {
                inverse = -1;
                if (probeDif > 90)
                {
                    plane.GetComponent<PlaneBehavior>().setSpeedX(-factor * realDif / 2);
                    Debug.Log("Enemy up right");
                }
                else
                {
                    plane.GetComponent<PlaneBehavior>().setSpeedX(factor * realDif / 2);
                    Debug.Log("Enemy up left");
                }
            }*/
            //<< important part
            //seperated part
            /*}
            else
            {
            if (realDif >= 90)
                {
                    inverse = 1;
                } else
                {
                    inverse = -1;
                }*/


            /*if(realDif < 1 || realDif > 89)
            {
                plane.GetComponent<PlaneBehavior>().setLock(true);
            }
            if (plane.GetComponent<PlaneBehavior>().getLock())
            {
                plane.GetComponent<PlaneBehavior>().setSpeedY(inverse * inAim / 30);
            }*/
            //important part >>
            /*
            if (inAim > Mathf.Min(new float[] { 90 - realDif, realDif }))
                plane.GetComponent<PlaneBehavior>().setSpeedY(inverse * inAim / 1.0f);
            //seperated part
            //}
            Debug.Log("InAim: " + inAim);
            */
            //<< important part

    }
}
