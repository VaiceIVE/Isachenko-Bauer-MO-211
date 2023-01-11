using System.Collections.Generic;
using System;

namespace Spaceship__Server;

public class TreeBuilder
{
    public Dictionary<int, object> tree = null;

    public TreeBuilder(object[] args)
    {
        List<string> lis = (List<string>)args[0];
        object Tree = this.tree;

        foreach (string str in lis)
        {
            string[] mas = str.Split(' ');
            List<int> list = new List<int>() { };
            foreach (var i in mas)
            {
                list.Add(Int32.Parse((string)i));
            }
            list.Reverse();
            object next = null;
            List<object> sos = new List<object>();
            foreach (int i in list)
            {
                if (next == null)
                {
                    next = new List<int>() { i };
                    sos.Add(next);
                }
                else
                {
                    if (next is List<int>)
                    {
                        List<int> prev = (List<int>)next;
                        next = new Dictionary<int, object>() { { i, prev } };
                        sos.Add(next);
                    }
                    else
                    {
                        Dictionary<int, object> prev = (Dictionary<int, object>)next;
                        next = new Dictionary<int, object>() { { i, prev } };
                        sos.Add(next);
                    }
                }
            }
            if (tree == null)
            {
                this.tree = (Dictionary<int, object>)next;
            }
            else
            {
                list.Reverse();
                sos.Reverse();
                int counter = 0;
                object treee = new Dictionary<int, object>();

                bool added = false;

                Dictionary<int, object> copytree = tree;

                while (!(treee is List<int>))
                {
                    if (copytree.TryGetValue(list[counter], out treee))
                    {
                        if (!(treee is List<int>))
                        {
                            copytree = (Dictionary<int, object>)treee;
                        }
                        else
                        {
                            treee = (List<int>)treee;
                        }
                    }
                    else
                    {
                        copytree.Add(list[counter], sos[counter + 1]);
                        added = true;
                        break;
                    }
                    counter++;
                }
                if (!added)
                {
                    ((List<int>)treee).Add(list[counter]);
                }
            }
        }

    }
}
