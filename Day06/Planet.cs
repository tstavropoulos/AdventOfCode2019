using System;
using System.Collections.Generic;

namespace Day06
{
    public class Planet
    {
        public string Name { get; }

        public Planet parent = null;
        public List<Planet> children = new List<Planet>();

        public int Tier
        {
            get
            {
                if (parent == null)
                {
                    return 0;
                }

                return parent.Tier + 1;
            }
        }

        public Planet(string name)
        {
            Name = name;
        }

        public void AddChild(Planet child)
        {
            children.Add(child);

            if (child.parent != null)
            {
                throw new Exception("Already coupled");
            }

            child.parent = this;
        }

        public IEnumerable<Planet> Connections()
        {
            foreach (Planet child in children)
            {
                yield return child;
            }

            if (parent != null)
            {
                yield return parent;
            }
        }
    }
}
