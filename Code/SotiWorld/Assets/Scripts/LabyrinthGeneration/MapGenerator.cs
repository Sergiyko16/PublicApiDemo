﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Integration;

namespace Assets.Scripts.LabyrinthGeneration
{
    public static class MapGenerator
    {
        public static Matrix GenerateMap(DeviceGroup[] deviceGroups)
        {
            Node rootNode = new Node("Default", "", "Red");

            DeviceGroup[] orderedDeviceGroups = deviceGroups.OrderBy(g => g.Level).ToArray();

            List<Node> nodes = new List<Node>();
            nodes.Add(rootNode);

            foreach (DeviceGroup deviceGroup in orderedDeviceGroups)
            {
                AddGroup(rootNode, deviceGroup, nodes);
            }

            Matrix matrix = RoomGenerator.Generate(rootNode);

            //Removing door from the first row

            foreach (List<List<Tile>> tiles in matrix.Tiles)
            {
                tiles[0].RemoveAll(t => t.TileType == TileType.Door);
            }

            return matrix;
        }

        private static void AddGroup(Node topNode, DeviceGroup deviceGroup, List<Node> processedNodes)
        {
            string[] paths = deviceGroup.Path.Split(new[] {@"\", @"\\"}, StringSplitOptions.RemoveEmptyEntries);

            Node groupNode = null;

            if (paths.Length == 1)
            {
                groupNode = new Node(topNode, deviceGroup.Path, deviceGroup.Name, deviceGroup.Icon);

                topNode.Nodes.Add(groupNode);
                processedNodes.Add(groupNode);
                return;
            }

            int indexOfName = deviceGroup.Path.LastIndexOf(@"\", StringComparison.Ordinal);

            string parentPath = deviceGroup.Path.Remove(indexOfName);

            Node parentNode = processedNodes.Single(n => n.Path == parentPath);

            groupNode = new Node(parentNode, deviceGroup.Path, deviceGroup.Name, deviceGroup.Icon);

            parentNode.Nodes.Add(groupNode);
            processedNodes.Add(groupNode);
        }
    }
}
