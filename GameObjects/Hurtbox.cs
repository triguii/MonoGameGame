using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotGame.GameObjects;

public class Hurtbox : Entity
{
    //Class to represent the area where an entity can be damaged. (Hurtbox is not the correct name, should be hitbox)
    public bool isEnemyHurtbox;
    public float damageAmount;
    public float damageKnockback;

    //Rectangle constructor
    public Hurtbox(RectangleF boundingRectangle, bool isEnemyHurtbox, float damageAmount, float damageKnockback = 40f)
    {
        Bounds = boundingRectangle;
        this.isEnemyHurtbox = isEnemyHurtbox;
        this.damageAmount = damageAmount;
        this.damageKnockback = damageKnockback;
    }

    //Ellipse constructor
    public Hurtbox(CircleF boundingCircle, bool isEnemyHurtbox, float damageAmount, float damageKnockback = 40f)
    {
        Bounds = boundingCircle;
        this.isEnemyHurtbox = isEnemyHurtbox;
        this.damageAmount = damageAmount;
        this.damageKnockback = damageKnockback;

    }

}

