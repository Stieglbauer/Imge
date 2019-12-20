using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
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

        public bool getActive()
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
    [SerializeField]
    private Texture good;
    private Fighter[] fighters;
    public GameObject testDummy1, testDummy2, testDummy3;
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
        for(int i=1; i<fighters.Length; i++)
        {
            if (!fighters[i].getActive()) continue;
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
            gaindistance(plane);
        }
        else if (stance == Stance.SEMITAILORING)
        {
            semitailoring(plane);
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

    private void semitailoring(Fighter plane)
    {

    }

    private void avoidCollision(Fighter plane)
    {

    }

    private void gaindistance(Fighter plane)
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
        if (realDif >= 90)
        {
            inverse = 1;
            if (probeDif > 90)
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * (realDif - 180) / (tolerance  * maxSpeedX));
            }
            else
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * (realDif - 180) / (tolerance * maxSpeedX));
            }
        }
        else
        {
            inverse = -1;
            if (probeDif > 90)
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(-factor * realDif / (tolerance * maxSpeedX));
            }
            else
            {
                plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedX(factor * realDif / (tolerance * maxSpeedX));
            }
        }
        if (inAim > Mathf.Min(new float[] { 90 - realDif, realDif }))
            plane.getIdentity().GetComponent<PlaneBehavior>().setSpeedY(inverse * inAim / (tolerance * maxSpeedX));

        
        return inAim;

    }

    private void tailoring(Fighter plane)
    {
        Vector3 tail = plane.getTarget().transform.position + 60 * (plane.getTarget().transform.rotation * Vector3.back);
        if (Vector3.Distance(plane.getIdentity().transform.position, tail) > 20)
        {
            plane.getIdentity().GetComponent<PlaneBehavior>().setForwardV(plane.getIdentity().GetComponent<PlaneBehavior>().getMaxForwardV());
            flyTowards(tail, plane, 2);
        } else
        {
            float inAim = flyTowards(plane.getTarget().transform.position, plane, 10);
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
