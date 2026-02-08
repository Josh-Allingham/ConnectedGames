using UnityEngine;

public class FootstepParticle : MonoBehaviour
{
    public Player.PlayerType InteractionType = Player.PlayerType.NULL;
    public PlayerFootsteps player;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    

    void SpawnParticles(ParticleSystem particles)
    {
        
        if (particles != null)
        {
            ParticleSystem newParticle = Instantiate(particles, transform.position, Quaternion.identity);
            //No chain reactions
            newParticle.GetComponent<FootstepParticle>().InteractionType = Player.PlayerType.NULL;
        }
            
    }
    private void OnTriggerEnter(Collider other)
    {
        if (InteractionType == Player.PlayerType.NULL)
            return;

        if (other.TryGetComponent(out FootstepParticle particle))
        {
            Player.PlayerType otherType = particle.InteractionType;
            if (otherType != Player.PlayerType.NULL && InteractionType != Player.PlayerType.NULL)
            {
                SpawnParticles(player.ParticleCrossover(InteractionType, otherType));
                Destroy(particle.gameObject);
                Destroy(this);
            }
                
            
        }
    }
}
