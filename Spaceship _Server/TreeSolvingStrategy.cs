using Hwdtech;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Spaceship__Server;

public class TreeDesicion
{
    public bool Decide(object[] args)
    {

        Dictionary<int, object> tree = (Dictionary<int, object>) args[0];

        List<int> deltas = (List<int>) args[1];

        int dimension = 0;

        object difftree = new Dictionary<int, object>();

        while(!(difftree is List<int>))
        {

            difftree = null;

            if(tree.TryGetValue(deltas[dimension], out difftree))
            {
                if(!(difftree is List<int>))
                {
                    tree = (Dictionary<int, object>) difftree;
                }
                else
                {
                    difftree = (List<int>) difftree;
                }
            }
            else
            {
                return false;
            }
            dimension++;
        } 

        if(((List<int>)difftree).Contains(deltas[dimension]))
        {   
            throw new Exception();
        }
        return false;
    }
}
