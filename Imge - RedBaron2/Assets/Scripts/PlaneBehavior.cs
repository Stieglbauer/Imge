﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour
{
    private float x = 0, y = 0;
    [SerializeField]
    private float agilityX, agilityY;
    [SerializeField]
    private float speed;
    private float maxSpeed;
    private bool shooting = false;
    [SerializeField]
    private float maxCooldown;
    public int health;
    private int maxHealth;
    private bool crash = false;
    private bool lockOnTarget = false;
    private float heat = 0;
    private bool overheated = false;
    public void setLock(bool val)
    {
        this.lockOnTarget = val;
    }
    public bool getLock()
    {
        return this.lockOnTarget;
    }
    public float getMaxCooldown()
    {
        return this.maxCooldown;
    }
    private float cooldown = 0;
    public float getCooldown()
    {
        return this.cooldown;
    }
    private float diveSound = 7;
    private float crashRot = 0;
    private int mg = 1;
    private GameObject coneFlash;
    private GameObject hull;
    private GameObject sound;
    private GameObject projectile;
    private GameObject referencez;

    private GameObject diveMid;
    private GameObject diveBegin;
    private int counter;
    private float up = 0;
    private float left = 0;

    // Start is called before the first frame update
    void Start()
    {
        referencez = GameObject.Find("PrefabReferences");
        if (maxCooldown == 0)
        {
            this.maxCooldown = referencez.GetComponent<PrefabRefs>().getGMCooldown();
        }
        if (speed == 0)
        {
            //this.speed = referencez.GetComponent<PrefabRefs>().getSpeed();
        }
        if(maxSpeed == 0)
        {
            this.maxSpeed = this.speed;
        }
        if (health == 0)
        {
            this.health = referencez.GetComponent<PrefabRefs>().getGeneralHealth();
        }
        if (agilityX == 0) { 
            //this.agilityX = 30f;
        }
        if(agilityY == 0)
        {
            //this.agilityY = 30f;
        }
        maxHealth = health;
        coneFlash = referencez.GetComponent<PrefabRefs>().getConeFlash();
        hull = referencez.GetComponent<PrefabRefs>().getHull();
        sound = referencez.GetComponent<PrefabRefs>().getMGShotSound();
        projectile = referencez.GetComponent<PrefabRefs>().getProjectile();
        diveBegin = referencez.GetComponent<PrefabRefs>().getDiveBegin();
        diveMid = referencez.GetComponent<PrefabRefs>().getDiveMid();
    }

    // Update is called once per frame
    void Update()
    {
        //safe steering directions
        up = y * agilityY;
        left = x * agilityX;

        //handle steering
        if (crash)
        {
            // to stop vibrating after crash
            counter++;
            if(counter >= 1000)
            {
                Vibration.Cancel();
            }
            if (diveSound != 0.0f) {
                diveSound -= Time.deltaTime;
                if(diveSound < 0.0f)
                {
                    Instantiate(diveMid, this.gameObject.transform);
                    diveSound = 0.0f;
                }
            }
            /*if(this.gameObject.transform.rotation.z > -this.gameObject.transform.rotation.w)
            {
                y = 1;
            } else
            {
                y = -1;
            }*/
            //Quaternion rot = Quaternion.LookRotation(Vector3.down, this.gameObject.transform.rotation * Vector3.up /*Vector3.left*/ /*Vector3.forward*/); //this.gameObject.transform.rotation, new Quaternion(0, 0, -0.7f, 0.7f), 0.005f);
            //this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, new Quaternion(rot.y, rot.z, rot.x, rot.w), 0.1f);
            this.gameObject.transform.rotation  = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.LookRotation(Vector3.down, this.gameObject.transform.rotation * Vector3.forward), 0.005f);
                
                y = 0;
            if(crashRot < 400 / agilityX)
                crashRot += 0.03f;
            x = crashRot;
        }
        this.gameObject.transform.Rotate(new Vector3(y * agilityY * Time.deltaTime, 0, -x * agilityX * Time.deltaTime), Space.Self);

        x = 0;
        y = 0;




        //move Plane forward
        this.gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);


        //handle shooting
        //shooting = this.gameObject.GetComponent<PlayerBehavior>().getShooting();
        cooldown+=Random.Range(0.9f,1.1f) * Time.deltaTime * 80;
        if (shooting)
        {
            if(cooldown >= maxCooldown && !overheated)
            {
                heat += 0.05f;
                if (heat > 2)
                {
                    overheated = true;
                }

                cooldown %= maxCooldown;
                mg *= -1;
                GameObject coneFlashInstance = Instantiate(coneFlash, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, coneFlash.transform.position.y, coneFlash.transform.position.z), this.gameObject.transform.rotation,  this.gameObject.transform);
                coneFlashInstance.transform.Translate(coneFlash.transform.position.x * mg, 0, 0, Space.Self);
                //coneFlashInstance.transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                //coneFlashInstance.transform.rotation = this.gameObject.transform.rotation;
                //GameObject hullInstance = Instantiate(hull, this.gameObject.transform.position + new Vector3(hull.transform.position.x, hull.transform.position.y, hull.transform.position.z * mg), this.gameObject.transform.rotation);
                GameObject hullInstance = Instantiate(hull, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, hull.transform.position.y, hull.transform.position.z), this.gameObject.transform.rotation);
                hullInstance.transform.Translate(new Vector3(hull.transform.position.x * mg, 0, 0));
                //hullInstance.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
                hullInstance.GetComponent<fallDown>().SetSideSpeed(this.gameObject.transform.rotation * (Vector3.right * mg * 8 + Vector3.forward * speed));
                Instantiate(sound, this.gameObject.transform);
                GameObject projectileInstance = Instantiate(projectile, this.gameObject.transform.position + this.gameObject.transform.rotation * new Vector3(0, projectile.transform.position.y, projectile.transform.position.z), this.gameObject.transform.rotation);
                projectileInstance.transform.Translate(projectile.transform.position.x * mg, 0, 0, Space.Self);
                if (this.gameObject.GetComponent<PlayerBehavior>() != null)
                {
                    projectileInstance.transform.Rotate(new Vector3(90, 0, 0), Space.Self);
                }
                else
                {
                    float bulletSpread = 1;
                    float randa = Random.Range(-bulletSpread, bulletSpread), randb = Random.Range(-bulletSpread, bulletSpread), randc = Random.Range(-bulletSpread, bulletSpread);
                    projectileInstance.transform.Rotate(new Vector3(90 + randa, randb, randc), Space.Self);
                }
                //projectileInstance.transform.Translate(0, 0, projectile.transform.position.z * mg, Space.Self);
            }
        }

        if(cooldown > maxCooldown)
        {
            if(heat > 0)
                heat -= 0.01f;
            if (heat < 0.5f)
                overheated = false;
        }

        if(this.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            this.gameObject.GetComponent<PlayerBehavior>().setGlowing(heat);
        }
    }

    public void setSpeedX(float val)
    {
        if (!crash)
        {
            if (val > 1) val = 1;
            if (val < -1) val = -1;
            x = val;
        }
    }

    public void setSpeedY(float val)
    {
        if (!crash)
        {
            if (val > 1) val = 1;
            if (val < -1) val = -1;
            y = val;
        }
    }

    public void setShooting(bool val)
    {
        this.shooting = val;
    }

    public void reduceHealth()
    {   
        // Hit-Vibration
        if(this.gameObject.GetComponent<PlayerBehavior>() != null)
        {
            Vibration.Vibrate(400);
        }
        health--;
        if(health == maxHealth/2)
        {
            Instantiate(referencez.GetComponent<PrefabRefs>().getSmoke(), this.gameObject.transform);
        }
        if(health <= 0 && !crash)
        {
            // Crash-Vibration
            if(this.gameObject.GetComponent<PlayerBehavior>() != null)
            {
                long[] pattern = new long[] { 0, 400, 400, 400, 400};
                Vibration.Vibrate(pattern, 1);
                counter = 0;
            } else { 
                this.gameObject.AddComponent<DestroyYourself>();
                this.gameObject.GetComponent<DestroyYourself>().setTime(20);
            }
            Instantiate(diveBegin, this.gameObject.transform);
            crash = true;
        }
    }

    public void setForwardV(float v)
    {

            if (v >= (0.5f * maxSpeed) && v <= maxSpeed)
            {
                if (Mathf.Abs(this.speed - v) < 0.5 || this.gameObject.GetComponent<PlayerBehavior>() != null)
                    this.speed = v;
                else if (speed - v > 0)
                    speed -= 0.5f;
                else
                    speed += 0.5f;
            }
        
    }

    public float getForwardV()
    {
        return this.speed;
    }

    public int getHealth()
    {
        return this.health;
    }

    public float getMaxForwardV()
    {
        return this.maxSpeed;
    }

    public float getAgilityX()
    {
        return this.agilityX;
    }

    public float getAgilityY()
    {
        return this.agilityY;
    }

    public float getLeft()
    {
        return this.left;
    }

    public float getUp()
    {
        return this.up;
    }
}
