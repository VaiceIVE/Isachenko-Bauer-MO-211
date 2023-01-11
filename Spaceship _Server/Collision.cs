using Hwdtech;
using System;
using System.Collections.Generic;

namespace Spaceship__Server;

public class CollisionSolver
{
    public object Solve2D(object[] args)
    {
        //Предполагается, что дерево содержит только случаи столкновений, так как они менее вероятны и мы сделаем вывод о столкновении быстрее
        return (object) Hwdtech.IoC.Resolve<bool>("Strategies.SolveTree", 
        Hwdtech.IoC.Resolve<Dictionary<int, object>>("Get.Solution.Tree"), 
        Hwdtech.IoC.Resolve<List<int>>("CalculateDeltas", args));
    }
}
