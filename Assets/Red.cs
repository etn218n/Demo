using System.Collections;
using UnityEngine;

public class Red : Bird
{
    private void Start()
    {
        this.StateChanged += SetAnimation;
        Idle(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnSlingshot();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ReadyToFly();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Fly();
        }
    }

    public override void Idle()        { State = BirdState.Idle;        }
    public override void OnSlingshot() { State = BirdState.OnSlingshot; }
    public override void ReadyToFly()  { State = BirdState.Ready;       }

    public override void Fly()
    {
        State = BirdState.Flying;
        StartCoroutine(WhileFlying());
    }

    private IEnumerator WhileFlying()
    {
        float maxWaitTime = 4f;
        float waitTime    = 0f;

        while (true)
        {
            waitTime += Time.deltaTime;

            if (waitTime > maxWaitTime)          { State = BirdState.Disappearing; break; }
            if (rb2d.velocity.magnitude < 0.01f) { State = BirdState.Disappearing; break; }

            yield return null;
        }

        // wait for Disappear Animation to complete
        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

    private void SetAnimation() { anim.SetInteger("StateID", (int)State); }
}
