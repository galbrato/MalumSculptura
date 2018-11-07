using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperficieInteragivel : interagivel{
    public int Id;
    public porta Porta;

    public override void interacao2(){
        if(Vector3.Distance(plaTrans.position, trans.position) > distMin)
			return;
        Porta.interacao3(Id);
    }
}
