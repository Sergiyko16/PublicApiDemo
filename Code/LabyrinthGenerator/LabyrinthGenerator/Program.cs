﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Node root = new Node("Root");

            var top = new Node(root, "Top");

            root.Nodes.Add(top);

            Node middleNode = new Node(root, "Middle");
            middleNode.Nodes.Add(new Node(middleNode, "Middle_1"));
            middleNode.Nodes.Add(new Node(middleNode, "Middle_2"));

            root.Nodes.Add(middleNode);

            Node bottomNode = new Node(root, "Bottom");
            bottomNode.Nodes.Add(new Node(bottomNode, "Bottom_1"));
            bottomNode.Nodes.Add(new Node(bottomNode, "Bottom_2"));

            Node subBottonNode = new Node(bottomNode, "SubBottom");
            subBottonNode.Nodes.AddRange(new[] { new Node(subBottonNode, "SubButtom_1"), new Node(subBottonNode, "SubBottom2") });

            bottomNode.Nodes.Add(subBottonNode);

            root.Nodes.Add(bottomNode);

            Matrix matrix = RoomGenerator.Generate(root);

            matrix.Print();

            Console.ReadLine();
        }
    }

    public class RoomGenerator
    {
        public static Matrix Generate(Node parentNode)
        {
            Dictionary<Node, int> offsets = new Dictionary<Node, int>(); 

            int depth = parentNode.GetMaxDepth();

            int height = depth * (Settings.DefaultHeight + Settings.RoomCoridorLenght);

            var allNodes = new []{parentNode}.FancyFlatten(n => true, n => n.Nodes).ToArray();

            int labyrinthWidth = parentNode.GetSize();

            Matrix matrix = new Matrix(labyrinthWidth, height);

            int yOffset = 0;
            int xOffset = 0;

            foreach (var levelGroup in allNodes.GroupBy(n => n.Level))
            {
                Node currentParent = null;

                foreach (var group in levelGroup)
                {
                    if (group.Parent != null)
                    {
                        if (currentParent != group.Parent)
                        {
                            yOffset = 0;
                        }
                    }

                    currentParent = group.Parent;

                    Room room = new Room(group.GetSize());

                    var parentOffset = 0;

                    if (group.Parent != null)
                        parentOffset = offsets[group.Parent];

                    matrix.Merge(room.AsMatrix(), yOffset + parentOffset, xOffset);

                    offsets.Add(group, yOffset + parentOffset);

                    yOffset += group.GetSize() + Settings.Scale;
                }

                xOffset += Settings.DefaultHeight + Settings.RoomCoridorLenght - 1;
                yOffset = 0;
            }

            return matrix;
        }
    }
}
