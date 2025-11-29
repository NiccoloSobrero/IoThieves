namespace AI.AI_States
{
    public class SuspiciousState : AIState
    {
        public SuspiciousState(AIController npc) : base(npc) {}

        public override void Enter()
        {
            //Text : What's that
        }

        public override void Execute()
        {
           //Guardarsi intorno
           //Camminare in giro cercando di rimanere nell'area attorno al trigger
           //Lo fa per un tot e poi torna a "cazzeggiare??"
           
           //Idea:
           //Simile a Patrol MA: 
           //   -Raggiungere il prossimo waypoint usando un simil randomWalk -> Splittare la total distance e ogni tot scegliere a caso un intorno da raggiungere
           //   -Far ruotare la visuale del Npc rispetto al displacement della linea direttaa (diretto o inverso)
        }

        public override void Exit()
        {
          //Text: Must have been the wind 
        }
    }
    
    /* Per gli Hackables
     *  - FindObjectOfType PointOfIntersts
     *  - Trova quello che ha il device impazzuto
     *  - A* per trovare la strada ed arrivarci
     *  - Fine solving --> Previous State (patrolling / suspicion)
     * 
     */
}