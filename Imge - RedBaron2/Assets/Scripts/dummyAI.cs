using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummyAI : MonoBehaviour
{
    private Fighter[] fighters;
    public GameObject t;
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

    private struct Fighter
    {
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

    void Start()
    {
        //need2rework
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Plane");
        fighters = new Fighter[arr.Length + 1];
        fighters[0] = new Fighter(GameObject.Find("Player"), true);
        for (int i = 0; i < arr.Length; i++)
        {
            fighters[i + 1] = new Fighter(arr[i], false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        t = fighters[1].getTarget();
        for (int i = 1; i < fighters.Length; i++)
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
        else if (stance == Stance.AVOIDCOLLISION)
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
        float inAim = Quaternion.Angle(direction, Quaternion.LookRotation(plane.getIdentity().transform.rotation * Vector3.forward, Vector3.forward));

        Quaternion testProbe = plane.getIdentity().transform.rotation * Quaternion.AngleAxis(90, Vector3.forward);
        Quaternion desiredRotation = Quaternion.LookRotation(plane.getIdentity().transform.rotation * Vector3.forward, direction * Vector3.forward);


        float realDif = Quaternion.Angle(plane.getIdentity().transform.rotation, desiredRotation);
        float probeDif = Quaternion.Angle(testProbe, desiredRotation);
        int inverse = 1;
        int factor = -1;
        float maxSpeedX = plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityX();
        float maxSpeedY = plane.getIdentity().GetComponent<PlaneBehavior>().getAgilityY();
        if (realDif > 90)
        {
            inverse = 1;
            if (probeDif > 90)
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * 20);
            }
            else
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * 20);
            }
        }
        else
        {
            inverse = -1;
            if (probeDif > 90)
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * 20);
            }
            else
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * 20);
            }
        }
        plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedY(inverse * 20 * Mathf.Min(1, Mathf.Max(0, 45 - Mathf.Min(realDif, 180 - realDif))/45));

        return inAim;

    }

    private bool targetInDeadSpot(Fighter plane)
    {
        /*float y = plane.getIdentity().GetComponent<PlaneBehavior>().getUp();
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
        testDummy1.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float angle = getDeadSpotAngle(x, y);
        targetPos = Quaternion.EulerAngles(0, (x * y > 0 ? 1 : -1) * (0.5f*Mathf.PI - angle), 0) * targetPos;
        //Debug.Log(angle * 180 / Mathf.PI);
        testDummy2.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        float radius = getDeadSpotRadius(x, y, plane.getIdentity().GetComponent<PlaneBehavior>().getForwardV(), angle);
        targetPos += new Vector3(0, (y > 0 ? -1 : 1) * radius, 0);
        testDummy3.transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
        return targetPos.x * targetPos.x + targetPos.y * targetPos.y <= radius * radius * 1.2f;*/


        float y = 80;
        float x = 80;
        Vector3 targetPos = new Vector3(plane.getTarget().transform.position.x, plane.getTarget().transform.position.y, plane.getTarget().transform.position.z);
        targetPos -= plane.getIdentity().transform.position;
        targetPos = Quaternion.Inverse(plane.getIdentity().transform.rotation) * targetPos;
        float angle = getDeadSpotAngle(x, y);
        targetPos = Quaternion.EulerAngles(0, (x * y > 0 ? 1 : -1) * (0.5f * Mathf.PI - angle), 0) * targetPos;
        float radius = getDeadSpotRadius(x, y, 60, angle);
        targetPos += new Vector3(0, (y > 0 ? -1 : 1) * radius, 0);
        return targetPos.x * targetPos.x + targetPos.y * targetPos.y <= radius * radius * 1.2f;
    }

    private void tailoring(Fighter plane)
    {
        flyTowards(plane.getTarget().transform.position, plane, 1);
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
        for (int i = 0; i < fighters.Length; i++)
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
        return 360.0f / Mathf.Sqrt(speedX * speedX + speedY * speedY) * speedForward * Mathf.Cos(deadSpotAngle) / (2 * Mathf.PI);
    }

    public float getDeadSpotAngle(float speedX, float speedY)
    {
        //in radians
        //returns 0 if plane is performing looping
        return Mathf.Min(Vector3.Angle(new Vector3(speedX, speedY, 0), new Vector3(0, 1, 0)), Vector3.Angle(new Vector3(speedX, speedY, 0), new Vector3(0, -1, 0))) / 180 * Mathf.PI;
    }
}
