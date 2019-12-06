using System;
using System.Collections.Generic;

namespace Day06
{
    public class Planet
    {
        public string Name { get; }

        public Planet parent = null;
        public List<Planet> children = new List<Planet>();

        public int Distance { get; set; } = int.MaxValue;

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

        public void SetParent(Planet parent)
        {
            if (this.parent != null)
            {
                throw new Exception();
            }

            this.parent = parent;
        }

        public void AddChild(Planet child)
        {
            children.Add(child);
        }

        public IEnumerable<Planet> Connections()
        {
            if (parent != null)
            {
                yield return parent;
            }

            foreach (Planet child in children)
            {
                yield return child;
            }
        }
    }
}
