using UnityEngine;

public class PuzzleItem : MonoBehaviour
{
    protected bool isSolved;
    protected ParticleSystem yippee;
    public ParticleSystem prefab;

    protected virtual void Start()
    {
        CheckParticles();
    }

    public virtual void Solve()
    {
        isSolved = true;
        yippee.Play();
        Prize();
    }

    public virtual void Prize()
    {
        print("Wow you beat the puzle! :D");
        print("maybe you could replace this with something like openning a hatch");
        print("or dropping an item for another puzle! :D");
    }

    public void CheckParticles()
    {
        yippee = Instantiate(prefab, this.transform);
        yippee.transform.localPosition = new Vector3(0, 0.5f, -0.5f);
    }
}
